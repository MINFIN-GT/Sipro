using System;
using System.Collections.Generic;
using System.Data.Common;
using Dapper;
using SiproModelCore.Models;
using Utilities;

namespace SiproDAO.Dao
{
    public class SctipoPropiedadDAO
    {
        public static bool guardarSctipoPropiedad(SctipoPropiedad sctipoPropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM sctipo_propiedad WHERE subcomponente_tipoid=:subcomponenteTipoId " +
                        "AND subcomponente_propiedadid=:subcomponentePropiedadId", new { subcomponenteTipoId = sctipoPropiedad.subcomponenteTipoid,
                            subcomponentePropiedadId = sctipoPropiedad.subcomponentePropiedadid });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE sctipo_propiedad SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                            "fecha_actualizacion=:fechaActualizacion WHERE subcomponente_tipoid=:subcomponenteTipoid AND subcomponente_propiedadid=:subcomponentePropiedadid",
                            sctipoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO sctipo_propiedad VALUES (:subcomponenteTipoid, :subcomponentePropiedadid, :usuarioCreo, :usuarioActualizo, " +
                            ":fechaCreacion, :fechaActualizacion)", sctipoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "SctipoPropiedadDAO.class", e);
            }

            return ret;
        }


        public static bool eliminarSctipoPropiedad(SctipoPropiedad sctipoPropiedad)
        {
            bool ret = false;
            try
            {
                ret = guardarSctipoPropiedad(sctipoPropiedad);
            }
            catch (Exception e)
            {
                CLogger.write("2", "SctipoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalSctipoPropiedad(SctipoPropiedad sctipoPropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM sctipo_propiedad WHERE subcomponente_tipoid=:subcomponenteTipoid AND " +
                        "subcomponente_propiedadid=:subcomponentePropiedadid", new
                        {
                            subcomponenteTipoid = sctipoPropiedad.subcomponenteTipoid,
                            subcomponentePropiedadid = sctipoPropiedad.subcomponentePropiedadid
                        });

                    ret = eliminado > 0 ? true : false;
                }
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("3", "SctipoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<SctipoPropiedad> getSctipoPropiedades(int subcomponenteTipoid)
        {
            List<SctipoPropiedad> ret = new List<SctipoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<SctipoPropiedad>("SELECT * FROM sctipo_propiedad WHERE subcomponente_tipoid=:subcomponenteTipoid", new { subcomponenteTipoid = subcomponenteTipoid }).AsList<SctipoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "SctipoPropiedadDAO.class", e);
            }

            return ret;
        }
    }
}
