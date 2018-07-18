using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;
using Newtonsoft.Json;

namespace SiproDAO.Dao
{
    public class UnidadEjecutoraDAO
    {
        public class EstructuraPojo
        {
            public int unidadEjecutora;
            public String nombreUnidadEjecutora;
            public int entidad;
            public String nombreEntidad;
            public int ejercicio;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
        }

        public static UnidadEjecutora getUnidadEjecutora(int ejercicio, int entidad, int unidadEjecutora)
        {
            UnidadEjecutora ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<UnidadEjecutora>("SELECT ue.* FROM UNIDAD_EJECUTORA ue " +
                        "WHERE ue.unidad_ejecutora=:unidadEjecutora AND ue.entidadentidad=:entidad AND ue.ejercicio=:ejercicio",
                        new { unidadEjecutora = unidadEjecutora, entidad = entidad, ejercicio = ejercicio });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "UnidadEjecutoraDAO.class", e);
            }
            return ret;
        }


        public static bool guardarUnidadEjecutora(UnidadEjecutora unidadejecutora)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    long existe = db.ExecuteScalar<long>("SELECT COUNT(*) FROM UNIDAD_EJECUTORA WHERE " +
                        "unidad_ejecutora=:unidadEjecutora AND entidadentidad=:entidadentidad AND ejercicio=:ejercicio", unidadejecutora);

                    if (existe > 0)
                    {
                        int result = db.Execute("UPDATE UNIDAD_EJECUTORA SET nombre=:nombre WHERE unidad_ejecutora=:unidadEjecutora AND entidadentidad=:entidadentidad AND ejercicio=:ejercicio", unidadejecutora);
                        ret = result > 0 ? true : false;
                    }
                    else
                    {
                        int result = db.Execute("INSERT INTO UNIDAD_EJECUTORA VALUES(:unidadEjecutora, :nombre, :entidadentidad, :ejercicio)", unidadejecutora);
                        ret = result > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "UnidadEjecutoraDAO.class", e);
            }
            return ret;
        }

        public static bool guardar(int idEntidad, int ejercicio, int id, String nombre)
        {
            UnidadEjecutora pojo = getUnidadEjecutora(ejercicio, idEntidad, id);
            bool ret = false;

            if (pojo == null)
            {
                pojo = new UnidadEjecutora();
                pojo.nombre = nombre;
                pojo.entidadentidad = idEntidad;
                pojo.ejercicio = ejercicio;
                pojo.unidadEjecutora = id;

                try
                {
                    guardarUnidadEjecutora(pojo);
                    ret = true;
                }
                catch (Exception e)
                {
                    CLogger.write("3", "UnidadEjecutoraDAO.class", e);
                }
            }

            return ret;
        }

        public static bool actualizar(int idEntidad, int ejercicio, int id, String nombre)
        {
            UnidadEjecutora pojo = getUnidadEjecutora(ejercicio, idEntidad, id);
            bool ret = false;

            if (pojo != null)
            {
                pojo.nombre = nombre;
                pojo.entidadentidad = idEntidad;
                pojo.ejercicio = ejercicio;
                pojo.unidadEjecutora = id;

                try
                {
                    guardarUnidadEjecutora(pojo);
                    ret = true;
                }
                catch (Exception e)
                {
                    CLogger.write("4", "UnidadEjecutoraDAO.class", e);
                }
            }

            return ret;
        }

        public static List<UnidadEjecutora> getPagina(int pagina, int registros, int ejercicio, int entidad, string filtro_busqueda)
        {
            List<UnidadEjecutora> ret = new List<UnidadEjecutora>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT ue.* FROM UNIDAD_EJECUTORA ue " +
                        "WHERE ue.entidadentidad=:entidad and ue.ejercicio=:ejercicio");
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join(" ", query_a, "ue.nombre LIKE '%" + filtro_busqueda + "%'");

                        Int32 unidadEjecutora;
                        if (Int32.TryParse(filtro_busqueda, out unidadEjecutora))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " ue.unidad_ejecutora LIKE '%" + filtro_busqueda + "%'");
                        }
                    }
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + registros + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + registros + ") + 1)");
                    ret = db.Query<UnidadEjecutora>(query,
                        new { entidad = entidad, ejercicio = ejercicio }).AsList<UnidadEjecutora>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "UnidadEjecutoraDAO.class", e);
            }
            return ret;
        }

        public static List<UnidadEjecutora> getPaginaPorEntidad(int pagina, int registros, int entidadId, int ejercicio)
        {
            List<UnidadEjecutora> ret = new List<UnidadEjecutora>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT ue.* FROM UNIDAD_EJECUTORA ue " +
                        "WHERE ue.entidadentidad=:entidadId AND ue.ejercicio=:ejercicio");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + registros + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + registros + ") + 1)");
                    ret = db.Query<UnidadEjecutora>(query, new { entidadId = entidadId, ejercicio = ejercicio }).AsList<UnidadEjecutora>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "UnidadEjecutoraDAO.class", e);
            }
            return ret;
        }

        public static String getJson(int pagina, int registros, int ejercicio, int entidad, string filtro_busqueda)
        {
            String jsonEntidades = "";

            List<UnidadEjecutora> pojos = getPagina(pagina, registros, ejercicio, entidad, filtro_busqueda);
            List<EstructuraPojo> listaEstructuraPojos = new List<EstructuraPojo>();

            try
            {
                foreach (UnidadEjecutora pojo in pojos)
                {
                    EstructuraPojo estructuraPojo = new EstructuraPojo();
                    estructuraPojo.entidad = pojo.entidadentidad;
                    estructuraPojo.ejercicio = pojo.ejercicio;
                    estructuraPojo.unidadEjecutora = pojo.unidadEjecutora;
                    estructuraPojo.nombreUnidadEjecutora = pojo.nombre;
                    estructuraPojo.entidad = pojo.entidads.entidad;
                    estructuraPojo.nombreEntidad = pojo.entidads.nombre;
                    listaEstructuraPojos.Add(estructuraPojo);
                }
                jsonEntidades = "\"unidadesEjecutoras\" : " + JsonConvert.SerializeObject(listaEstructuraPojos);
            }
            catch (Exception e)
            {
                CLogger.write("7", "UnidadEjecutoraDAO.class", e);
            }

            return jsonEntidades;
        }

        public static String getJsonPorEntidad(int pagina, int registros, int entidadId, int ejercicio)
        {
            String jsonEntidades = "";

            List<UnidadEjecutora> pojos = getPaginaPorEntidad(pagina, registros, entidadId, ejercicio);
            List<EstructuraPojo> listaEstructuraPojos = new List<EstructuraPojo>();

            try
            {
                foreach (UnidadEjecutora pojo in pojos)
                {
                    EstructuraPojo estructuraPojo = new EstructuraPojo();
                    estructuraPojo.unidadEjecutora = pojo.unidadEjecutora;
                    estructuraPojo.nombreUnidadEjecutora = pojo.nombre;
                    estructuraPojo.entidad = pojo.entidads.entidad;
                    estructuraPojo.nombreEntidad = pojo.entidads.nombre;
                    listaEstructuraPojos.Add(estructuraPojo);
                }

                jsonEntidades = "\"unidadesEjecutoras\" : " + JsonConvert.SerializeObject(listaEstructuraPojos);
            }
            catch (Exception e)
            {
                CLogger.write("8", "UnidadEjecutoraDAO.class", e);
            }

            return jsonEntidades;
        }

        public static long getTotal(int ejercicio, int entidad, string filtro_busqueda)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = "SELECT COUNT(*) FROM UNIDAD_EJECUTORA WHERE entidadentidad=:entidad AND ejercicio=:ejercicio";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join(" ", query_a, "nombre LIKE '%" + filtro_busqueda + "%'");

                        Int32 unidadEjecutora;
                        if (Int32.TryParse(filtro_busqueda, out unidadEjecutora))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " unidad_ejecutora LIKE '%" + filtro_busqueda + "%'");
                        }
                    }
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    ret = db.ExecuteScalar<long>(query, new { entidad = entidad, ejercicio = ejercicio });
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "UnidadEjecutoraDAO.class", e);
            }
            return ret;
        }

        public static List<UnidadEjecutora> getUnidadEjecutoras(int ejercicio, int entidad)
        {
            List<UnidadEjecutora> ret = new List<UnidadEjecutora>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<UnidadEjecutora>("SELECT ue.* FROM UNIDAD_EJECUTORA ue INNER JOIN ENTIDAD e ON e.entidad=ue.entidadentidad " +
                        "WHERE ue.entidadentidad=:entidad AND ue.ejercicio=:ejercicio",
                        new { entidad = entidad, ejercicio = ejercicio }).AsList<UnidadEjecutora>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("10", "UnidadEjecutoraDAO.class", e);
            }
            return ret;
        }
    }
}
