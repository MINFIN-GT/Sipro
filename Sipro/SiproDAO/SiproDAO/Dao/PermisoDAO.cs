using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using System.Data.Common;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class PermisoDAO
    {
        public PermisoDAO()
        {
        }

        public static List<Permiso> getPermisos()
        {
            List<Permiso> ret = new List<Permiso>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Permiso>("SELECT * FROM permiso WHERE estado=1 ORDER BY id").AsList<Permiso>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PermisoDAO.class", e);
            }
            return ret;
        }

        public static bool guardarPermiso(Permiso permiso)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    long existe = db.ExecuteScalar<long>("SELECT COUNT(*) FROM PERMISO WHERE id=:id", new { permiso.id });

                    if (existe > 0)
                    {
                        db.Query<Usuario>("UPDATE PERMISO SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado WHERE id=:id", permiso);
                        ret = true;
                    }
                    else
                    {
                        db.Query<Usuario>("INSERT INTO PERMISO VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado)", permiso);
                        ret = true;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PermisoDAO", e);
            }

            return ret;
        }

        public static Permiso getPermiso(String nombrepermiso)
        {
            Permiso ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Permiso>("SELECT * FROM PERMISO WHERE nombre=:nombre", new { nombre = nombrepermiso });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PermisoDAO", e);
            }
            return ret;
        }

        public static bool eliminarPermiso(Permiso permiso)
        {
            bool ret = false;

            try
            {
                permiso.estado = 0;
                ret = guardarPermiso(permiso);
            }
            catch (Exception e)
            {
                CLogger.write("4", "PermisoDAO", e);
            }
            return ret;
        }

        public static Permiso getPermisoById(int idpermiso)
        {
            Permiso ret = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Permiso>("SELECT * FROM PERMISO WHERE id=:id", new { id = idpermiso });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PermisoDAO", e);
            }
            return ret;
        }

        public static List<Permiso> getPermisosPagina(int pagina, int numeroPermisos, String filtro_id, String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion)
        {
            List<Permiso> ret = new List<Permiso>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (Select * FROM Permiso p  where estado= :estado ";
                    String query_a = "";
                    if (filtro_id != null && filtro_id.Trim().Length > 0)
                        query_a = String.Join("", query_a, " p.id LIKE :filtro_id ");
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.nombre LIKE '%" + filtro_nombre + "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " usuario_creo LIKE :filtro_usuario_creo ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND ", query_a, "") : ""));
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroPermisos + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroPermisos + ") + 1)");
                    ret = db.Query<Permiso>(query, new { estado = 1, filtro_id = filtro_id, filtro_usuario_creo = filtro_usuario_creo, filtro_fecha_creacion = filtro_fecha_creacion }).AsList<Permiso>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "PermisoDAO", e);
            }
            return ret;
        }

        public static long getTotalPermisos(String filtro_id, String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT count(p.id) FROM Permiso p WHERE p.estado=1";
                    String query_a = "";
                    if (filtro_id != null && filtro_id.Trim().Length > 0)
                        query_a = String.Join("", query_a, " p.id LIKE :filtro_id ");
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.nombre LIKE '%" + filtro_nombre + "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " usuario_creo LIKE :filtro_usuario_creo ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND ", query_a, "") : ""));
                    ret = db.ExecuteScalar<long>(query, new { filtro_id = filtro_id, filtro_usuario_creo = filtro_usuario_creo, filtro_fecha_creacion = filtro_fecha_creacion });
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "PermisoDAO", e);
            }

            return ret;
        }
    }
}
