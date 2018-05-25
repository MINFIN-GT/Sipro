using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using System.Data.Common;
using Dapper;
using Utilities;

namespace SiproDAO.Dao
{
    public class TipoAdquisicionDAO
    {

        public static long getTotalTipoAdquisicionDisponibles(String idTiposAdquisiciones)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.ExecuteScalar<int>("SELECT COUNT(*) FROM TIPO_ADQUISICION WHERE estado=1");
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "TipoAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static List<TipoAdquisicion> getTipoAdquisicionPaginaTotalDisponibles(int pagina, int numeroTipoAdquisicion, String idTipoAdquisicion)
        {
            List<TipoAdquisicion> ret = new List<TipoAdquisicion>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM TIPO_ADQUISICION WHERE estado=1",
                        idTipoAdquisicion != null && idTipoAdquisicion.Length > 0 ? " and ta.id not in (" + idTipoAdquisicion + ")" : "");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroTipoAdquisicion + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroTipoAdquisicion + ") + 1)");

                    ret = db.Query<TipoAdquisicion>(query).AsList<TipoAdquisicion>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "TipoAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static long getTotalTipoAdquisicion(String filtro_cooperante, String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM TIPO_ADQUISICION ta WHERE ta.estado=1 ";
                    String query_a = "";
                    if (filtro_cooperante != null && filtro_cooperante.Trim().Length > 0)
                        query_a = String.Join("", query_a, " ta.cooperante.nombre LIKE '%", filtro_cooperante, "%' ");
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " ta.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " ta.usuarioCreo LIKE '%", filtro_usuario_creo, "%' ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(ta.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));


                    ret = db.ExecuteScalar<long>(query, new { filtro_fecha_creacion = filtro_fecha_creacion });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "TipoAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static List<TipoAdquisicion> getTipoAdquisicionPorObjeto(int objetoId, int objetoTipo)
        {
            List<TipoAdquisicion> ret = new List<TipoAdquisicion>();
            int cooperanteCodigo = 0;
            switch (objetoTipo)
            {
                case 1:
                    Componente componente = ComponenteDAO.getComponente(objetoId);
                    cooperanteCodigo = componente.proyectos.prestamos.cooperantes.codigo;
                    break;
                case 2:
                    Subcomponente subcomponente = SubComponenteDAO.getSubComponente(objetoId);
                    cooperanteCodigo = subcomponente.componentes.proyectos.prestamos.cooperantes.codigo;
                    break;
                case 3:
                    Producto producto = ProductoDAO.getProductoPorId(objetoId);
                    if (producto.componentes != null)
                    {
                        cooperanteCodigo = producto.componentes.proyectos.prestamos.cooperantes.codigo;
                    }
                    else if (producto.subcomponentes != null)
                    {
                        cooperanteCodigo = producto.subcomponentes.componentes.proyectos.prestamos.cooperantes.codigo;
                    }
                    break;
                case 4:
                    Subproducto subproducto = SubproductoDAO.getSubproductoPorId(objetoId);
                    if (subproducto.productos.componentes != null)
                    {
                        cooperanteCodigo = subproducto.productos.componentes.proyectos.prestamos.cooperantes.codigo;
                    }
                    else if (subproducto.productos.subcomponentes != null)
                    {
                        cooperanteCodigo = subproducto.productos.subcomponentes.componentes.proyectos.prestamos.cooperantes.codigo;
                    }
                    break;
                case 5:
                    Actividad actividad = ActividadDAO.getActividadPorId(objetoId);
                    if (actividad.treepath != null)
                    {
                        int proyectoId = Convert.ToInt32(actividad.treepath.Substring(0, 8)) - 10000000;
                        if (proyectoId != 0)
                            cooperanteCodigo = ProyectoDAO.getProyecto(proyectoId).prestamos.cooperantes.codigo;
                    }
                    break;
            }

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String str_query = "SELECT * FROM TIPO_ADQUISICION ta WHERE ta.cooperantecodigo=:codigo AND ta.estado=1";
                    ret = db.Query<TipoAdquisicion>(str_query, new { codigo = cooperanteCodigo }).AsList<TipoAdquisicion>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "TipoAdquisicionDAO.class", e);
            }

            return ret;
        }

        public static List<TipoAdquisicion> getTipoAdquisicionPagina(int pagina, int numeroTipoAdquisicion, String filtro_cooperante, String filtro_nombre, String filtro_usuario_creo,
                String filtro_fecha_creacion, String columna_ordenada, String orden_direccion)
        {

            List<TipoAdquisicion> ret = new List<TipoAdquisicion>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT ta FROM TipoAdquisicion ta WHERE ta.estado = 1 ";
                    String query_a = "";
                    if (filtro_cooperante != null && filtro_cooperante.Trim().Length > 0)
                        query_a = String.Join("", query_a, " ta.cooperante.nombre LIKE '%", filtro_cooperante, "%' ");
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " ta.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " ta.usuarioCreo LIKE '%", filtro_usuario_creo, "%' ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(ta.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroTipoAdquisicion + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroTipoAdquisicion + ") + 1)");

                    ret = db.Query<TipoAdquisicion>(query, new { filtro_fecha_creacion = filtro_fecha_creacion }).AsList<TipoAdquisicion>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "TipoAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static TipoAdquisicion getTipoAdquisicionPorId(int tipoAdquisicionId)
        {
            TipoAdquisicion ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<TipoAdquisicion>("SELECT * FROM TIPO_ADQUISICION ta WHERE ta.id=:tipoAdquisicionId", new { tipoAdquisicionId = tipoAdquisicionId });
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "TipoAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static bool guardarTipoAdquisicion(TipoAdquisicion tipoAdquisicion)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM TIPO_ADQUISICION WHERE id=id", new { id = tipoAdquisicion.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE TIPO_ADQUISICION SET cooperantecodigo=:cooperantecodigo, cooperanteejercicio=:cooperanteejercicio, nombre=:nombre, " +
                            "usuario_creo =:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado, " +
                            "convenio_cdirecta=:convenio_cdirecta WHERE id=:id", tipoAdquisicion);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_tipo_adquisicion.nextval FROM DUAL");
                        tipoAdquisicion.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO TIPO_ADQUISICION(:id, :cooperantecodigo, :cooperanteejercicio, :nombre, :usuarioCreo, :usuarioActualizo, " +
                            ":fechaCreacion, :fechaActualizacion, :estado, :convenioCdirecta)", tipoAdquisicion);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "TipoAdquisicionDAO.class", e);
            }
            return ret;
        }

        public static bool borrarTipoAdquisicion(TipoAdquisicion tipoAdquisicion)
        {
            bool ret = false;
            try
            {
                tipoAdquisicion.estado = 0;
                guardarTipoAdquisicion(tipoAdquisicion);
                ret = true;
            }
            catch (Exception e)
            {
                CLogger.write("8", "TipoAdquisicionDAO.class", e);
            }
            return ret;
        }
    }
}
