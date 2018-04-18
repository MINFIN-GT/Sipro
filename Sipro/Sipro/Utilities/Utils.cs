using System;

namespace Sipro.Utilities
{
    public class Utils
    {
        public Utils()
        {

        }

        public static byte[] getFechaHora(DateTime fechaHora)
        {
            long getLong = fechaHora.ToBinary();
            return BitConverter.GetBytes(getLong);
        }

        public static DateTime getFechaHora(byte[] fechaHora)
        {
            long getLong = BitConverter.ToInt64(fechaHora, 0);
            return DateTime.FromBinary(getLong);
        }
    }
}
