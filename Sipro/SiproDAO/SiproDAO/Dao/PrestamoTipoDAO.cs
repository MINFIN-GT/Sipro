using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
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
                        int result = db.Execute("UPDATE PRESTAMO_TIPO SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado WHERE id=:id", prestamotipo);
                        ret = result > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_prestamo_tipo.nextval FROM DUAL");
                        prestamotipo.id = sequenceId;
                        int result = db.Execute("INSERT INTO PRESTAMO_TIPO VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado)", prestamotipo);
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

        public static List<PrestamoTipo> getPrestamosTipoPagina(int pagina, int numeroproyectotipos, String filtro_busqueda, String columna_ordenada, String orden_direccion, String excluir)
        {
            List<PrestamoTipo> ret = new List<PrestamoTipo>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM Prestamo_Tipo p WHERE estado = 1 ";
                    String query_a = "";
                    if (filtro_busqueda != null)
                    {
                        query_a = String.Join("", query_a, " p.nombre LIKE '%", filtro_busqueda, "%' ");

                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(" + fecha_creacion.ToString("dd/MM/yyyy") +",'DD/MM/YY') ");
                        }
                    }
                    
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = String.Join(" ", query, (excluir != null && excluir.Length > 0 ? "and p.id not in (" + excluir + ")" : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroproyectotipos + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroproyectotipos + ") + 1)");

                    ret = db.Query<PrestamoTipo>(query).AsList<PrestamoTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "PrestamoTipo", e);
            }
            return ret;
        }

        public static long getTotalPrestamosTipos(String filtro_busqueda)
        {
            long ret = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM PRESTAMO_TIPO p WHERE p.estado=1 ";
                    String query_a = "";
                    if (filtro_busqueda != null)
                    {
                        query_a = String.Join("", query_a, " p.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " usuario_creo LIKE '%" + filtro_busqueda + "%'");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(" + fecha_creacion.ToString("dd/MM/yyyy") + ",'DD/MM/YY') ");
                        }
                    }
                        
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    ret = db.ExecuteScalar<long>(query);
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
                    bool eliminado = guardarPrestamoTipo(prestamoTipo);
                    ret = eliminado;
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
                        db.Execute("INSERT INTO PRESTAMO_TIPO_PRESTAMO (PRESTAMOID, TIPOPRESTAMOID, USUARIO_CREO, FECHA_CREACION, ESTADO) VALUES (:prestamoId, :tipoPrestamoId, :usuarioCreo, :fechaCreacion, :estado)",
                            new { prestamoId = prestamo.id, tipoPrestamoId = tipos[i], usuarioCreo = usuario, fechaCreacion = DateTime.Now, estado = 1 });
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
                    string query = String.Join(" ", "SELECT ptp.* FROM PRESTAMO_TIPO_PRESTAMO ptp",                        
                        "WHERE ptp.prestamoId=:prestamoId and ptp.estado=1");
                    ret = db.Query<PrestamoTipoPrestamo>(query, new { prestamoId = prestamoId }).AsList<PrestamoTipoPrestamo>();
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
