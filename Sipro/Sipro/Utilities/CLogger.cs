using System;
using System.Reflection;
using log4net;

namespace Sipro.Utilities
{
    public class CLogger
    {
        private static ILog log;

        public CLogger()
        {

        }

        static public void write(String str, Object obj, Exception e)
        {
            log =  LogManager.GetLogger(obj.GetType());
            log.Error(str, e);
        }

        static public void write_simple(String error_num, Object obj, String error)
        {
            log = LogManager.GetLogger(obj.GetType());
            log.Error(String.Join(" ", obj.ToString(), error_num, "\n" + error));
        }

        static public void writeFullConsole(String message, Exception e)
        {
            DateTime date = new DateTime();
            System.Console.WriteLine(String.Join(" ", date.ToString("dd/MM/yyyy HH:mm:ss"), message, "\n", e.Message));
            System.Console.WriteLine(e.ToString());
        }
    }
}
