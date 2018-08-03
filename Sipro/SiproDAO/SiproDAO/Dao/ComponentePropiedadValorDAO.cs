using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Utilities;
using Dapper;

namespace SiproDAO.Dao
{
    public class ComponentePropiedadValorDAO
    {

        public static ComponentePropiedadValor getValorPorComponenteYPropiedad(int idPropiedad, int idComponente)
        {
            ComponentePropiedadValor ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ComponentePropiedadValor>("SELECT * FROM componente_propiedad_valor WHERE componenteid=:componenteId AND " +
                        "componente_propiedadid=:propiedadid", new { componenteId = idComponente, propiedadid = idPropiedad });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProductoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static bool guardarComponentePropiedadValor(ComponentePropiedadValor componentePropiedadValor)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM componente_propiedad_valor WHERE componenteid=:componenteId AND componente_propiedadid=:propiedadid",
                        new { componenteId = componentePropiedadValor.componenteid, propiedadid = componentePropiedadValor.componentePropiedadid });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE componente_propiedad_valor SET valor_string=:valorString, valor_entero=:valorEntero, valor_decimal=:valorDecimal, " +
                            "valor_tiempo=:valorTiempo, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion " +
                            "WHERE componenteid=:componenteid AND componente_propiedadid=:componentePropiedadid", componentePropiedadValor);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO componente_propiedad_valor VALUES (:componenteid, :componentePropiedadid, :valorString, :valorEntero, :valorDecimal, " +
                            ":valorTiempo, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", componentePropiedadValor);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ComponentePropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalComponentePropiedadValor(ComponentePropiedadValor componentePropiedadValor)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM componente_propiedad_valor WHERE componenteid=:componenteid AND componente_propiedadid=:componentePropiedadid",
                        new { componenteid = componentePropiedadValor.componenteid, componentePropiedadid = componentePropiedadValor.componentePropiedadid });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ComponentePropiedadValorDAO.class", e);
            }
            return ret;
        }
    }
}
