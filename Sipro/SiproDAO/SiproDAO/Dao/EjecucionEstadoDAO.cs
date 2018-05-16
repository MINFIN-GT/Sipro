using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class EjecucionEstadoDAO
    {

        public static long getTotalEjecucionEstado()
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<long>("SELECT count(*) FROM EJECUCION_ESTADO a");
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "EjecucionEstadoDAO.class", e);
            }
            return ret;
        }

        public static List<EjecucionEstado> getEjecucionEstadosPagina(int pagina, int numeroEjecucionEstado)
        {
            List<EjecucionEstado> ret = new List<EjecucionEstado>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM EJECUCION_ESTADO";
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroEjecucionEstado + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroEjecucionEstado + ") + 1)");
                    ret = db.Query<EjecucionEstado>(query).AsList<EjecucionEstado>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "EjecucionEstadoDAO.class", e);
            }
            return ret;
        }

        public static EjecucionEstado getEjecucionEstadoById(int id)
        {
            EjecucionEstado ret = new EjecucionEstado();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<EjecucionEstado>("SELECT * FROM EJECUCION_ESTADO WHERE id=:id", id);
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "EjecucionEstadoDAO.class", e);
            }
            return ret;
        }
    }
}
