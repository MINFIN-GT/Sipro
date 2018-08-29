using System;
using System.Collections.Generic;
using System.Data.Common;
using Dapper;
using SiproModelCore.Models;
using Utilities;

namespace SiproDAO.Dao
{
    public class SubproductoTipoDAO
    {
        public static SubproductoTipo getSubproductoTipo(int codigo)
        {
            SubproductoTipo ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<SubproductoTipo>("SELECT * FROM suproducto_tipo WHERE id=:id", new { id = codigo });
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubproductoTipoDAO.class", e);
            }
            return ret;
        }

        public static bool guardarSubproductoTipo(SubproductoTipo subproductoTipo)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM subproducto_tipo WHERE id=:id", new { id = subproductoTipo.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE subproducto_tipo SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, " +
                            "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado WHERE id=:id", subproductoTipo);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_subproducto_tipo.nextval FROM DUAL");
                        subproductoTipo.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO subproducto_tipo(:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, " +
                            ":estado)", subproductoTipo);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubproductoTipoDAO.class", e);
            }

            return ret;
        }

        public static bool eliminarSubproductoTipo(SubproductoTipo subproductoTipo)
        {
            bool ret = false;
            try
            {
                subproductoTipo.estado = 0;
                subproductoTipo.fechaActualizacion = DateTime.Now;
                ret = guardarSubproductoTipo(subproductoTipo);
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubproductoTipoDAO.class", e);
            }

            return ret;
        }

        public static List<SubproductoTipo> getPagina(int pagina, int registros, String filtro_busqueda, String columna_ordenada, String orden_direccion)
        {
            List<SubproductoTipo> ret = new List<SubproductoTipo>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM subproducto_tipo p where p.estado = 1 ";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " p.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + registros + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + registros + ") + 1)");

                    ret = db.Query<SubproductoTipo>(query).AsList<SubproductoTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubproductoTipoDAO.class", e);
            }
            return ret;
        }

        public static long getTotal(String filtro_busqueda)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM subproducto_tipo p where p.estado = 1";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " p.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubproductoTipoDAO.class", e);
            }
            return ret;
        }
    }
}
