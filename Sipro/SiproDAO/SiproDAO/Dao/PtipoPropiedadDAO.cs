using SiproModelCore.Models;
using System;
using System.Data.Common;
using Utilities;
using Dapper;
using System.Collections.Generic;

namespace SiproDAO.Dao
{
    public class PtipoPropiedadDAO
    {
        public static bool guardarPtipoPropiedad(PtipoPropiedad ptipoPropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM ptipo_propiedad WHERE proyecto_tipoid=:proyectoTipoid AND proyecto_propiedadid=:proyectoPropiedadid", 
                        new { proyectoTipoid = ptipoPropiedad.proyectoTipoid, proyectoPropiedadid = ptipoPropiedad.proyectoPropiedadid });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE ptipo_propiedad SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                            "fecha_actualizacion=:fechaActualizacion, estado=:estado WHERE proyecto_tipoid=:proyectoTipoid AND proyecto_propiedadid=:proyectoPropiedadid",
                            ptipoPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO ptipo_propiedad VALUES (:proyectoTipoid, :proyectoPropiedadid, :usuarioCreo, :usuarioActualizo, :fechaCreacion, " +
                            ":fechaActualizacion, :estado)", ptipoPropiedad);
                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PtipoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarPtipoPropiedad(PtipoPropiedad ptipoPropiedad)
        {
            bool ret = false;

            try
            {
                ptipoPropiedad.estado = 0;
                ret = guardarPtipoPropiedad(ptipoPropiedad);
            }
            catch (Exception e)
            {
                CLogger.write("2", "PtipoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalPtipoPropiedad(PtipoPropiedad ptipoPropiedad)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM ptipo_propiedad WHERE proyecto_tipoid=:proyectoTipoid AND proyecto_propiedadid=:proyectoPropiedadid",
                        ptipoPropiedad);

                    ret = eliminado > 0 ? true : false;
                }
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("3", "PtipoPropiedadDAO.class", e);
            }
            return ret;
        }

        public static List<PtipoPropiedad> getPtipoPropiedades(int proyectoTipoid)
        {
            List<PtipoPropiedad> ret = new List<PtipoPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<PtipoPropiedad>("SELECT * FROM ptipo_propiedad WHERE proyecto_tipoid=:proyectoTipoid", new { proyectoTipoid = proyectoTipoid }).AsList<PtipoPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "PtipoPropiedadDAO.class", e);
            }

            return ret;
        }
    }
}
