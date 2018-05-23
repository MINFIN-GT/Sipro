using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using System.Data.Common;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class RolDAO
    {

        public static List<Rol> getRoles()
        {
            List<Rol> ret = new List<Rol>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Rol>("Select * FROM Rol WHERE estado=1").AsList<Rol>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadDAO", e);
            }

            return ret;
        }

        public static Rol getRol(int id)
        {
            Rol ret = new Rol();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Rol>("Select * FROM Rol WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadDAO", e);
            }
            return ret;
        }

        public static RolUsuarioProyecto getRolUser(String user)
        {
            RolUsuarioProyecto ret = new RolUsuarioProyecto();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<RolUsuarioProyecto>("Select * FROM RolUsuarioProyecto WHERE usuario=:usuario", new { usuario = user });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "RolDAO", e);
            }
            return ret;
        }

        /*public static Cooperante getCooperante(String user)
    {
        Cooperante ret = new Cooperante();

        try
        {
                    using (DbConnection db = new OracleContext().getConnection())
                    {
                        ret = db.QueryFirstOrDefault<Cooperante>("SELECT c.* FROM Cooperante c INNER JOIN ProyectoUsuario p ON ")
                    }
                        String query = "Select p.proyecto.cooperante from ProyectoUsuario p where p.id.usuario=:usuario";
            Query<Cooperante> criteria = session.createQuery(query, Cooperante.class);
                criteria.setParameter("usuario",user);
                tmp = criteria.getResultList();
                ret = tmp.get(0);
            }
            catch(Exception e){
                CLogger.write("1", ActividadDAO.class, e);
            }
            return ret;
        }*/

        public static List<RolPermiso> getPermisosPorRol(int rolid)
        {
            List<RolPermiso> ret = new List<RolPermiso>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<RolPermiso>("SELECT * FROM RolPermiso WHERE estado=1 and rolid=:rolid", new { rolid = rolid }).AsList<RolPermiso>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadDAO", e);
            }
            return ret;
        }
    }
}
