using System;
using System.Configuration;

namespace Utilities
{
    public class Utils
    {
        public Utils()
        {

        }

        public static DateTime? getFechaHoraNull(string fechaHora)
        {
            if (fechaHora != null)
                return DateTime.ParseExact(fechaHora, "dd/MM/yyyy", null);
            return null;
        }

        public static DateTime getFechaHora(string fechaHora)
        {
            DateTime getDateTime = DateTime.ParseExact(fechaHora, "dd/MM/yyyy", null);
            return getDateTime;
        }

        public static int getWorkingDays(DateTime fecha_inicio, DateTime fecha_fin)
        {
            TimeSpan interval = fecha_fin - fecha_inicio;

            int totalWeek = interval.Days / 7;
            int totalWorkingDays = 5 * totalWeek;
            int remainingDays = interval.Days % 7;

            for (int i = 0; i <= remainingDays; i++)
            {
                DayOfWeek test = (DayOfWeek)(((int)fecha_inicio.DayOfWeek + i) % 7);
                if (test >= DayOfWeek.Monday && test <= DayOfWeek.Friday)
                    totalWorkingDays++;
            }

            return totalWorkingDays + 1;
        }

        public static DateTime setDateCeroHoras(DateTime fecha)
        {
            DateTime cfecha = new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0, 0);
            return cfecha;
        }

        public static string getDirectorioTemporal()
        {
            var directorioTemporal = ConfigurationManager.AppSettings["directorioTemporal"];
            return directorioTemporal;
        }

        public static string getJartImportProject()
        {
            var pathJar = ConfigurationManager.AppSettings["pathImportJar"];
            return pathJar;
        }
    }
}
