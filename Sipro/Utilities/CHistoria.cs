using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Dapper;

namespace Utilities
{
    public class CHistoria
    {
        public static String getHistoria(String query, String[] campos)
        {
            String resultado = "";
            if (query != null && query.Length > 0 && campos != null && campos.Length > 0)
            {
                List<dynamic> datos = getDatos(query);
                for (int d = 0; d < datos.Count; d++)
                {
                    Object[] dato = (Object[])datos[d];
                    if (resultado.Length > 0)
                    {
                        resultado += ", ";
                    }
                    resultado += "[";
                    String objeto = "";
                    for (int c = 0; c < campos.Length; c++)
                    {
                        if (objeto.Length > 0)
                        {
                            objeto += ", ";
                        }
                        objeto += "{\"nombre\": \"" + campos[c] + "\", \"valor\": \"" + (dato[c] != null ? ((string)dato[c]) : "") + "\"}";
                    }
                    resultado += objeto + "]";
                }
            }
            resultado = "[" + resultado + "]";
            return resultado;
        }

        public static List<dynamic> getDatos(String query)
        {
            List<dynamic> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    ret = db.Query<dynamic>(query).AsList<dynamic>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "CHistoria.class", e);
            }
            return ret;
        }
    }
}
