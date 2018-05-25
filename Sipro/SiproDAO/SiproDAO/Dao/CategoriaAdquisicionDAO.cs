using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using System.Data.Common;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class CategoriaAdquisicionDAO
    {

        public static List<CategoriaAdquisicion> getCategoriaAdquisicion()
        {
            List<CategoriaAdquisicion> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<CategoriaAdquisicion>("SELECT * FROM CATEGORIA_ADQUISICION WHERE estado=1").AsList<CategoriaAdquisicion>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "CategoriaAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static long getTotalCategoriaAdquisicion(String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM CATEGORIA_ADQUISICION WHERE estado=1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " usuario_creo LIKE '%", filtro_usuario_creo, "%' ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    ret = db.ExecuteScalar<long>(query, new { filtro_fecha_creacion = filtro_fecha_creacion });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "CategoriaAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static List<CategoriaAdquisicion> getCategoriaAdquisicionPagina(int pagina, int numeroCategoriaAdquisicion, String filtro_nombre, String filtro_usuario_creo,
                String filtro_fecha_creacion, String columna_ordenada, String orden_direccion)
        {
            List<CategoriaAdquisicion> ret = new List<CategoriaAdquisicion>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT ca FROM CategoriaAdquisicion ca WHERE ca.estado = 1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " ca.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " ca.usuarioCreo LIKE '%", filtro_usuario_creo, "%' ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(ca.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, " ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroCategoriaAdquisicion + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroCategoriaAdquisicion + ") + 1)");

                    ret = db.Query<CategoriaAdquisicion>(query, new { filtro_fecha_creacion = filtro_fecha_creacion }).AsList<CategoriaAdquisicion>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "CategoriaAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static CategoriaAdquisicion getCategoriaPorId(int id)
        {
            CategoriaAdquisicion ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM CATEGORIA_ADQUISICION WHERE id=:id";
                    ret = db.QueryFirstOrDefault<CategoriaAdquisicion>(query, new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "CategoriaAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static bool guardarCategoria(CategoriaAdquisicion Categoria)
        {
            bool ret = false;
            int guardado = 0;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM CATEGORIA_ADQUISICION WHERE id=:id", new { id = Categoria.id });

                    if (existe > 0)
                    {
                        guardado = db.Execute("UPDATE CATEGORIA_ADQUISICION SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuario_actualizo, " +
                            "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado WHERE id=:id", Categoria);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_categoria_adquisicion.nextval FROM DUAL");
                        Categoria.id = sequenceId;
                        guardado = db.Execute("INSERT INTO CATEGORIA_ADQUISICION VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, " +
                            ":fechaActualizacion, :estado)", Categoria);
                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "CategoriaAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarCategoria(CategoriaAdquisicion Categoria)
        {
            bool ret = false;
            try
            {
                Categoria.estado = 0;
                ret = guardarCategoria(Categoria);
            }
            catch (Exception e)
            {
                CLogger.write("6", "CategoriaAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static List<CategoriaAdquisicion> getCategoriaAdquisicionLB(String lineaBase)
        {
            List<CategoriaAdquisicion> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "SELECT c* FROM CATEGORIA_ADQUISICION ca",
                    "WHERE ca.estado = 1",
                    lineaBase != null ? "AND ca.linea_base LIKE '%" + lineaBase + "%'" : "AND ca.actual=1");

                    ret = db.Query<CategoriaAdquisicion>(query).AsList<CategoriaAdquisicion>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "CategoriaAdquisicionDAO.class", e);
            }
            return ret;
        }         
    }
}
