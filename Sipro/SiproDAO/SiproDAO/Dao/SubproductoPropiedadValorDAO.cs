using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;

namespace SiproDAO.Dao
{
    public class SubproductoPropiedadValorDAO
    {
        public static SubproductoPropiedadValor getSubproductoPropiedadValor(int propiedadId, int subproductoId)
        {
            SubproductoPropiedadValor ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<SubproductoPropiedadValor>("SELECT * FROM subproducto_propiedad_valor WHERE subproductoid=:subproductoId AND subproducto_propiedadid=:propiedadId",
                        new { subproductoId = subproductoId, propiedadId = propiedadId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubproductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static bool guardarSubproductoPropiedadValor(SubproductoPropiedadValor subproductoPropiedadValor)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM subproducto_propiedad_valor WHERE subproductoid=:subproductoId AND subproducto_propiedadid=:propiedadId",
                        new { subproductoId = subproductoPropiedadValor.subproductoid, propiedadId = subproductoPropiedadValor.subproductoPropiedadid });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE subproducto_propiedad_valor SET valor_entero=:valorEntero, valor_string=:valorString, valor_decimal=:valorDecimal, " +
                            "valor_tiempo=:valorTiempo, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                            "estado=:estado WHERE subproductoid=:subproductoid AND subproducto_propiedadid=:subproductoPropiedadid", subproductoPropiedadValor);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO subproducto_propiedad_valor VALUES (:subproductoid, :subproducto_propiedadid, :valorEntero, :valorString, " +
                            ":valorDecimal, :valorTiempo, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado)", subproductoPropiedadValor);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubproductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarSubproductoPropiedad(SubproductoPropiedadValor subproductoPropiedadValor)
        {
            bool ret = false;
            try
            {
                subproductoPropiedadValor.estado = 0;
                subproductoPropiedadValor.fechaActualizacion = DateTime.Now;
                ret = guardarSubproductoPropiedadValor(subproductoPropiedadValor);
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubproductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static List<SubproductoPropiedadValor> getPagina(int pagina, int registros, int subproductoId)
        {
            List<SubproductoPropiedadValor> ret = new List<SubproductoPropiedadValor>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT e.* FROM subproducto_propiedad_valor e " +
                        "WHERE e.subproductoid=:subproductoId");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + registros + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + registros + ") + 1)");

                    ret = db.Query<SubproductoPropiedadValor>(query, new { subproductoId = subproductoId }).AsList<SubproductoPropiedadValor>(); ;
                }

            }
            catch (Exception e)
            {
                CLogger.write("5", "SubproductoPropiedadValorDAO.class", e);
            }

            return ret;
        }

        /*public static String getJson(int pagina, int registros, Integer productoId) {
            String jsonEntidades = "";

            List<SubproductoPropiedadValor> pojos = getPagina(pagina, registros, productoId);

            List<EstructuraPojo> listaEstructuraPojos = new ArrayList<EstructuraPojo>();

            for (SubproductoPropiedadValor pojo : pojos) {
                EstructuraPojo estructuraPojo = new EstructuraPojo();

                estructuraPojo.productoid = pojo.getSubproducto().getId();
                estructuraPojo.propiedadid = pojo.getSubproductoPropiedad().getId();

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

        public static long getTotal(int subproductoId)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<long>("SELECT COUNT(*) FROM subproducto_propiedad_valor WHERE subproductoid=:subproductoId", new { subproductoId = subproductoId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "SubproductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static List<SubproductoPropiedadValor> getSubproductoPropiedadValor(int subproductoId)
        {
            List<SubproductoPropiedadValor> ret = new List<SubproductoPropiedadValor>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM subproducto_propiedad_valor e WHERE e.subproductoid=:id");
                    ret = db.Query<SubproductoPropiedadValor>(query, new { id = subproductoId }).AsList<SubproductoPropiedadValor>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "SubproductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        /*public static String getJson(Integer productoId) {
            String jsonEntidades = "";

            List<SubproductoPropiedadValor> pojos = getSubproductoPropiedadValor(productoId);

            List<EstructuraPojo> listaEstructuraPojos = new ArrayList<EstructuraPojo>();

            for (SubproductoPropiedadValor pojo : pojos) {
                EstructuraPojo estructuraPojo = new EstructuraPojo();

                estructuraPojo.productoid = pojo.getSubproducto().getId();
                estructuraPojo.propiedadid = pojo.getSubproductoPropiedad().getId();

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



        public static SubproductoPropiedadValor getValorPorSubProdcutoYPropiedad(int idPropiedad, int idSubProducto)
        {
            SubproductoPropiedadValor ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM subproducto_propiedad_valor WHERE subproductoid=:subproductoid AND subproducto_propiedadid=:propiedadid");

                    ret = db.QueryFirstOrDefault<SubproductoPropiedadValor>(query, new { subproductoid = idSubProducto, propiedadid = idPropiedad });
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "SubproductoPropiedadValorDAO.class", e);
            }
            return ret;
        }
    }
}
