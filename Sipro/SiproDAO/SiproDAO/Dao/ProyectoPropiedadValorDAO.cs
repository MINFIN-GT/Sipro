using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class ProyectoPropiedadValorDAO
    {
        public static ProyectoPropiedadValor getValorPorProyectoYPropiedad(int idPropiedad, int idProyecto)
        {
            ProyectoPropiedadValor ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ProyectoPropiedadValor>("SELECT * FROM PROYECTO_PROPIEDAD_VALOR WHERE proyectoid=:proyectoId AND " +
                        "proyectoPropiedadid=:proyectoPropiedadId AND estado=1", new { proyectoId = idProyecto, proyectoPropiedadId = idPropiedad });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProyectoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static bool guardarProyectoPropiedadValor(ProyectoPropiedadValor proyectoPropiedadValor)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM PROYECTO_PROPIEDAD_VALOR WHERE proyectoid=:proyectoId AND " +
                        "proyecto_propiedadid=:proyectoPropiedadid", new
                        {
                            proyectoId = proyectoPropiedadValor.proyectoid,
                            proyectoPropiedadid = proyectoPropiedadValor.proyectoPropiedadid
                        });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE proyecto_propiedad_valor SET valor_string=:valorString, valor_entero=:valorEntero, valor_decimal=:valorDecimal, " +
                            "valor_tiempo=:valorTiempo, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                            "fecha_actualizacion=:fechaActualizacion, estado=:estado WHERE proyectoid=:proyectoid AND proyecto_propiedadid=:proyectoPropiedadid", proyectoPropiedadValor);
                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO proyecto_propiedad_valor VALUES (:proyectoid, :proyectoPropiedadid, :valorString, :valorEntero, " +
                            ":valorDecimal, :valorTiempo, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, estado)", proyectoPropiedadValor);
                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProyectoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarProyectoPropiedadValor(ProyectoPropiedadValor proyectoPropiedadValor)
        {
            bool ret = false;
            try
            {
                proyectoPropiedadValor.estado = 0;
                ret = guardarProyectoPropiedadValor(proyectoPropiedadValor);
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalRiesgoPropiedadValor(ProyectoPropiedadValor proyectoPropiedadValor)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM PROYECTO_PROPIEDAD_VALOR WHERE proyectoid=:proyectoid AND proyectoPropieadadid=:proyectoPropieadadid",
                        new { proyectoid = proyectoPropiedadValor.proyectoid, proyectoPropieadadid = proyectoPropiedadValor.proyectoPropiedadid });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProyectoPropiedadValorDAO.class", e);
            }
            return ret;
        }

        public static List<ProyectoPropiedadValor> getProyectoPropiedadadesValoresPorProyecto(int idProyecto)
        {
            List<ProyectoPropiedadValor> ret = new List<ProyectoPropiedadValor>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM PROYECTO_PROPIEDAD_VALOR ppv",
                        "JOIN PROYECTO p p.id=ppv.proyectoid",
                        "WHERE p.id=:idProy AND ppv.estado=1");
                    ret = db.Query<ProyectoPropiedadValor>(query, new { idProy = idProyecto }).AsList<ProyectoPropiedadValor>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProyectoPropiedadValorDAO.class", e);
            }
            return ret;
        }
    }
}
