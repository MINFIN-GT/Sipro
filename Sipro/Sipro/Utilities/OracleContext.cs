using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;

namespace Sipro.Utilities
{
    public class OracleContext
    {
        public IDbConnection connection { get; set; }
        private string connectionString;

        public OracleContext()
        {
            connectionString = ConfigurationManager.ConnectionStrings["oracle"].ConnectionString;
        }


        public DbConnection getConnection(){
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

    }
}
