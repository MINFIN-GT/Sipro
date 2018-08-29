using System;
using System.Collections.Generic;
using System.Data.Common;
using Dapper;
using SiproModelCore.Models;
using Utilities;

namespace SiproDAO.Dao
{
    public class SubproductoPropiedadDAO
    {
        public static SubproductoPropiedad getSubproductoPropiedad(int codigo)
        {
            SubproductoPropiedad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<SubproductoPropiedad>("SELECT * FROM subproducto_propiedad WHERE id=:id", new { id = codigo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubproductoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool guardarSubproductoPropiedad(SubproductoPropiedad subproductoPropiedad)
        {
            bool ret = false;
            try
            {
                using(DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM subproducto_propiedad WHERE id=:id", new { id = subproductoPropiedad.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE subproducto_propiedad SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, " +
                            "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado, dato_tipoid=:datoTipoid WHERE id=:id", subproductoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_subproducto_propiedad.nextval FROM DUAL");
                        subproductoPropiedad.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO subproducto_propiedad VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, " +
                            ":fechaActualizacion, :estado, :datoTipoid)", subproductoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubproductoPropiedadDAO.class", e);
            }

            return ret;
        }

	public static bool eliminarSubproductoPropiedad(SubproductoPropiedad subproductoPropiedad) {
            bool ret = false;
            try
            {
                subproductoPropiedad.estado = 0;
                subproductoPropiedad.fechaActualizacion = DateTime.Now;
                ret = guardarSubproductoPropiedad(subproductoPropiedad);
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubproductoPropiedadDAO.class", e);
            }

		return ret;
	}

        public static List<SubproductoPropiedad> getPagina(int pagina, int registros, String filtro_busqueda, String columna_ordenada, String orden_direccion)
        {
            List<SubproductoPropiedad> ret = new List<SubproductoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT e.* FROM subproducto_propiedad e where e.estado = 1";
                    String query_a = "";

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " e.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " e.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(e.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + registros + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + registros + ") + 1)");

                    ret = db.Query<SubproductoPropiedad>(query).AsList<SubproductoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubproductoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static long getTotal(String filtro_busqueda)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM subproducto_propiedad e  where e.estado = 1";
                    String query_a = "";

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " e.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " e.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(e.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "SubproductoPropiedadDAO.class", e);
            }
            return ret;
        }

        /*public static String getJsonPorTipo(int idTipoProducto,int idSubproducto) {
            List<SubproductoPropiedad> subproductoPropiedades = getSubprodcutoPropiedadesPorTipo(idTipoProducto);
            List<HashMap<String,Object>> campos = new ArrayList<>();
            for(SubproductoPropiedad subproductoPropiedad: subproductoPropiedades){
                HashMap <String,Object> campo = new HashMap<String, Object>();
                campo.put("id", subproductoPropiedad.getId());
                campo.put("nombre", subproductoPropiedad.getNombre());
                campo.put("tipo", subproductoPropiedad.getDatoTipo().getId());
                SubproductoPropiedadValor proyectoPropiedadValor = SubproductoPropiedadValorDAO.getValorPorSubProdcutoYPropiedad(subproductoPropiedad.getId(), idSubproducto);
                if (proyectoPropiedadValor !=null ){
                    switch ((Integer) campo.get("tipo")){
                        case 1:
                            campo.put("valor", proyectoPropiedadValor.getValorString());
                            break;
                        case 2:
                            campo.put("valor", proyectoPropiedadValor.getValorEntero());
                            break;
                        case 3:
                            campo.put("valor", proyectoPropiedadValor.getValorDecimal());
                            break;
                        case 5:
                            campo.put("valor", Utils.formatDate(proyectoPropiedadValor.getValorTiempo()));
                            break;
                    }
                }
                else{
                    campo.put("valor", "");
                }
                campos.add(campo);
            }

            String response_text = CFormaDinamica.convertirEstructura(campos);
            response_text = String.join("", "\"subproductopropiedades\":",response_text);
            response_text = String.join("", "{\"success\":true,", response_text,"}");

            return response_text;
        }*/

        public static List<SubproductoPropiedad> getSubproductoPropiedadesPorTipo(int idTipoPropiedad)
        {
            List<SubproductoPropiedad> ret = new List<SubproductoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT p.* FROM subproducto_propiedad p",
                        "INNER JOIN subprodtipo_propiedad ptp ON ptp.subproducto_propiedadid=p.id",
                        "INNER JOIN subproducto_tipo pt ON ptp.subproducto_tipoid=pt.id",
                        "where pt.id=:subproductoTipoId ",
                        "and p.estado = 1");

                    ret = db.Query<SubproductoPropiedad>(query, new { subproductoId = idTipoPropiedad }).AsList<SubproductoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static SubproductoPropiedad getSubproductoPropiedadPorId(int id)
        {
            SubproductoPropiedad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<SubproductoPropiedad>("SELECT * FROM subproducto_propiedad WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "SubproductoPropiedadDAO.class", e);
            }
            return ret;
        }
    }
}
