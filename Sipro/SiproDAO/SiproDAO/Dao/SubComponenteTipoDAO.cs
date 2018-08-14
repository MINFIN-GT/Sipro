using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Utilities;
using Dapper;

namespace SiproDAO.Dao
{
    public class SubComponenteTipoDAO
    {
        public static List<SubcomponenteTipo> getSubComponenteTipos()
        {
            List<SubcomponenteTipo> ret = new List<SubcomponenteTipo>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<SubcomponenteTipo>("SELECT * FROM subcomponente_tipo WHERE estado=1").AsList<SubcomponenteTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubComponenteTipoDAO.class", e);
            }
            return ret;
        }


        public static SubcomponenteTipo getSubComponenteTipoPorId(int id)
        {
            SubcomponenteTipo ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<SubcomponenteTipo>("SELECT * FROM subcomponente_tipo WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubComponenteTipoDAO.class", e);
            }
            return ret;
        }

        public static bool guardarSubComponenteTipo(SubcomponenteTipo subcomponenteTipo)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM subcomponente_tipo WHERE id=:id", new { id = subcomponenteTipo.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE subcomponente_tipo SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, " +
                            "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado WHERE id=:id", subcomponenteTipo);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_subcomponente_tipo.nextval FROM DUAL");
                        subcomponenteTipo.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO subcomponente_tipo VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, " +
                            ":estado)", subcomponenteTipo);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubComponenteTipoDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarSubComponenteTipo(SubcomponenteTipo subcomponenteTipo)
        {
            bool ret = false;

            try
            {
                subcomponenteTipo.estado = 0;
                subcomponenteTipo.fechaActualizacion = DateTime.Now;
                ret = guardarSubComponenteTipo(subcomponenteTipo);
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubComponenteTipoDAO.class", e);
            }
            return ret;
        }



        public static bool eliminarTotalSubComponenteTipo(SubcomponenteTipo subcomponenteTipo)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM subcomponente_tipo WHERE id=:id", new { id = subcomponenteTipo.id });

                    ret = eliminado > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubComponenteTipoDAO.class", e);
            }
            return ret;
        }


        public static List<SubcomponenteTipo> getSubComponenteTiposPagina(int pagina, int numerosubcomponentestipo, String filtro_busqueda, 
            String columna_ordenada, String orden_direccion)
        {
            List<SubcomponenteTipo> ret = new List<SubcomponenteTipo>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM subcomponente_tipo c WHERE c.estado = 1 ";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(" + fecha_creacion.ToString("dd/MM/yyyy") + ",'DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numerosubcomponentestipo + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numerosubcomponentestipo + ") + 1)");

                    ret = db.Query<SubcomponenteTipo>(query).AsList<SubcomponenteTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "SubComponenteTipoDAO.class", e);
            }
            return ret;
        }

        public static long getTotalSubComponenteTipo(String filtro_busqueda)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM subcomponente_tipo c WHERE c.estado=1 ";
                    String query_a = "";

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(" + fecha_creacion.ToString("dd/MM/yyyy") + ",'DD/MM/YY') ");
                        }

                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "SubComponenteTipoDAO.class", e);
            }
            return ret;
        }
    }
}
