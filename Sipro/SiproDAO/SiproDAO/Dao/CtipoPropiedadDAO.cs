using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Utilities;
using Dapper;

namespace SiproDAO.Dao
{
    public class CtipoPropiedadDAO
    {
        public static bool guardarCtipoPropiedad(CtipoPropiedad ctipoPropiedad)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM ctipo_propiedad WHERE componente_tipoid=:componenteTipoId AND componente_propiedadid=:componentePropiedadid",
                        new { componenteTipoId = ctipoPropiedad.componenteTipoid, componentePropiedadid = ctipoPropiedad.componentePropiedadid });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE ctipo_propiedad SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                            "fecha_actualizacion=:fechaActualizacion WHERE componente_tipoid=:componenteTipoid AND componente_propiedadid=:componentePropiedadid",
                            ctipoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO ctipo_propiedad VALUES (:componenteTipoid, :componentePropiedadid, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)",
                            ctipoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "CtipoPropiedadDAO.class", e);
            }

            return ret;
        }

        public static bool EliminarCtipoPropiedad(CtipoPropiedad ctipoPropiedad)
        {
            bool ret = false;

            try
            {
                ret = guardarCtipoPropiedad(ctipoPropiedad);
            }
            catch (Exception e)
            {
                CLogger.write("2", "CtipoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalCtipoPropiedad(CtipoPropiedad ctipoPropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM ctipo_propiedad WHERE componente_tipoid=:componenteTipoid AND componente_propiedadid=:componentePropiedadid",
                        new { componenteTipoId = ctipoPropiedad.componenteTipoid, componentePropiedadid = ctipoPropiedad.componentePropiedadid });

                    ret = eliminado > 0 ? true : false;
                }
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("3", "CtipoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<CtipoPropiedad> getCtipoPropiedades(int componenteTipoid)
        {
            List<CtipoPropiedad> ret = new List<CtipoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<CtipoPropiedad>("SELECT * FROM ctipo_propiedad WHERE componente_tipoid=:componenteTipoid", new { componenteTipoid = componenteTipoid }).AsList<CtipoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "CtipoPropiedadDAO.class", e);
            }

            return ret;
        }
    }
}
