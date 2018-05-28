using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;

namespace Utilities
{
    public class OracleContext
    {
        public IDbConnection connection { get; set; }
        private string connectionString;
        private string connectionStringHistory;
        private string connectionStringAnalytic;

        public OracleContext()
        {
            connectionString = ConfigurationManager.ConnectionStrings["oracle"].ConnectionString;
            connectionStringHistory = ConfigurationManager.ConnectionStrings["oracle"].ConnectionString;
            connectionStringAnalytic = ConfigurationManager.ConnectionStrings["oracle"].ConnectionString;
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

        public DbConnection getConnectionAnalytic()
        {
            DbConnection db = null;
            try
            {
                db = new OracleConnection(connectionStringAnalytic);
            }
            catch (Exception e)
            {
                CLogger.write("2", "OracleContext.class", e);
            }
            return db;
        }
    }
}
