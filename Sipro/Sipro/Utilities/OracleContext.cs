using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;

namespace Sipro.Utilities
{
    public class OracleContext
    {
        private string connectionString;
        private string connectionStringHistory;

        public OracleContext()
        {
            connectionString = ConfigurationManager.ConnectionStrings["oracle"].ConnectionString;
            connectionStringHistory = ConfigurationManager.ConnectionStrings["oracle"].ConnectionString;
        }


        public DbConnection getConnection()
        {
            DbConnection db = null;
            try
            {
                db = new OracleConnection(connectionString);
            }
            catch (Exception e)
            {
                CLogger.write("1", "OracleContext.class", e);
            }
            return db;
        }

        public DbConnection getConnectionHistory()
        {
            DbConnection db = null;
            try
            {
                db = new OracleConnection(connectionStringHistory);
            }
            catch (Exception e)
            {
                CLogger.write("2", "OracleContext.class", e);
            }
            return db;
        }
    }
}
