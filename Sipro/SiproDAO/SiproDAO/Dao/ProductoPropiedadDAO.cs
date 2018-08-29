using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;

namespace SiproDAO.Dao
{
    public class ProductoPropiedadDAO
    {
        private class Stdatadinamico
        {
            public String id;
            public String tipo;
            public String label;
            public String valor;
        }

        public static ProductoPropiedad getProductoPropiedad(int id)
        {
            ProductoPropiedad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ProductoPropiedad>("SELECT * FROM producto_propiedad WHERE id=:id AND estado=1", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProductoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool guardarProductoPropiedad(ProductoPropiedad productoPropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM producto_propiedad WHERE id=:id", new { id = productoPropiedad.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE producto_propiedad SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo," +
                            " fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, dato_tipoid=:datoTipoid, estado=:estado WHERE id=:id", productoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_producto_propiedad.nextval FROM DUAL");
                        productoPropiedad.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO producto_propiedad VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, " +
                            ":fechaActualizacion, :datoTipoid, :estado)", productoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProductoPropiedadDAO.class", e);
            }

            return ret;
        }

        public static bool eliminar(ProductoPropiedad productoPropiedad)
        {
            bool ret = false;
            try
            {
                productoPropiedad.estado = 0;
                productoPropiedad.fechaActualizacion = DateTime.Now;
                ret = guardarProductoPropiedad(productoPropiedad);
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProductoPropiedadDAO.class", e);
            }

            return ret;
        }

        public static List<ProductoPropiedad> getProductoPropiedadPagina(int pagina, int registros, String filtro_busqueda, String columna_ordenada, String orden_direccion)
        {
            List<ProductoPropiedad> ret = new List<ProductoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT e.* FROM producto_propiedad e WHERE e.estado = 1";
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

                    ret = db.Query<ProductoPropiedad>(query).AsList<ProductoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProductoPropiedadDAO.class", e);
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
                    String query = "SELECT COUNT(*) FROM producto_propiedad e WHERE e.estado = 1";
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

                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "ProductoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<ProductoPropiedad> getProductoPropiedadesPorTipo(int idProductoTipo)
        {
            List<ProductoPropiedad> ret = new List<ProductoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT p.* FROM producto_propiedad p",
                        "INNER JOIN prodtipo_propiedad ptp ON ptp.producto_propiedadid=p.id",
                        "INNER JOIN producto_tipo pt ON pt.id=ptp.producto_tipoid",
                        "WHERE pt.id=:productoTipoId AND p.estado=1");

                    ret = db.Query<ProductoPropiedad>(query, new { productoTipoId = idProductoTipo }).AsList<ProductoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ProyectoPropiedadDAO.class", e);
            }
            return ret;
        }
    }


}
