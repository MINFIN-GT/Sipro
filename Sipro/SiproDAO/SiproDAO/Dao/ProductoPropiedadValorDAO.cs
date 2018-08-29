using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Utilities;
using Dapper;

namespace SiproDAO.Dao
{
    public class ProductoPropiedadValorDAO
    {
        /*
        static class EstructuraPojo {
		Integer propiedadid;
		Integer productoid;

		Integer valorEntero;
		String valorString;
		BigDecimal valorDecimal;
		Date valorTiempo;

		String estado;
	}
	class stdatadinamico {
		String id;
		String tipo;
		String label;
		String valor;
	}*/

        public static ProductoPropiedadValor getProductoPropiedadValor(int propiedadId, int productoId)
        {
            ProductoPropiedadValor ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ProductoPropiedadValor>("SELECT * FROM producto_propiedad_valor WHERE producto_propiedadid=:productoPropiedadId AND productoid=:productoId",
                        new { productoPropiedadId = propiedadId, productoId = productoId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static bool guardarProductoPropiedadValor(ProductoPropiedadValor productoPropiedadValor)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM producto_propiedad_valor WHERE producto_propiedadid=:productoPropiedadId AND productoid=:productoId",
                        new { productoPropiedadId = productoPropiedadValor.productoid, productoId = productoPropiedadValor.productoid });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE producto_propiedad_valor SET valor_entero=:valorEntero, valor_string=:valorString, valor_decimal=:valorDecimal, " +
                            "valor_tiempo=:valorTiempo, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                            "estado=:estado WHERE producto_propiedadid=:productoPropiedadid AND productoid=:productoid", productoPropiedadValor);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO producto_propiedad_valor VALUES (:productoPropiedadid, :productoid, :valorEntero, :valorString, " +
                            ":valorDecimal, :valorTiempo, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado)", productoPropiedadValor);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProductoPropiedadValorDAO.class", e);
            }

            return ret;
        }

        public static bool eliminarProductoPropiedadValor(int propiedadId, int productoId)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM producto_propiedad_valor WHERE producto_propiedadid=:productoPropiedadId AND productoid=:productoId",
                        new { productoPropiedadId = propiedadId, productoId = productoId });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProductoPropiedadValorDAO.class", e);
            }

            return ret;
        }

        public static List<ProductoPropiedadValor> getPagina(int pagina, int registros, int productoId)
        {
            List<ProductoPropiedadValor> ret = new List<ProductoPropiedadValor>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT e.* FROM producto_propiedad_valor e WHERE e.productoid=:productoId");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + registros + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + registros + ") + 1)");

                    ret = db.Query<ProductoPropiedadValor>(query, new { productoId = productoId }).AsList<ProductoPropiedadValor>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        /*public static String getJson(int pagina, int registros, Integer productoId) {
            String jsonEntidades = "";

            List<ProductoPropiedadValor> pojos = getPagina(pagina, registros, productoId);

            List<EstructuraPojo> listaEstructuraPojos = new ArrayList<EstructuraPojo>();

            for (ProductoPropiedadValor pojo : pojos) {
                EstructuraPojo estructuraPojo = new EstructuraPojo();

                estructuraPojo.productoid = pojo.getProducto().getId();
                estructuraPojo.propiedadid = pojo.getProductoPropiedad().getId();

                estructuraPojo.valorEntero = pojo.getValorEntero();
                estructuraPojo.valorString = pojo.getValorString();
                estructuraPojo.valorDecimal = pojo.getValorDecimal();
                estructuraPojo.valorTiempo = pojo.getValorTiempo();

                estructuraPojo.estado = "C";

                listaEstructuraPojos.add(estructuraPojo);
            }

            jsonEntidades = Utils.getJSonString("productoTipos", listaEstructuraPojos);

            return jsonEntidades;
        }*/

        public static long getTotal()
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<long>("SELECT COUNT(*) FROM producto_propiedad_valor");
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ProductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static List<ProductoPropiedadValor> getProductoPropiedadValor(int productoId)
        {
            List<ProductoPropiedadValor> ret = new List<ProductoPropiedadValor>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM producto_propiedad_valor WHERE productoid=:productoId",
                        new { productoId = productoId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "ProductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        /*public static String getJson(Integer productoId) {
            String jsonEntidades = "";

            List<ProductoPropiedadValor> pojos = getProductoPropiedadValor(productoId);

            List<EstructuraPojo> listaEstructuraPojos = new ArrayList<EstructuraPojo>();

            for (ProductoPropiedadValor pojo : pojos) {
                EstructuraPojo estructuraPojo = new EstructuraPojo();

                estructuraPojo.productoid = pojo.getProducto().getId();
                estructuraPojo.propiedadid = pojo.getProductoPropiedad().getId();

                estructuraPojo.valorEntero = pojo.getValorEntero();
                estructuraPojo.valorString = pojo.getValorString();
                estructuraPojo.valorDecimal = pojo.getValorDecimal();
                estructuraPojo.valorTiempo = pojo.getValorTiempo();
                estructuraPojo.estado = "C";

                listaEstructuraPojos.add(estructuraPojo);
            }

            jsonEntidades = Utils.getJSonString("productoTipos", listaEstructuraPojos);

            return jsonEntidades;
        }*/

        public static ProductoPropiedadValor getValorPorProdcutoYPropiedad(int idPropiedad, int idProducto)
        {
            ProductoPropiedadValor ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ProductoPropiedadValor>("SELECT * FROM producto_propiedad_valor WHERE producto_propiedadid=:productoPropiedadId AND productoid=:productoId",
                        new { productoPropiedadId = idPropiedad, productoId = idProducto });
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "ProductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static ProductoPropiedadValor getValorPorProductoYPropiedad(int idPropiedad, int idProducto)
        {
            ProductoPropiedadValor ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ProductoPropiedadValor>("SELECT * FROM producto_propiedad_valor " +
                        "WHERE productoid=:idProducto AND producto_propiedadid=:propiedadid", new { idProducto = idProducto, propiedadid = idPropiedad });
                }
            }
            catch (Exception e)
            {
                CLogger.write("10", "ProductoPropiedadValorDAO.class", e);
            }
            return ret;
        }
    }
}
