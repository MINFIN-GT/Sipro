using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Dapper;
using SiproModelCore.Models;
using Utilities;

namespace SiproDAO.Dao
{
    public class SubComponentePropiedadDAO
    {
        public static List<SubcomponentePropiedad> getSubComponentePropiedadesPorTipoSubComponente(int idTipoSubComponente)
        {
            List<SubcomponentePropiedad> ret = new List<SubcomponentePropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT p.* FROM subcomponente_propiedad p",
                        "INNER JOIN sctipo_propiedad ptp ON ptp.subcomponente_propiedad=p.id",
                        "INNER JOIN subcomponente_tipo pt ON pt.id=ptp.subcomponente_tipoid",
                        "WHERE p.estado=1 AND pt.id=:idSubcomponenteTipo");
                    ret = db.Query<SubcomponentePropiedad>(query, new { idSubcomponenteTipo = idTipoSubComponente }).AsList<SubcomponentePropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static long getTotalSubComponentePropiedades()
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<long>("SELECT COUNT(*) FROM subcomponente_propiedad p WHERE p.estado=1");
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<SubcomponentePropiedad> getSubComponentePropiedadPaginaTotalDisponibles(int pagina, int numerosubcomponentepropiedades, String idPropiedades)
        {
            List<SubcomponentePropiedad> ret = new List<SubcomponentePropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM subcomponente_propiedad p",
                        "WHERE p.estado=1",
                        idPropiedades.Length > 0 ? "AND p.id NOT IN(" + idPropiedades + ")" : "");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numerosubcomponentepropiedades + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numerosubcomponentepropiedades + ") + 1)");
                    ret = db.Query<SubcomponentePropiedad>(query).AsList<SubcomponentePropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static SubcomponentePropiedad getSubComponentePropiedadPorId(int id)
        {
            SubcomponentePropiedad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<SubcomponentePropiedad>("SELECT * FROM subcomponente_propiedad WHERE id=:id AND estado=1", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool guardarSubComponentePropiedad(SubcomponentePropiedad subcomponentePropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM subcomponente_propiedad WHERE id=:id", new { id = subcomponentePropiedad.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE subcomponente_propiedad SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, " +
                            "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, dato_tipoid=:datoTipoid, estado=:estado WHERE id=:id", subcomponentePropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_subcomponente_propiedad.nextval FROM DUAL");
                        subcomponentePropiedad.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO subcomponente_propiedad VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, " +
                            ":datoTipoid, :estado)", subcomponentePropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarSubComponentePropiedad(SubcomponentePropiedad subcomponentePropiedad)
        {
            bool ret = false;
            try
            {
                subcomponentePropiedad.estado = 0;
                ret = guardarSubComponentePropiedad(subcomponentePropiedad);
            }
            catch (Exception e)
            {
                CLogger.write("6", "SubComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalSubComponentePropiedad(SubcomponentePropiedad subcomponentePropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM subcomponente_propiedad WHERE id=:id", new { id = subcomponentePropiedad.id });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "SubComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<SubcomponentePropiedad> getSubComponentePropiedadesPagina(int pagina, int numeroSubComponentePropiedades,
                String filtro_busqueda, String columna_ordenada, String orden_direccion)
        {
            List<SubcomponentePropiedad> ret = new List<SubcomponentePropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT c FROM subcomponente_propiedad c WHERE c.estado=1";
                    String query_a = "";

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroSubComponentePropiedades + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroSubComponentePropiedades + ") + 1)");
                    ret = db.Query<SubcomponentePropiedad>(query).AsList<SubcomponentePropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "SubComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static long getTotalSubComponentePropiedad(String filtro_busqueda)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM subcomponente_propiedad c WHERE c.estado=1";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "SubComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<SubcomponentePropiedad> getSubComponentePropiedadesPorTipoComponente(int idTipoSubComponente)
        {
            List<SubcomponentePropiedad> ret = new List<SubcomponentePropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT cp.* FROM subcomponente_propiedad cp",
                        "INNER JOIN sctipo_propiedad ctp ON ctp.subcomponente_propiedadid=cp.id",
                        "INNER JOIN subcomponente_tipo ct ON ct.id=ctp.subcomponente_tipoid",
                        "WHERE ct.id=:idTipoSComp AND cp.estado=1");

                    ret = db.Query<SubcomponentePropiedad>(query, new { idTipoSComp = idTipoSubComponente }).AsList<SubcomponentePropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("10", "SubComponentePropiedadDAO.class", e);
            }
            return ret;
        }
    }
}
