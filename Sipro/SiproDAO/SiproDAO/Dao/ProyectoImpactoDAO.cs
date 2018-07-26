using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class ProyectoImpactoDAO
    {
        public static ProyectoImpacto getProyectoImpacto(int idProyecto, int entidad)
        {
            ProyectoImpacto ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ProyectoImpacto>("SELECT * FROM PROYECTO_IMPACTO WHERE proyectoid=:idProyecto AND entidadentidad=:entidad AND ejercicio=:ejercicio",
                        new { proyectoid = idProyecto, entidadentidad = entidad });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProyectoImpactoDAO.class", e);
            }
            return ret;
        }

        public static bool guardarProyectoImpacto(ProyectoImpacto proyectoImpacto)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM PROYECTO_IMPACTO WHERE proyectoid=:proyectoid AND entidadentidad=:entidadentidad AND ejercicio=:ejercicio", proyectoImpacto);

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE proyecto_impacto SET proyectoid=:proyectoid, entidadentidad=:entidadentidad, impacto=:impacto, estado=:estado, " +
                            "usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                            "ejercicio=:ejercicio WHERE id=:id", proyectoImpacto);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO proyecto_impacto VALUES (:proyectoid, :entidadentidad, :impacto, :estado, :usuarioCreo, :usuarioActualizo, " +
                            ":fechaCreacion, :fechaActualizacion, :ejercicio)", proyectoImpacto);

                        ret = guardado > 0 ? true : false;
                    }
                }
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProyectoImpactoDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarProyectoImpacto(ProyectoImpacto proyectoImpacto)
        {
            bool ret = false;
            try
            {
                proyectoImpacto.estado = 0;
                ret = guardarProyectoImpacto(proyectoImpacto);
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoImpactoDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalProyectoImpacto(ProyectoImpacto proyectoImpacto)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM PROYECTO_IMPACTO WHERE proyectoid=:proyectoid AND entidadentidad=:entidadentidad AND ejercicio=:ejercicio", proyectoImpacto);

                    ret = eliminado > 0 ? true : false;
                }
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProyectoImpactoDAO.class", e);
            }
            return ret;
        }

        public static List<ProyectoImpacto> getProyectoImpactoPorProyecto(int idProyecto)
        {
            List<ProyectoImpacto> ret = new List<ProyectoImpacto>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM PROYECTO_IMPACTO pi",
                        "JOIN PROYECTO p ON p.id=pi.proyectoid",
                        "WHERE p.id=:idProy AND pi.estado=1");

                    ret = db.Query<ProyectoImpacto>(query, new { idProy = idProyecto }).AsList<ProyectoImpacto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProyectoImpactoDAO.class", e);
            }
            return ret;
        }         
    }
}
