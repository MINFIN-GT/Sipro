using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class ComponenteTipoDAO
    {
        public static List<ComponenteTipo> getComponenteTipos()
        {
            List<ComponenteTipo> ret = new List<ComponenteTipo>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<ComponenteTipo>("SELECT * FROM COMPONENTE_TIPO WHERE estado=1").AsList<ComponenteTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ComponenteTipoDAO.class", e);
            }
            return ret;
        }

        public static ComponenteTipo getComponenteTipoPorId(int id)
        {
            ComponenteTipo ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ComponenteTipo>("SELECT * FROM COMPONENTE_TIPO WHERE id=:id AND estado=1", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ComponenteTipoDAO.class", e);
            }
            return ret;
        }

        public static bool guardarComponenteTipo(ComponenteTipo componenteTipo, List<CtipoPropiedad> ctipoPropiedades)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM COMPONENTE_TIPO WHERE id=:id", new { id = componenteTipo.id });

                    if (existe > 0)
                    {
                        int guardar = db.Execute("UPDATE COMPONENTE_TIPO SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, " +
                            "usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado " +
                            "WHERE id=:id", componenteTipo);

                        ret = guardar > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_componente_tipo.nextval FROM DUAL");
                        componenteTipo.id = sequenceId;
                        int guardar = db.Execute("INSERT INTO COMPONENTE_TIPO VALUE (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, " +
                            "fechaActualizacion, estado)", componenteTipo);

                        ret = guardar > 0 ? true : false;
                    }

                    if (ret == true && ctipoPropiedades != null && ctipoPropiedades.Count > 0)
                    {
                        foreach (CtipoPropiedad propiedad in ctipoPropiedades)
                        {
                            existe = db.ExecuteScalar<int>("SELECT * FROM CTIPO_PROPIEDAD WHERE componente_tipoid=:componenteTipoId AND componente_propiedadid=:componentePropiedadId",
                                new { componenteTipoId = propiedad.componentePropiedadid, componentePropiedadId = propiedad.componentePropiedadid });

                            if (existe > 0)
                            {
                                int guardar = db.Execute("UPDATE CTIPO_PROPIEDAD SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                    "fecha_actualizacion=:fechaActualizacion WHERE componente_tipoid=:componenteTipoid AND componente_propiedadid=:componentePropiedadid", propiedad);

                                ret = guardar > 0 ? true : false;
                            }
                            else
                            {
                                int guardar = db.Execute("INSERT INTO CTIPO_PROPIEDAD VALUES (:componenteTipoid, :componentePropiedadid, :usuarioCreo, :usuarioActualizo, :fechaCreacion, " +
                                    ":fechaActualizacion)", propiedad);

                                ret = guardar > 0 ? true : false;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ComponenteTipoDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarComponenteTipo(ComponenteTipo componenteTipo)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    componenteTipo.estado = 0;

                    int guardar = db.Execute("UPDATE COMPONENTE_TIPO SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, " +
                            "usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado " +
                            "WHERE id=:id", componenteTipo);

                    ret = guardar > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "ComponenteTipoDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalComponenteTipo(ComponenteTipo componenteTipo)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM COMPONENTE_TIPO WHERE id=:id", new { id = componenteTipo });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ComponenteTipoDAO.class", e);
            }
            return ret;
        }

        public static List<ComponenteTipo> getComponenteTiposPagina(int pagina, int numerocomponentestipo, String filtro_nombre, String filtro_usuario_creo,
                String filtro_fecha_creacion, String columna_ordenada, String orden_direccion)
        {
            List<ComponenteTipo> ret = new List<ComponenteTipo>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM COMPONENTE_TIPO c WHERE c.estado = 1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " c.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuarioCreo LIKE '%", filtro_usuario_creo, "%' ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numerocomponentestipo + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numerocomponentestipo + ") + 1)");

                    ret = db.Query<ComponenteTipo>(query).AsList<ComponenteTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "ComponenteTipoDAO.class", e);
            }
            return ret;
        }

        public static long getTotalComponenteTipo(String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM ComponenteTipo c WHERE c.estado=1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " c.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuarioCreo LIKE '%", filtro_usuario_creo, "%' ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ComponenteTipoDAO.class", e);
            }
            return ret;
        }
    }
}
