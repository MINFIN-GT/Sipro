using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using System.Data.Common;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class EtiquetaDAO
    {
        public static List<Etiqueta> getEtiquetas()
        {
            List<Etiqueta> ret = new List<Etiqueta>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Etiqueta>("SELECT * FROM ETIQUETA WHERE estado=1").AsList<Etiqueta>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "EtiquetaDAO.class", e);
            }
            return ret;
        }

        public static Etiqueta getEtiquetaPorId(int id)
        {
            Etiqueta ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Etiqueta>("SELECT * FROM ETIQUETA WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "EtiquetaDAO.class", e);
            }
            return ret;
        }
    }
}
