using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.Common;
using Utilities;

namespace SiproDAO.Dao
{
    public class SubcomponentePropiedadValorDAO
    {
        public static SubcomponentePropiedadValor getValorPorSubComponenteYPropiedad(int idPropiedad, int idSubComponente)
        {
            SubcomponentePropiedadValor ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<SubcomponentePropiedadValor>("SELECT * FROM subcomponente_propiedad_valor " +
                        "WHERE subcomponenteid=:idSubComponente AND subcomponente_propiedadid=:propiedadid", new { idSubComponente = idSubComponente, propiedadid = idPropiedad });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubComponentePropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static bool guardarSubComponentePropiedadValor(SubcomponentePropiedadValor subcomponentePropiedadValor)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM subcomponente_propiedad_valor WHERE subcomponenteid=:idSubComponente AND subcomponente_propiedadid=:propiedadid",
                        new { idSubComponente = subcomponentePropiedadValor.subcomponenteid, propiedadid = subcomponentePropiedadValor.subcomponentePropiedadid });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE subcomponente_propiedad_valor SET valor_string=:valorString, valor_entero=:valorEntero, valor_decimal=:valorDecimal, " +
                            "valor_tiempo=:valorTiempo, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion " +
                            "WHERE subcomponenteid=:subcomponenteid AND subcomponente_propiedadid=:subcomponentePropiedadid", new { subcomponenteid = subcomponentePropiedadValor .subcomponenteid,
                                subcomponentePropiedadid = subcomponentePropiedadValor.subcomponentePropiedadid});

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO subcomponente_propiedad_valor VALUES (:subcomponenteid, :subcomponentePropiedadid, :valorString, :valorEntero, :valorDecimal, " +
                            ":valorTiempo, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", subcomponentePropiedadValor);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubComponentePropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalSubComponentePropiedadValor(SubcomponentePropiedadValor subcomponentePropiedadValor)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM subcomponente_propiedad_valor WHERE subcomponenteid=:subcomponenteid AND subcomponente_propiedadid=:subcomponentePropiedadid",
                        new
                        {
                            subcomponenteid = subcomponentePropiedadValor.subcomponenteid,
                            subcomponentePropiedadid = subcomponentePropiedadValor.subcomponentePropiedadid
                        });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubComponentePropiedadValorDAO.class", e);
            }
            return ret;
        }
    }
}
