using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.Common;
using Sipro.Utilities;
using SiproModelCore.Models;

namespace Sipro.Dao
{
    public class PrestamoTipoDAO
    {

        public static List<PrestamoTipo> getPrestamoTipos()
        {
            List<PrestamoTipo> ret = new List<PrestamoTipo>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<PrestamoTipo>("SELECT * FROM Prestamo_Tipo WHERE estado=1").AsList<PrestamoTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "PrestamoTipoDAO.", e);
            }
            return ret;
        }

        public static PrestamoTipo getPrestamoTipoPorId(int id)
        {
            PrestamoTipo ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<PrestamoTipo>("SELECT * FROM Prestamo_Tipo WHERE id=:id AND estado=1", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "PrestamoTipoDAO", e);
            }
            return ret;
        }

        public static bool guardarPrestamoTipo(PrestamoTipo prestamotipo)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    long existe = db.ExecuteScalar<long>("SELECT COUNT(*) FROM PRESTAMO_TIPO WHERE id=:id", new { id = prestamotipo.id });
                    if (existe > 0)
                    {
                        int result = db.Execute("UPDATE PRESTAMO_TIPO SET nombre=:nombre, descripcion=:descripcion, usuarioCreo=:usuario_creo, usuarioActualizo=:usuario_actual, fechaCreacion=:fecha_creacion, fechaActualizacion=:fecha_actualizacion, estado=:estado WHERE id=:id", prestamotipo);
                        ret = result > 0 ? true : false;
                    }
                    else
                    {
                        int result = db.Execute("INSERT INTO PRESTAMO_TIPO VALUES (:id, :nombre, :descripcion, :usuario_creo, :usuario_actualizo, :fecha_creacion, :fecha_actualizacion, :estado)", prestamotipo);
                        ret = result > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "PrestamoTipoDAO", e);
            }
            return ret;
        }

        public static List<PrestamoTipo> getPrestamosTipoPagina(int pagina, int numeroproyectotipos, String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion,
            String columna_ordenada, String orden_direccion, String excluir)
        {
            List<PrestamoTipo> ret = new List<PrestamoTipo>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM Prestamo_Tipo p WHERE estado = 1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " p.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " usuario_creo LIKE :filtro_usuario_creo ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = String.Join(" ", query, (excluir != null && excluir.Length > 0 ? "and p.id not in (" + excluir + ")" : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroproyectotipos + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroproyectotipos + ") + 1)");

                    ret = db.Query<PrestamoTipo>(query, new { filtro_usuario_creo = filtro_usuario_creo, filtro_fecha_creacion = filtro_fecha_creacion }).AsList<PrestamoTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "PrestamoTipo", e);
            }
            return ret;
        }

        public static long getTotalPrestamosTipos(String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT count(p.id) FROM PrestamoTipo p WHERE p.estado=1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " p.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " usuario_creo LIKE :filtro_usuario_creo ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    ret = db.ExecuteScalar<long>(query, new { filtro_usuario_creo = filtro_usuario_creo, filtro_fecha_creacion = filtro_fecha_creacion });
                }

            }
            catch (Exception e)
            {
                CLogger.write("5", "PrestamoTipoDAO", e);
            }
            return ret;
        }

        public static bool eliminarPrestamoTipo(PrestamoTipo prestamoTipo)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    prestamoTipo.estado = 0;
                    guardarPrestamoTipo(prestamoTipo);
                    ret = true;
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "PrestamoTipoDAO", e);
            }
            return ret;
        }

        public static bool desasignarTiposAPrestamo(int prestamoId)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    db.Execute("DELETE FROM PRESTAMO_TIPO_PRESTAMO WHERE prestamoid=:prestamoid", new { prestamoid = prestamoId });
                    ret = true;
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "PrestamoTipoDAO", e);
            }
            return ret;
        }

        public static bool asignarTiposAPrestamo(List<int> tipos, Prestamo prestamo, String usuario)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    for (int i = 0; i < tipos.Count; i++)
                    {
                        db.Execute("INSERT INTO PRESTAMO_TIPO_PRESTAMO VALUES (:prestamoId, :tipoPrestamoId, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado)",
                            new { prestamoId = prestamo.id, tipoPrestamoId = tipos[i], usuarioCreo = usuario, fechaCreacion = DateTime.Now, estado = 1, usuarioActualizo = DBNull.Value });
                    }

                    ret = true;
                }
            }
            catch (Exception e)
            {
                ret = false;
                CLogger.write("8", "PrestamoTipoDAO", e);
            }
            return ret;
        }

        public static List<PrestamoTipoPrestamo> getPrestamoTiposPrestamo(int prestamoId)
        {
            List<PrestamoTipoPrestamo> ret = new List<PrestamoTipoPrestamo>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT ptp.*, p.id, ue.unidad_ejecutora, e.entidad, pt.id FROM PRESTAMO_TIPO_PRESTAMO ptp",
                        "INNER JOIN PRESTAMO p ON p.id=ptp.prestamoid",
                        "INNER JOIN UNIDAD_EJECUTORA ue ON ue.unidad_ejecutora=p.ueunidad_ejecutora",
                        "INNER JOIN ENTIDAD e ON e.entidad=ue.entidadentidad",
                        "INNER JOIN PRESTAMO_TIPO pt ON pt.id=ptp.tipoprestamoid",
                        "WHERE ptp.prestamoId=:prestamoId");
                    ret = db.Query<PrestamoTipoPrestamo,Prestamo, UnidadEjecutora, Entidad, PrestamoTipo,PrestamoTipoPrestamo>(query, 
                        (ptp, p, ue,e, pt) => 
                        {
                            ptp.prestamos = p;
                            p.unidadEjecutoras = ue;
                            ue.entidads = e;
                            ptp.prestamoTipos = pt;
                            return ptp;
                        },
                        new { prestamoId = prestamoId }, splitOn : "id,unidad_ejecutora, entidad, id").AsList<PrestamoTipoPrestamo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "PrestamoTipoDAO", e);
            }
            return ret;
        }
    }
}
