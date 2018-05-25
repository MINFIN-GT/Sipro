using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class InteresTipoDAO
    {
        public static long getTotalInteresTipos()
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<long>("SELECT COUNT(*) FROM INTERES_TIPO");
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "InteresTipoDAO.class", e);
            }
            return ret;
        }

        public static List<InteresTipo> getInteresTiposPagina(int pagina, int numeroInteresTipo)
        {
            List<InteresTipo> ret = new List<InteresTipo>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM INTERES_TIPO ";
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroInteresTipo + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroInteresTipo + ") + 1)");
                    ret = db.Query<InteresTipo>(query).AsList<InteresTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "InteresTipoDAO.class", e);
            }
            return ret;
        }

        public static InteresTipo getInteresTipoById(int id)
        {
            InteresTipo ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<InteresTipo>("SELECT * FROM INTERES_TIPO WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "InteresTipoDAO.class", e);
            }
            return ret;
        }         
    }
}
