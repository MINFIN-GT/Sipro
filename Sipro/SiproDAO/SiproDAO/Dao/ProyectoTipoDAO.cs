using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class ProyectoTipoDAO
    {
        public static List<ProyectoTipo> getProyectoTipos()
        {
            List<ProyectoTipo> ret = new List<ProyectoTipo>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<ProyectoTipo>("SELECT * FROM PROYECTO_TIPO WHERE estado=1").AsList<ProyectoTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProyectoTipoDAO.class", e);
            }
            return ret;
        }

        public static ProyectoTipo getProyectoTipoPorId(int id)
        {
            ProyectoTipo ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ProyectoTipo>("SELECT * FROM PROYECTO_TIPO WHERE estado=1 AND id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProyectoTipoDAO.class", e);

            }
            return ret;
        }

        public static bool guardarProyectoTipo(ProyectoTipo proyectotipo)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    long existe = db.ExecuteScalar<long>("SELECT COUNT(*) FROM PROYECTO_TIPO WHERE id=:id", new { id = proyectotipo.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE PROYECTO_TIPO SET nombre=:nombre, descripcion=:descripcion, usario_creo=:usuarioCreo, " +
                            "usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                            "estado=:estado WHERE id=:id", proyectotipo);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_proyecto_tipo.nextval FROM DUAL");
                        proyectotipo.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO PROYECTO_TIPO VALUES (:id, :nombre, :descripcion, :usarioCreo, :usuarioActualizo, :fechaCreacion, " +
                            ":fechaActualizacion, :estado)", proyectotipo);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoTipoDAO.class", e);
            }
            return ret;
        }

        public static List<ProyectoTipo> getProyectosTipoPagina(int pagina, int numeroproyectotipos, String filtro_nombre, String filtro_usuario_creo,
                String filtro_fecha_creacion, String columna_ordenada, String orden_direccion)
        {
            List<ProyectoTipo> ret = new List<ProyectoTipo>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM PROYECTO_TIPO p WHERE p.estado=1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " p.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usarioCreo LIKE '%", filtro_usuario_creo, "%' ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " str(date_format(p.fechaCreacion,'%d/%m/%YYYY')) LIKE '%", filtro_fecha_creacion, "%' ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroproyectotipos + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroproyectotipos + ") + 1)");

                    ret = db.Query<ProyectoTipo>(query).AsList<ProyectoTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProyectoTipo.class", e);
            }
            return ret;
        }

        public static long getTotalProyectoTipos(String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT count(p.id) FROM ProyectoTipo p WHERE p.estado=1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " p.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usarioCreo LIKE '%", filtro_usuario_creo, "%' ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " str(date_format(p.fechaCreacion,'%d/%m/%YYYY')) LIKE '%", filtro_fecha_creacion, "%' ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    ret = db.ExecuteScalar<int>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProyectoTipoDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarProyectoTipo(ProyectoTipo proyectoTipo)
        {
            bool ret = false;
            try
            {
                proyectoTipo.estado = 0;
                ret = guardarProyectoTipo(proyectoTipo);
            }
            catch (Exception e)
            {
                CLogger.write("6", "ProyectoTipoDAO.class", e);
            }
            return ret;
        }
         
    }
}
