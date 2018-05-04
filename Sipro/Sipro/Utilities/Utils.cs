using System;

namespace Sipro.Utilities
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
    }
}
