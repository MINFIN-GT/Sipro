using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class ProyectoPropiedadDAO
    {
        public static List<ProyectoPropiedad> getProyectoPropiedadesPorTipoProyectoPagina(int pagina, int idTipoProyecto)
        {
            List<ProyectoPropiedad> ret = new List<ProyectoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM proyecto_propiedad p",
                        "INNER JOIN ptipo_propiedad ptp ON ptp.proyecto_propiedadid=p.id",
                        "INNER JOIN proyecto_tipo pt ON pt.id=ptp.proyecto_tipoid",
                        "WHERE pt.id=:idTipoProyecto AND p.estado=1", new { idTipoProyecto = idTipoProyecto });
                    ret = db.Query<ProyectoPropiedad>(query).AsList<ProyectoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static long getTotalProyectoPropiedades(String filtro_busqueda)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM proyecto_propiedad p WHERE p.estado=1 ";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " p.nombre LIKE '%" + filtro_busqueda + "%' ");

                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<ProyectoPropiedad> getProyectoPropiedadPaginaTotalDisponibles(int pagina, int numeroproyectopropiedades, String idPropiedades,
                int numeroElementos)
        {
            List<ProyectoPropiedad> ret = new List<ProyectoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM proyecto_propiedad p WHERE p.estado=1",
                        (idPropiedades != null && idPropiedades.Length > 0) ? "AND p.id not in (:idPropiedades)" : "");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroproyectopropiedades + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroproyectopropiedades + ") + 1)");

                    ret = db.Query<ProyectoPropiedad>(query, new { idPropiedades = idPropiedades }).AsList<ProyectoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<ProyectoPropiedad> getProyectoPropiedadesPorTipoProyecto(int idTipoProyecto)
        {
            List<ProyectoPropiedad> ret = new List<ProyectoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT pp.* FROM proyecto_tipo pt",
                        "INNER JOIN ptipo_propiedad ptp ON ptp.proyecto_tipoid = pt.id",
                        "INNER JOIN proyecto_propiedad pp ON pp.id=ptp.proyecto_propiedadid",
                        "WHERE pt.id=:idTipoProy");
                    ret = db.Query<ProyectoPropiedad>(query, new { idTipoProy = idTipoProyecto }).AsList<ProyectoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static ProyectoPropiedad getProyectoPropiedadPorId(int id)
        {
            ProyectoPropiedad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ProyectoPropiedad>("SELECT * FROM PROYECTO_PROPIEDAD WHERE id=:id AND estado=1", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static long getTotalProyectoPropiedadesDisponibles(String idPropiedades)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT COUNT(*) FROm proyecto_propiedad WHERE estado=1",
                        idPropiedades != null && idPropiedades.Length > 0 ? "AND id not in (:idPropiedades)" : "");
                    ret = db.ExecuteScalar<long>(query, new { idPropiedades = idPropiedades });
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<ProyectoPropiedad> getProyectoPropiedadesPagina(int pagina, int numeroProyectoPropiedades, String filtro_busqueda, String columna_ordenada,
            String orden_direccion)
        {
            List<ProyectoPropiedad> ret = new List<ProyectoPropiedad>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM proyecto_propiedad p where p.estado = 1 ";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " p.nombre LIKE '%" + filtro_busqueda + "%' ");

                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(" + fecha_creacion.ToString("dd/MM/yyyy") + ",'DD/MM/YY') ");
                        }
                    }
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroProyectoPropiedades + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroProyectoPropiedades + ") + 1)");

                    ret = db.Query<ProyectoPropiedad>(query).AsList<ProyectoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool guardarProyectoPropiedad(ProyectoPropiedad proyectoPropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM proyecto_propiedad WHERE id=:id", new { id = proyectoPropiedad.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE proyecto_propiedad SET nombre=:nombre, descripcion=:descripcion, dato_tipoid=:datoTipoid, " +
                            "usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                            "estado=:estado WHERE id=:id", proyectoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_proyecto_propiedad.nextval FROM DUAL");
                        proyectoPropiedad.id = sequenceId;

                        int guardado = db.Execute("INSERT INTO proyecto_propiedad VALUES (:id, :nombre, :descripcion, :datoTipoid, :usuarioCreo, :usuarioActualizo, " +
                            ":fechaCreacion, :fechaActualizacion, :estado)", proyectoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarProyectoPropiedad(ProyectoPropiedad proyectoPropiedad)
        {
            bool ret = false;
            try
            {
                proyectoPropiedad.estado = 0;
                ret = guardarProyectoPropiedad(proyectoPropiedad);
            }
            catch (Exception e)
            {
                CLogger.write("9", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalProyectoPropiedad(ProyectoPropiedad proyectoPropiedad)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM proyecto_propiedad WHERE id=:id", new { id = proyectoPropiedad.id });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("10", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }
    }
}
