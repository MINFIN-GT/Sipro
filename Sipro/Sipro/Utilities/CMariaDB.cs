using System;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace Sipro.Utilities
{
    public class CMariaDB
    {
        private static MySqlConnection connection;
        private static MySqlConnection connection_analytic;
        private static String connection_string;
        private static String connection_string_local;
        private static String connection_string_analytic;
        private static String connection_string_analytic_local;

        public CMariaDB()
        {
            connection_string = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            connection_string_local = ConfigurationManager.ConnectionStrings["MySQL_local"].ConnectionString;
            connection_string_analytic = ConfigurationManager.ConnectionStrings["MySQL_Analytic"].ConnectionString;
        }

        public static Boolean connect()
        {
            try
            {
                connection = new MySqlConnection(connection_string);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                    return true;
            }
            catch 
            {
                try
                {
                    connection = new MySqlConnection(connection_string_local);
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                        return true;

                }
                catch (Exception ee)
                {
                    CLogger.writeFullConsole("Error 1 : CMariaDB.class ", ee);
                }



            }
            return false;
        }

        public static MySqlConnection getConnection()
        {
            return connection;
        }


        public static void close()
        {
            try
            {
                connection.Close();
            }
            catch (Exception e)
            {
                CLogger.writeFullConsole("Error 3 : CMariaDB.class ", e);
            }
        }

        public static long getNextID(MySqlConnection connection, String table)
        {
            long ret = -1;
            try
            {
                MySqlCommand stm = new MySqlCommand("SELECT last_id FROM uid WHERE table_name=@table",connection);
                stm.Prepare();
                stm.Parameters.AddWithValue("@table",table);
                MySqlDataReader rs=stm.ExecuteReader();
                if (rs.NextResult())
                {
                    ret = rs.GetInt64("last_id");
                }
            }
            catch (Exception e)
            {
                CLogger.writeFullConsole("Error 4: CMariaDB.class", e);
            }
            return ret;
        }


        public static Boolean saveLastID(MySqlConnection connection, String table, long last_id)
        {
            Boolean ret = false;
            try
            {
                MySqlCommand stm = new MySqlCommand("UPDATE uid SET last_id=@id WHERE table_name=@table", connection);
                stm.Prepare();
                stm.Parameters.AddWithValue("@id", last_id);
                stm.Parameters.AddWithValue("@table", table);
                if (stm.ExecuteNonQuery() > 0)
                    ret = true;
            }
            catch (Exception e)
            {
                CLogger.writeFullConsole("Error 5: CMariaDB.class", e);
            }
            return ret;
        }

        public static Boolean connectAnalytic()
        {
            try
            {
                connection = new MySqlConnection(connection_string_analytic);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                    return true;
            }
            catch
            {
                try
                {
                    connection = new MySqlConnection(connection_string_analytic_local);
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                        return true;

                }
                catch (Exception ee)
                {
                    CLogger.writeFullConsole("Error 7 : CMariaDB.class ", ee);
                }



            }            return false;
        }

        public static MySqlConnection getConnection_analytic()
        {
            return connection_analytic;
        }


        public static void close_analytic()
        {
            try
            {
                connection_analytic.Close();
            }
            catch (Exception e)
            {
                CLogger.writeFullConsole("Error 8 : CMariaDB.class ", e);
            }
        }
    }
}
