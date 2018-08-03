using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Utilities;
using Dapper;

namespace SiproDAO.Dao
{
    public class ComponentePropiedadDAO
    {

        public static List<ComponentePropiedad> getComponentePropiedadesPorTipoComponentePagina(int idTipoComponente)
        {
            List<ComponentePropiedad> ret = new List<ComponentePropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT cp.* FROM componente_propiedad cp",
                        "INNER JOIN ctipo_propiedad ctp ON ctp.componente_propiedadid=cp.id",
                        "INNER JOIN componente_tipo ct ON ct.id=ctp.componente_tipoid",
                        "WHERE cp.estado=1 and ct.id=:tipoComponenteId");
                    ret = db.Query<ComponentePropiedad>(query, new { tipoComponenteId = idTipoComponente }).AsList<ComponentePropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static long getTotalComponentePropiedades()
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<long>("SELECT COUNT(*) FROM componente_propiedad p WHERE p.estado=1");
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<ComponentePropiedad> getComponentePropiedadPaginaTotalDisponibles(int pagina, int numerocomponentepropiedades, String idPropiedades)
        {
            List<ComponentePropiedad> ret = new List<ComponentePropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT cp.* FROM componente_propiedad cp",
                        (idPropiedades != null && idPropiedades.Length > 0 ? "WHERE cp.estado=1 and cp.id NOT IN (" + idPropiedades + ")" : "WHERE cp.estado=1"));
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numerocomponentepropiedades + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numerocomponentepropiedades + ") + 1)");
                    ret = db.Query<ComponentePropiedad>(query).AsList<ComponentePropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static ComponentePropiedad getComponentePropiedadPorId(int id)
        {
            ComponentePropiedad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ComponentePropiedad>("SELECT * FROM componente_propiedad WHERE id=:id",
                        new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "ComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool guardarComponentePropiedad(ComponentePropiedad componentePropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM componente_propiedad WHERE id=:id", new { id = componentePropiedad.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE componente_propiedad SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, " +
                            "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, dato_tipoid=:datoTipoid, estado=:estado WHERE id=:id", componentePropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_componente_propiedad.nextval FROM DUAL");
                        componentePropiedad.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO componente_propiedad VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, " +
                            ":datoTipoid, :estado)", componentePropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarComponentePropiedad(ComponentePropiedad componentePropiedad)
        {
            bool ret = false;
            try
            {
                componentePropiedad.estado = 0;
                componentePropiedad.fechaActualizacion = DateTime.Now;
                ret = guardarComponentePropiedad(componentePropiedad);
            }
            catch (Exception e)
            {
                CLogger.write("6", "ComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalComponentePropiedad(ComponentePropiedad componentePropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELTE FROM componente_propiedad WHERE id=:id", new { id = componentePropiedad.id });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<ComponentePropiedad> getComponentePropiedadesPagina(int pagina, int numeroComponentePropiedades, String filtro_busqueda,
            String columna_ordenada, String orden_direccion)
        {
            List<ComponentePropiedad> ret = new List<ComponentePropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT c FROM componente_propiedad c WHERE c.estado=1";
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
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroComponentePropiedades + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroComponentePropiedades + ") + 1)");

                    ret = db.Query<ComponentePropiedad>(query).AsList<ComponentePropiedad>();
                }

            }
            catch (Exception e)
            {
                CLogger.write("8", "ComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static long getTotalComponentePropiedad(String filtro_busqueda)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM componente_propiedad c WHERE c.estado=1";
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
                CLogger.write("9", "ComponentePropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<ComponentePropiedad> getComponentePropiedadesPorTipoComponente(int idTipoComponente)
        {
            List<ComponentePropiedad> ret = new List<ComponentePropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT cp.* FROM componente_propiedad cp",
                        "INNER JOIN ctipo_propiedad ctp ON ctp.componente_propiedadid=cp.id",
                        "INNER JOIN componente_tipo ct ON ct.id=ctp.componente_tipoid",
                        "WHERE ct.id=:idTipoComp AND cp.estado=1");
                    ret = db.Query<ComponentePropiedad>(query, new { idTipoComp = idTipoComponente }).AsList<ComponentePropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("10", "ComponentePropiedadDAO.class", e);
            }
            return ret;
        }
    }
}
