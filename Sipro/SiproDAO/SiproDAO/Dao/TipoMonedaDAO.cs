using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using Utilities;
using System.Data.Common;
using Dapper;

namespace SiproDAO.Dao
{
    public class TipoMonedaDAO
    {
        public static long getTotalAuotirzacionTipo(String filtro_busqueda)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT COUNT(*) FROM TIPO_MONEDA",
                        "WHERE id LIKE '%" + filtro_busqueda + "%'",
                        "OR nombre like '%" + filtro_busqueda + "%'");
                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "TipoMonedaDAO.class", e);
            }
            return ret;
        }

        public static List<TipoMoneda> getAutorizacionTiposPagina(int pagina, int numeroTipoMoneda, String filtro_busqueda)
        {
            List<TipoMoneda> ret = new List<TipoMoneda>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT a.* FROM TIPO_MONEDA a ",
                        "WHERE a.id LIKE '%" + filtro_busqueda + "%'",
                        "OR a.nombre like '%" + filtro_busqueda + "%'");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroTipoMoneda + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroTipoMoneda + ") + 1)");
                    ret = db.Query<TipoMoneda>(query).AsList<TipoMoneda>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "TipoMonedaDAO.class", e);
            }
            return ret;
        }

        public static List<TipoMoneda> getTiposMoneda()
        {
            List<TipoMoneda> ret = new List<TipoMoneda>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<TipoMoneda>("SELECT * FROM TIPO_MONEDA").AsList<TipoMoneda>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "TipoMonedaDAO.class", e);
            }
            return ret;
        }

        public static TipoMoneda getTipoMonedaPorSimbolo(String simbolo)
        {
            TipoMoneda ret = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<TipoMoneda>("SELECT a.* FROM TIPO_MONEDA a WHERE a.simbolo=:simb", new { simb = simbolo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "TipoMonedaDAO.class", e);
            }
            return ret;
        }

        public static TipoMoneda getTipoMonedaPorId(int id)
        {
            TipoMoneda ret = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<TipoMoneda>("SELECT a.* FROM TIPO_MONEDA a WHERE a.id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "TipoMonedaDAO.class", e);
            }
            return ret;
        }
    }
}
