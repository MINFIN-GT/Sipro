using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Utilities;
using Dapper;

namespace SiproDAO.Dao
{
    public class DatoTipoDAO
    {
        public static DatoTipo getDatoTipo(int codigo)
        {
            DatoTipo ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<DatoTipo>("SELECT * FROM dato_tipo WHERE id=:id", new { id = codigo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "DatoTipoDAO.class", e);
            }
            return ret;
        }

        public static bool guardar(int codigo, String nombre, String descripcion)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.Execute("SELECT COUNT(*) FROM dato_tipo WHERE id=:id", new { id = codigo });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE dato_tipo SET nombre=:nombre, descripcion=:descripcion WHERE id=:id",
                            new { nombre = nombre, descripcion = descripcion, id = codigo });

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_dato_tipo.nextval FROM DUAL");
                        int guardado = db.Execute("INSERT INTO dato_tipo VALUES (:id, :nombre, :descripcion)", new { id = sequenceId, nombre = nombre, descripcion = descripcion });

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "DatoTipoDAO.class", e);
            }

            return ret;
        }

        public static List<DatoTipo> getPagina(int pagina, int registros)
        {
            List<DatoTipo> ret = new List<DatoTipo>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM dato_tipo");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + registros + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + registros + ") + 1)");
                    ret = db.Query<DatoTipo>(query).AsList<DatoTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "DatoTipoDAO.class", e);
            }
            return ret;
        }

        public static long getTotal()
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<long>("SELECT COUNT(*) FROM dato_tipo");
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "DatoTipoDAO.class", e);
            }
            return ret;
        }

        public static List<DatoTipo> getDatoTipos()
        {
            List<DatoTipo> ret = new List<DatoTipo>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<DatoTipo>("SELECT * FROM dato_tipo").AsList<DatoTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "DatoTipoDAO.class", e);
            }
            return ret;
        }
    }
}
