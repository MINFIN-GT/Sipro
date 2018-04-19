using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Sipro.Utilities
{
    public class OracleContext
    {
        public IDbConnection connection { get; set; }
        private string connectionString;

        public OracleContext()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
            connection = new OracleConnection(connectionString);
        }
    }
}
