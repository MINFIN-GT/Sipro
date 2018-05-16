using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.Common;
using SiproModelCore.Models;
using Utilities;

namespace SiproDAO.Dao
{
    public class AutorizacionTipoDAO
    {
        public static long getTotalAuotirzacionTipo()
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<long>("SELECT COUNT(*) FROM AUTORIZACION_TIPO");
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "AutorizacionTipoDAO.class", e);
            }
            return ret;
        }


        public static List<AutorizacionTipo> getAutorizacionTiposPagina(int pagina, int numeroAutorizacionTipo)
        {
            List<AutorizacionTipo> ret = new List<AutorizacionTipo>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT a FROM AutorizacionTipo a ";
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroAutorizacionTipo + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroAutorizacionTipo + ") + 1)");
                    ret = db.Query<AutorizacionTipo>(query).AsList<AutorizacionTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "AutorizacionTipoDAO.class", e);
            }
            return ret;
        }

        public static AutorizacionTipo getAutorizacionTipoById(int id)
        {
            AutorizacionTipo ret = new AutorizacionTipo();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<AutorizacionTipo>("SELECT * FROM AUTORIZACION_TIPO WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "AutorizacionTipoDAO.class", e);
            }

            return ret;
        }
    }
}
