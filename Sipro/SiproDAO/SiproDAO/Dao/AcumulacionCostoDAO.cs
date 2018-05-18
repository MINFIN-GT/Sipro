using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class AcumulacionCostoDAO
    {
        public static List<AcumulacionCosto> getAcumulacionesCosto()
        {
            List<AcumulacionCosto> ret = new List<AcumulacionCosto>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<AcumulacionCosto>("SELECT * FROM ACUMULACION_COSTO WHERE estado=1").AsList<AcumulacionCosto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "AcumulacionCosto.class", e);
            }
            return ret;
        }

        public static AcumulacionCosto getAcumulacionCostoById(int id)
        {
            AcumulacionCosto ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<AcumulacionCosto>("SELECT * FROM ACUMULACION_COSTO WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "AcumulacionCosto.class", e);
            }
            return ret;
        }
    }
}
