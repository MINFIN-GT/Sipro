using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelAnalyticCore.Models;
using System.Data.SqlClient;

namespace SiproDAO.Dao
{
    public class DataSigadeDAO
    {
        public static List<DtmAvanceFisfinanDti> getInf()
        {
            List<DtmAvanceFisfinanDti> ret = new List<DtmAvanceFisfinanDti>();
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    ret = db.Query<DtmAvanceFisfinanDti>("SELECT * FROM DTM_AVANCE_FISFINAN_DTI").AsList<DtmAvanceFisfinanDti>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static DtmAvanceFisfinanDti getInfPorId(String noPrestamo, String codigoPresupuestario)
        {
            DtmAvanceFisfinanDti ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join("", "SELECT * FROM dtm_avance_fisfinan_dti i ",
                    "WHERE i.codigo_presupuestario=:codPre ",
                    "AND i.no_prestamo=:noPre ");

                    ret = db.QueryFirstOrDefault<DtmAvanceFisfinanDti>(query, new { codPre = codigoPresupuestario, noPre = noPrestamo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static DtmAvanceFisfinanDti getavanceFisFinanDMS1(String codigoPresupuestario)
        {
            DtmAvanceFisfinanDti ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join(" ", "SELECT * FROM DTM_AVANCE_FISFINAN_DET_DTI d",
                    "WHERE d.codigo_presupuestario=:codigo_presupuestario");

                    ret = db.QueryFirstOrDefault<DtmAvanceFisfinanDti>(query, new { codigo_presupuestario = codigoPresupuestario });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static List<Object> getAVANCE_FISFINAN_DET_DTI(String codigoPresupuestario)
        {
            List<Object> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join(" ", "SELECT d.ejercicio_fiscal, d.mes_desembolso, SUM(d.desembolsos_mes_gtq) AS desembolsos_mes_gtq",
                    "FROM DTM_AVANCE_FISFINAN_DET_DTI d",
                    "WHERE d.codigo_presupuestario=:codigoPresupuestario",
                    "GROUP BY d.ejercicio_fiscal, d.mes_desembolso",
                    "ORDER BY d.ejercicio_fiscal, d.mes_desembolso asc");

                    using (var reader = (SqlDataReader)db.ExecuteReader(query, new { codigoPresupuestario = codigoPresupuestario }))
                    {
                        ret.Add(reader);
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static List<Object> getAVANCE_FISFINAN_DET_DTIRango(String codigoPresupuestario, int anio_inicio, int anio_fin, int tipoMoneda, int entidad)
        {
            List<Object> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join(" ", "SELECT d.ejercicio_fiscal, d.mes_desembolso, sum(", tipoMoneda == 1 ? "d.desembolsos_mes_gtq" : "d.desembolsos_mes_usd", ") AS desembolsos_mes_gtq",
                    "FROM DTM_AVANCE_FISFINAN_DET_DTI d",
                    "WHERE d.codigo_presupuestario=:codigoPresupuestario",
                    "AND d.ejercicio_fiscal BETWEEN :anio_inicio AND :anio_fin",
                    "AND d.entidad_sicoin=:entidad",
                    "GROUP BY d.ejercicio_fiscal, d.mes_desembolso",
                    "ORDER BY d.ejercicio_fiscal, d.mes_desembolso ASC");

                    using (var reader = (SqlDataReader)db.ExecuteReader(query, new { codigoPresupuestario = codigoPresupuestario, anio_inicio = anio_inicio, anio_fin = anio_fin, entidad = entidad }))
                    {
                        ret.Add(reader);
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static List<DtmAvanceFisfinanDti> getCodigos()
        {
            List<DtmAvanceFisfinanDti> ret = new List<DtmAvanceFisfinanDti>();
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    ret = db.Query<DtmAvanceFisfinanDti>("SELECT * FROM DTM_AVANCE_FISFINAN_DET_DTI").AsList<DtmAvanceFisfinanDti>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static decimal totalDesembolsadoAFechaReal(String codigo_presupuestario, long anio, int mes)
        {
            decimal ret = default(decimal);
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join(" ", "SELECT SUM(d.desembolsos_mes_gtq)",
                        "FROM DTM_AVANCE_FISFINAN_DET_DTI d",
                        "where d.id.codigo_presupuestario=:codigo_presupuestario",
                        "and (d.id.ejercicioFiscal < :anio ",
                        "or (d.id.ejercicioFiscal=:anio  and (cast(d.id.mesDesembolso as integer)) < :mes))");
                    ret = db.ExecuteScalar<decimal>(query, new { codigo_presupuestario = codigo_presupuestario, anio = anio, mes = mes });
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static List<DtmAvanceFisfinanCmp> getComponentes(String codigo_presupuestario)
        {
            List<DtmAvanceFisfinanCmp> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join(" ", "SELECT * FROM dtm_avance_fisfinan_cmp ",
                    "WHERE codigo_presupuestario=:codigo_presupuestario ",
                    "ORDER BY numero_componente ASC");

                    ret = db.Query<DtmAvanceFisfinanCmp>(query, new { codigo_presupuestario = codigo_presupuestario }).AsList<DtmAvanceFisfinanCmp>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static List<DtmAvanceFisfinanEnp> getUnidadesEjecutoras(String codigo_presupuestario, int ejercicio)
        {
            List<DtmAvanceFisfinanEnp> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join(" ", "SELECT * FROM DTM_AVANCE_FISFIAN_ENP",
                    "WHERE codigo_presupuestario=:codigo_presupuestario",
                    "AND ejercicio_fiscal=:ejercicio ");

                    ret = db.Query<DtmAvanceFisfinanEnp>(query).AsList<DtmAvanceFisfinanEnp>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static List<DtmAvanceFisfinanDetDti> getInfPorUnidadEjecutora(String codigoPresupuestario, int ejercicio, int entidad, int UE)
        {
            List<DtmAvanceFisfinanDetDti> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join("", "SELECT * DTM_AVANCE_FISFINAN_DET_DTI d ",
                         "WHERE d.codigo_presupuestario=:codigoPresupuestario ",
                         "AND d.ejercicio_fiscal=:ejercicio ",
                         "AND d.entidad_sicoin=:entidad ",
                         "and d.unidad_ejecutora_sicoin=:unidad_ejecutora");
                    ret = db.Query<DtmAvanceFisfinanDetDti>(query, new { codigoPresupuestario = codigoPresupuestario, ejercicio = ejercicio, entidad = entidad, unidad_ejecutora = UE }).AsList<DtmAvanceFisfinanDetDti>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static List<DtmAvanceFisfinanDetDti> getInfPorUnidadEjecutoraALaFecha(String codigoPresupuestario, int entidad)
        {
            List<DtmAvanceFisfinanDetDti> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join("", "SELECT * FROM DTM_AVANCE_FISFINAN_DET_DTI d ",
                         "WHERE d.codigo_presupuestario=:codigoPresupuestario ",
                         "AND d.entidad_sicoin=:entidad ",
                         "ORDER BY d.ejercicio_fiscal, d.mes_desembolso ASC");

                    ret = db.Query<DtmAvanceFisfinanDetDti>(query, new { codigoPresupuestario = codigoPresupuestario }).AsList<DtmAvanceFisfinanDetDti>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static int getDiferenciaMontos(String codigo_presupuestario)
        {
            int ret = default(int);
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join(" ", "SELECT SUM(cmp.monto_componente-cs.monto_componente) diferencia ",
                    "FROM DTM_AVANCE_FISFINAN_CMP cmp, SIPRO.componente_sigade cs ",
                    "WHERE cmp.codigo_presupuestario=cs.codigo_presupuestario ",
                    "AND cmp.numero_componente = cs.numero_componente ",
                    "AND cmp.codigo_presupuestario=:codigo_presupuestario ",
                    "AND cs.estado = 1");

                    ret = Convert.ToInt32(db.ExecuteScalar<decimal>(query, new { codigo_presupuestario = codigo_presupuestario }));
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "DataSigadeDAO.class", e);
            }
            return ret;
        }

        public static decimal totalDesembolsadoAFechaRealDolaresPorEntidad(String codigo_presupuestario, long anio, int mes, int entidadSicoin)
        {
            decimal ret = default(decimal);
            try
            {
                using (DbConnection db = new OracleContext().getConnectionAnalytic())
                {
                    string query = String.Join(" ", "SELECT SUM(d.desembolsos_mes_usd)",
                    "FROM DTM_AVANCE_FISFINAN_DET_DTI d",
                    "WHERE d.codigoPresupuestario=:codigo_presupuestario",
                    "AND (d.ejercicioFiscal < :anio ",
                    "OR (d.ejercicioFiscal=:anio and (cast(d.id.mesDesembolso as integer)) < :mes))",
                    "AND d.entidadSicoin=:entidadSicoin");

                    ret = db.ExecuteScalar<decimal>(query, new { codigo_presupuestario = codigo_presupuestario, anio = anio, mes = mes, entidadSicoin = entidadSicoin });
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "DataSigadeDAO.class", e);
            }
            return ret;
        }
    }
}
