using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class EntidadDAO
    {
        public static List<Entidad> getEntidades(int ejercicio)
        {
            List<Entidad> ret = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Entidad>("SELECT * FROM ENTIDAD WHERE ejercicio=:ejercicio", new { ejercicio = ejercicio }).AsList<Entidad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "EntidadDAO.class", e);
            }
            return ret;
        }

        public static Entidad getEntidad(int entidad, int ejercicio)
        {
            Entidad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Entidad>("SELECT * FROM ENTIDAD WHERE entidad=:entidad AND ejercicio=:ejercicio", new { entidad = entidad, ejercicio = ejercicio });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "EntidadDAO.class", e);
            }
            return ret;
        }

        public static bool guardarEntidad(int entidadid, int ejercicio, String nombre, String abreviatura)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    Entidad entidad = getEntidad(entidadid, ejercicio);
                    if (entidad != null)
                    {
                        entidad.abreviatura = abreviatura;
                        int result = db.Execute("UPDATE entidad SET nombre=:nombre, abreviatura=:abreviatura, ejercicio=:ejercicio WHERE entidad=:entidad", entidad);
                        ret = result > 0 ? true : false;
                    }
                    else
                    {
                        entidad = new Entidad();
                        entidad.nombre = nombre;
                        entidad.ejercicio = ejercicio;
                        entidad.abreviatura = abreviatura;
                        entidad.entidad = entidadid;

                        int result = db.Execute("INSERT INTO ENTIDAD VALUES (entidad, nombre, abreviatura, ejercicio)", entidad);
                        ret = result > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "EntidadDAO.class", e);
            }

            return ret;
        }

        public static List<Entidad> getEntidadesPagina(int pagina, int registros, String filtro_busqueda, String columna_ordenada, String orden_direccion)
        {
            List<Entidad> ret = new List<Entidad>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT e.* FROM Entidad e";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " TO_CHAR(e.entidad) LIKE '%" + filtro_busqueda + "%'");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " e.nombre LIKE'%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " e.abreviatura LIKE '%" + filtro_busqueda + "%'");                    
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "WHERE (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + registros + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + registros + ") + 1)");
                    ret = db.Query<Entidad>(query).AsList<Entidad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "EntidadDAO.class", e);
            }
            return ret;
        }

        public static long getTotalEntidades(String filtro_busqueda)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT count(*) FROM Entidad e ";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " TO_CHAR(e.entidad) LIKE '%" + filtro_busqueda + "%'");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " e.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " e.abreviatura LIKE '%" + filtro_busqueda + "%'");
                    }                    
                    query = query_a.Length > 0 ? String.Join("", query, " WHERE ", query_a) : query;

                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "EntidadDAO.class", e);
            }
            return ret;
        }
    }
}
