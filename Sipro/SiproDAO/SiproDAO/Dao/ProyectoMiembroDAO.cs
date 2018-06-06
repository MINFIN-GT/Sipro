using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using System.Data.Common;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class ProyectoMiembroDAO
    {
        public static ProyectoMiembro getProyectoMiembro(int idProyecto, int colaborador)
        {
            ProyectoMiembro ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ProyectoMiembro>("SELECT * FROM PROYECTO_MIEMBRO WHERE proyectoid=:idProyecto AND colaboradorid=:colaboradorId AND estado=1",
                        new { idProyecto = idProyecto, colaboradorId = colaborador });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProyectoMiembroDAO.class", e);
            }
            return ret;
        }

        public static bool guardarProyectoMiembro(ProyectoMiembro ProyectoMiembro)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM PROYECTO_MIEMBRO WHERE proyectoid=:proyectoid WHERE colaboradorid=:colaboradorid", ProyectoMiembro);

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE PROYECTO_MIEMBRO SET estado=:estado, fecha_creacion=:fechaCreacion, fecha_actualizacion=fechaActualizacion, " +
                            "usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo WHERE proyectoid=:proyectoid AND colaboradorid=:colaboradorid", ProyectoMiembro);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO PROYECTO_MIEMBRO VALUES (:proyectoid, :colaboradorid, :estado, :fechaCreacion, :fechaActualizacion, :usuarioCreo, " +
                            ":usuarioActualizo)", ProyectoMiembro);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProyectoMiembroDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarProyectoMiembro(ProyectoMiembro ProyectoMiembro)
        {
            bool ret = false;
            try
            {
                ProyectoMiembro.estado = 0;
                guardarProyectoMiembro(ProyectoMiembro);
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoMiembroDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarTotalProyectoMiembro(ProyectoMiembro ProyectoMiembro)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM PROYECTO_MIEMBRO WHERE proyectoid=:proyectoid AND colaboradorid=:colaboradorid", ProyectoMiembro);

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProyectoMiembroDAO.class", e);
            }
            return ret;
        }

        public static List<ProyectoMiembro> getProyectoMiembroPorProyecto(int idProyecto)
        {
            List<ProyectoMiembro> ret = new List<ProyectoMiembro>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM PROYECTO_MIEMBRO pm",
                        "JOIN PROYECTO p on p.id=pm.proyectoid",
                        "WHERE p.id=:idProy AND pm.estado=1");
                    ret = db.Query<ProyectoMiembro>(query, new { idProy = idProyecto }).AsList<ProyectoMiembro>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProyectoMiembroDAO.class", e);
            }
            return ret;
        }
    }
}
