using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using SiproModelCore.Models;
using Utilities;

namespace SiproDAO.Dao
{
    public class CooperanteDAO
    {
        public static List<Cooperante> getCooperantes()
        {
            List<Cooperante> ret = new List<Cooperante>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Cooperante>("SELECT * FROM COOPERANTE WHERE estado=1").AsList<Cooperante>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "CooperanteDAO.class", e);
            }
            return ret;
        }

        public static bool guardarCooperante(Cooperante cooperante)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    long existe = db.ExecuteScalar<long>("SELECT COUNT(*) FROM COOPERANTE WHERE codigo=:codigo", new { codigo = cooperante.codigo });

                    if (existe > 0)
                    {
                        int result = db.Execute("UPDATE cooperante SET siglas=:siglas, nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, " +
                            "usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado " +
                            "WHERE codigo=:codigo AND ejercicio=:ejercicio", cooperante);
                        ret = result > 0 ? true : false;
                    }
                    else
                    {
                        int result = db.Execute("INSERT INTO cooperante VALUES (:codigo, :siglas, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, " +
                            ":fechaActualizacion, :estado, :ejercicio)", cooperante);
                        ret = result > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "CooperanteDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarCooperante(Cooperante cooperante)
        {
            bool ret = false;

            try
            {
                cooperante.estado = 0;
                cooperante.fechaActualizacion = DateTime.Now;
                ret = guardarCooperante(cooperante);
            }
            catch (Exception e)
            {
                CLogger.write("4", "CooperanteDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalCooperante(Cooperante cooperante)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    var result = db.Execute("DELETE FROM cooperante WHERE codigo=:codigo AND ejercicio=:ejercicio", new { codigo = cooperante.codigo, ejercicio = cooperante.ejercicio });
                    ret = result > 1 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "CooperanteDAO.class", e);
            }
            return ret;
        }

        public static List<Cooperante> getCooperantesPagina(int pagina, int numerocooperantes, String filtro_codigo, String filtro_nombre,
            String filtro_usuario_creo, String filtro_fecha_creacion, String columna_ordenada, String orden_direccion)
        {
            List<Cooperante> ret = new List<Cooperante>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM Cooperante c WHERE c.estado = 1";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " c.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_codigo != null && filtro_codigo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_CHAR(c.codigo) LIKE :filtro_codigo ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE :filtro_usuario_creo ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numerocooperantes + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numerocooperantes + ") + 1)");

                    ret = db.Query<Cooperante>(query, new { filtro_codigo = filtro_codigo, filtro_usuario_creo = filtro_usuario_creo, filtro_fecha_creacion = filtro_fecha_creacion }).AsList<Cooperante>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "CooperanteDAO.class", e);
            }
            return ret;
        }

        public static long getTotalCooperantes(String filtro_codigo, String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM Cooperante c WHERE c.estado=1";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " c.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_codigo != null && filtro_codigo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_CHAR(c.codigo) LIKE :filtro_codigo ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE :filtro_usuario_creo ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    ret = db.ExecuteScalar<long>(query, new { filtro_codigo = filtro_codigo, filtro_usuario_creo = filtro_usuario_creo, filtro_fecha_creacion = filtro_fecha_creacion });
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "CooperanteDAO.class", e);
            }
            return ret;
        }

        public static Cooperante getCooperantePorCodigo(int codigo)
        {
            Cooperante ret = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Cooperante>("SELECT c.* FROM Cooperante c WHERE c.estado = 1 and c.codigo=:codigo", new { codigo = codigo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "CooperanteDAO.class", e);
            }
            return ret;
        }
    }
}
