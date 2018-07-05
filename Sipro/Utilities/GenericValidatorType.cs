using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class GenericValidatorType
    {
        public static bool ValidateType(String val, Type type)
        {
            bool ret = false;

            if (type == typeof(Int32))
            {
                if (!val.Equals(""))
                {
                    Int32 value;
                    if (!Int32.TryParse(val, out value))
                        return false;
                }
                
                ret = true;
            }
            else if (type == typeof(Int64))
            {
                Int64 value;
                if (!Int64.TryParse(val, out value))
                    return false;
                ret = true;
            }
            else if (type == typeof(String))
                ret = true;
            else if (type == typeof(DateTime))
            {
                if (!val.Equals(""))
                {
                    DateTime fecha;
                    if (!DateTime.TryParse(val, out fecha))
                        return false;
                }

                ret = true;
            }
            else if (type == typeof(decimal))
            {
                decimal value;
                if (!val.Equals(""))
                {
                    if (!decimal.TryParse(val, out value))
                        return false;
                }

                ret = true;
            }
            return ret;
        }
    }
}
