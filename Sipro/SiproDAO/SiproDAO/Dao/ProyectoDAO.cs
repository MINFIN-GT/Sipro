using System;
using System.Collections.Generic;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;

namespace SiproDAO.Dao
{
    public class ProyectoDAO
    {
        public static List<Proyecto> getProyectos(String usuario)
        {
            List<Proyecto> ret = new List<Proyecto>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Proyecto>("SELECT * FROM PROYECTO p WHERE p.id IN(SELECT u.proyectoid FROM PROYECTO_USUARIO u WHERE u.usuario=:usuario) AND p.estado=1", new { usuario = usuario }).AsList<Proyecto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "Proyecto.class", e);
            }
            return ret;
        }

        public static List<Proyecto> getProyectos(int prestamoId, String usuario)
        {
            List<Proyecto> ret = new List<Proyecto>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Proyecto>("SELECT * FROM PROYECTO p WHERE p.prestamoid=:prestamoid AND pid IN(SELECT u.proyectoid FROM PROYECTO_USUARIO u " +
                        "WHERE u.usuario=:usuario)", new { prestamoid = prestamoId, usuario = usuario }).AsList<Proyecto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "Proyecto.class", e);
            }
            return ret;
        }

        public static List<Proyecto> getProyectosByIdPrestamo(int idPrestamo)
        {
            List<Proyecto> ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Proyecto>("SELECT * FROM PROYECTO WHERE prestamoid=:id AND estado=1", new { id = idPrestamo }).AsList<Proyecto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoDAO.class", e);
            }

            return ret;
        }

        public static bool guardarProyecto(Proyecto proyecto, bool calcular_valores_agregados)
        {
            bool ret = false;
            try
            { 
                int result = 0;
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = "";
                    if (proyecto.id < 1)
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_proyecto.nextval FROM DUAL");
                        proyecto.id = sequenceId;
                        query = String.Join(" ", "INSERT INTO PROYECTO VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, " +
                            ":fechaActualizacion, :estado, :proyectoTipoid, :ueunidadEjecutora, :snip, :programa, :subprograma, :proyecto, :actividad, :obra, :latitud, " +
                            ":longitud, :objetivo, :directorProyecto, :enunciadoAlcance, :costo, :acumulacionCostoid, :objetivoEspecifico, :visionGeneral, :renglon, " +
                            ":ubicacionGeografica, :fechaInicio, :fechaFin, :duracion, :duracionDimension, :orden, :treePath, :nivel, :ejercicio, :entidad, :ejecucionFisicaReal, " +
                            ":proyectoClase, :projectCargado, :prestamoid, :observaciones, :coordinador, :fechaElegibilidad, :fechaInicioReal, :fechaFinReal, :congelado, :fechaCierre)");

                        result = db.Execute(query, proyecto);
                        if (result > 0)
                            proyecto.treepath = 10000000 + proyecto.id + "";

                        Usuario usu = UsuarioDAO.getUsuario(proyecto.usuarioCreo);
                        ProyectoUsuario pu = new ProyectoUsuario();
                        pu.proyectoid = proyecto.id;
                        pu.usuario = proyecto.usuarioCreo;
                        pu.usuarioCreo = proyecto.usuarioCreo;
                        pu.fechaActualizacion = proyecto.fechaActualizacion;
                        pu.usuarioActualizo = proyecto.usuarioActualizo;

                        result = db.Execute("INSERT INTO PROYECTO_USUARIO VALUES (:proyectoid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", pu);

                        if (!proyecto.usuarioCreo.Equals("admin"))
                        {
                            ProyectoUsuario pu_admin = new ProyectoUsuario();
                            pu_admin.proyectoid = proyecto.id;
                            pu_admin.usuario = "admin";
                            pu_admin.usuarioCreo = proyecto.usuarioCreo;

                            result = db.Execute("INSERT INTO PROYECTO_USUARIO VALUES (:proyectoid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", pu_admin);
                        }
                    }

                    query = String.Join(" ", "UPDATE proyecto SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, " +
                        "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado, proyecto_tipoid=:proyectoTipoid, ueunidad_ejecutora=:ueunidadEjecutora, " +
                        "snip=:snip, programa=:programa, subprograma=:subprograma, proyecto=:proyecto, actividad=:actividad, obra=:obra, latitud=:latitud, longitud=:longitud, " +
                        "objetivo=:objetivo, director_proyecto=:directorProyecto, enunciado_alcance=:enunciadoAlcance, costo=:costo, acumulacion_costoid=:acumulacionCostoid, " +
                        "objetivo_especifico=:objetivoEspecifico, vision_general=:visionGeneral, renglon=:renglon, ubicacion_geografica=:ubicacionGeografica, fecha_inicio=:fechaInicio, " +
                        "fecha_fin=:fechaFin, duracion=:duracion, duracion_dimension=:duracionDimension, orden=:orden, treePath=:treePath, nivel=:nivel, ejercicio=:ejercicio, entidad=:entidad, " +
                        "ejecucion_fisica_real=:ejecucionFisicaReal, proyecto_clase=:proyectoClase, project_cargado=:projectCargado, prestamoid=:prestamoid, observaciones=:observaciones, " +
                        "coordinador=:coordinador, fecha_elegibilidad=:fechaElegibilidad, fecha_inicio_real=:fechaInicioReal, fecha_fin_real=:fechaFinReal, congelado=:congelado, fecha_cierre=:fechaCierre " +
                        "WHERE id=:id");

                    result = db.Execute(query, proyecto);                                     

                    if (result > 0 && calcular_valores_agregados)
                        calcularCostoyFechas(proyecto.id);

                    ret = result > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoDAO.class", e);
                return false;
            }

            return ret;
        }

        public static Proyecto getProyectoPorId(int id, String usuario)
        {
            Proyecto ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Proyecto>("SELECT * FROM PROYECTO WHERE id=:id AND id " +
                        "IN(SELECT u.proyectoid FROM PROYECTO_USUARIO u WHERE u.usuario=:usuario)", new { id = id, usuario = usuario });
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProyectoDAO.class", e);
            }
            return ret;
        }

        public static Proyecto getProyecto(int id)
        {
            Proyecto ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Proyecto>("SELECT * FROM PROYECTO WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProyectoDAO.class", e);
            }
            return ret;
        }

        public static bool eliminarProyecto(Proyecto proyecto){
            bool ret = false;
            
            try{
                proyecto.estado = 0;
                proyecto.fechaActualizacion = DateTime.Now;
                ret = guardarProyecto(proyecto, false);
            }
            catch(Exception e){
                CLogger.write("6", "ProyectoDAO.class", e);
            }
            return ret;
        }

        public static long getTotalProyectos(String filtro_busqueda, String usuario, int? prestamoId)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM Proyecto p WHERE p.estado=1 ";
                    String query_a = "";
                    if (prestamoId > 0)
                        query_a = String.Join("", " p.prestamoid=", prestamoId + "");
                    if (prestamoId == null)
                        query_a = String.Join("", " p.prestamoid=null");
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " AND " : ""), " p.nombre LIKE '%" + filtro_busqueda + "%' ");

                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }                                                        
                    }
                    
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    if (usuario != null)
                        query = String.Join("", query, " AND p.id in (SELECT u.proyectoid FROM PROYECTO_USUARIO u WHERE u.usuario=:usuario )");

                    ret = db.ExecuteScalar<long>(query, new { usuario = usuario });
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ProyectoDAO.class", e);
            }
            return ret;
        }

        public static List<Proyecto> getProyectosPagina(int pagina, int numeroproyecto, String filtro_busqueda, String columna_ordenada, String orden_direccion, 
            String usuario, int? prestamoId)
        {
            List<Proyecto> ret = new List<Proyecto>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM PROYECTO p WHERE p.estado = 1";
                    String query_a = "";
                    if (prestamoId != null && prestamoId > 0)
                        query_a = String.Join("", " p.prestamoid=", prestamoId + "");
                    if (prestamoId == null)
                        query_a = String.Join("", " p.prestamoid=null");

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " AND " : ""), " p.nombre LIKE '%" + filtro_busqueda + "%' ");

                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }                      
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    if (usuario != null)
                        query = String.Join("", query, " AND p.id in (SELECT u.proyectoid FROM PROYECTO_USUARIO u where u.usuario=:usuario )");
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) :
                        String.Join(" ", query, "ORDER BY fecha_creacion ASC");
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroproyecto + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroproyecto + ") + 1)");

                    ret = db.Query<Proyecto>(query, new { usuario = usuario }).AsList<Proyecto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("8", "Proyecto.class", e);
            }
            return ret;
        }

        public static List<Proyecto> getProyectosPaginaDisponibles(int pagina, int numeroproyecto, String filtro_nombre, String filtro_usuario_creo,
            String filtro_fecha_creacion, String columna_ordenada, String orden_direccion, String idsProyectos)
        {
            List<Proyecto> ret = new List<Proyecto>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM PROYECTO p WHERE p.estado = 1";
                    if (idsProyectos != null && idsProyectos.Trim().Length > 0)
                        query = String.Join("", query, " AND p.id NOT IN (" + idsProyectos + ")");
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " p.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE '%", filtro_usuario_creo, "%' ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroproyecto + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroproyecto + ") + 1)");

                    ret = db.Query<Proyecto>(query, new { filtro_fecha_creacion = filtro_fecha_creacion }).AsList<Proyecto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "Proyecto.class", e);
            }
            return ret;
        }

        public static long getTotalProyectosDisponibles(String filtro_nombre, String filtro_usuario_creo, String filtro_fecha_creacion, String idsProyectos)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(*) FROM PROYECTO p WHERE p.estado=1 ";
                    String query_a = "";
                    if (filtro_nombre != null && filtro_nombre.Trim().Length > 0)
                        query_a = String.Join("", query_a, " p.nombre LIKE '%", filtro_nombre, "%' ");
                    if (filtro_usuario_creo != null && filtro_usuario_creo.Trim().Length > 0)
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " p.usuario_creo LIKE '%", filtro_usuario_creo, "%' ");
                    if (filtro_fecha_creacion != null && filtro_fecha_creacion.Trim().Length > 0)
                        query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(p.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE(:filtro_fecha_creacion,'DD/MM/YY') ");
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    if (idsProyectos != null && idsProyectos.Trim().Length > 0)
                        query = String.Join("", query, " AND p.id not in (" + idsProyectos + " )");

                    ret = db.ExecuteScalar<long>(query, new { filtro_fecha_creacion = filtro_fecha_creacion });
                }
            }
            catch (Exception e)
            {
                CLogger.write("10", "ProyectoDAO.class", e);
            }
            return ret;
        }       

        public static List<Proyecto> getProyectosPorUnidadEjecutora(String usuario, int unidadEjecutoraId)
        {
            List<Proyecto> ret = new List<Proyecto>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM PROYECTO p",
                        "INNER JOIN UNIDAD_EJECUTORA ue ON ue.unidad_ejecutora=p.ueunidad_ejecutora",
                        "WHERE p.id IN (SELECT u.proyectoid FROM PROYECTO_USUARIO u WHERE u.usuario=:usuario)",
                        "AND p.estado=1 AND ue.unidad_ejecutora=:unidadEjecutora");
                    ret = db.Query<Proyecto>(query, new { usuario = usuario, unidadEjecutora = unidadEjecutoraId }).AsList<Proyecto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("12", "Proyecto.class", e);
            }
            return ret;
        }

        public static Proyecto getProyectoPorUnidadEjecutora(int unidadEjecutoraId, int prestamoId, int entidad)
        {
            Proyecto ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT p.* FROM PROYECTO p ",
                        "INNER JOIN UNIDAD_EJECUTORA pp on p.ueunidad_ejecutora=pp.unidad_ejecutora",
                        "WHERE p.estado=1",
                        "AND p.prestamoid=:prestamoId",
                        "AND pp.unidad_ejecutora=:unidadEjecutora");

                    query = String.Join(" ", query, entidad > 0 ? "AND pp.entidadentidad=:entidad " : " ");
                    ret = db.QueryFirstOrDefault<Proyecto>(query, new { prestamoId = prestamoId, unidadEjecutora = unidadEjecutoraId, entidad = entidad });
                }
            }
            catch (Exception e)
            {
                CLogger.write("12", "Proyecto.class", e);
            }
            return ret;
        }

        public static Proyecto getProyectoOrden(int id)
        {
            Proyecto ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Proyecto>("SELECT * FROM PROYECTO WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("13", "ProyectoDAO.class", e);
            }
            return ret;
        }

        public static bool guardarProyectoOrden(Proyecto proyecto, DbConnection db)
        {
            bool ret = false;
            try
            {
                ret = guardarProyecto(proyecto, false);
            }
            catch (Exception e)
            {
                CLogger.write("14", "ProyectoDAO.class", e);
            }
            return ret;
        }

        public static List<Proyecto> getTodosProyectos()
        {
            List<Proyecto> ret = new List<Proyecto>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Proyecto>("SELECT * FROM PROYECTO WHERE estado=:1").AsList<Proyecto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("15", "Proyecto.class", e);
            }
            return ret;
        }

        public static decimal calcularCosto(Proyecto proyecto)
        {
            decimal costo = decimal.Zero;
            try
            {
                List<Componente> componentes = ComponenteDAO.getComponentesPorProyecto(proyecto.id);
                List<Actividad> actividades = ActividadDAO.getActividadesPorObjeto(proyecto.id, 1);

                if ((componentes != null && componentes.Count > 0) || (actividades != null && actividades.Count > 0))
                {

                    foreach (Componente componente in componentes)
                    {
                        costo += componente.costo ?? default(decimal);
                    }

                    if (actividades != null && actividades.Count > 0)
                    {
                        foreach (Actividad actividad in actividades)
                        {
                            costo += actividad.costo ?? default(decimal);
                        }
                    }
                }
                else
                {
                    costo = proyecto.costo ?? default(decimal);
                }
            }
            catch (Exception e)
            {
                CLogger.write("16", "Proyecto.class", e);
            }

            return costo;
        }

        public static DateTime calcularFechaMinima(Proyecto proyecto)
        {
            DateTime fechaActual = default(DateTime);
            try
            {
                List<Actividad> actividades = ActividadDAO.getActividadesPorObjeto(proyecto.id, 1);
                if (actividades != null && actividades.Count > 0)
                {
                    DateTime fechaMinima = new DateTime();
                    foreach (Actividad actividad in actividades)
                    {
                        if (fechaActual == null)
                            fechaActual = actividad.fechaInicio;
                        else
                        {
                            fechaMinima = actividad.fechaInicio;

                            if (fechaActual > fechaMinima)
                                fechaActual = fechaMinima;
                        }
                    }
                }

                List<Componente> componentes = ComponenteDAO.getComponentesPorProyecto(proyecto.id);
                if (componentes != null && componentes.Count > 0)
                {
                    DateTime fechaMinima = new DateTime();
                    foreach (Componente componente in componentes)
                    {
                        if (fechaActual == null)
                            fechaActual = componente.fechaInicio ?? default(DateTime);
                        else
                        {
                            fechaMinima = componente.fechaInicio ?? default(DateTime);

                            if (fechaActual > fechaMinima)
                                fechaActual = fechaMinima;
                        }
                    }
                }
                else if (fechaActual == null)
                    fechaActual = proyecto.fechaInicio ?? default(DateTime);
            }
            catch (Exception e)
            {
                CLogger.write("17", "Proyecto.class", e);
            }

            return fechaActual;
        }

        public static DateTime calcularFechaMaxima(Proyecto proyecto)
        {
            DateTime fechaActual = default(DateTime);
            try
            {
                List<Actividad> actividades = ActividadDAO.getActividadesPorObjeto(proyecto.id, 1);
                if (actividades != null && actividades.Count > 0)
                {
                    DateTime fechaMaxima = new DateTime();
                    foreach (Actividad actividad in actividades)
                    {
                        if (fechaActual == null)
                            fechaActual = actividad.fechaFin;
                        else
                        {
                            fechaMaxima = actividad.fechaFin;

                            if (fechaActual < fechaMaxima)
                                fechaActual = fechaMaxima;
                        }
                    }
                }

                List<Componente> componentes = ComponenteDAO.getComponentesPorProyecto(proyecto.id);
                if (componentes != null && componentes.Count > 0)
                {
                    DateTime fechaMaxima = new DateTime();
                    foreach (Componente componente in componentes)
                    {
                        if (fechaActual == null)
                            fechaActual = componente.fechaFin ?? default(DateTime);
                        else
                        {
                            fechaMaxima = componente.fechaFin ?? default(DateTime);

                            if (fechaActual < fechaMaxima)
                                fechaActual = fechaMaxima;
                        }
                    }
                }
                else if (fechaActual == null)
                    fechaActual = proyecto.fechaFin ?? default(DateTime);

            }
            catch (Exception e)
            {
                CLogger.write("18", "Proyecto.class", e);
            }

            return fechaActual;
        }

        public static bool calcularCostoyFechas(int proyectoId){
            bool ret = false;
            try
            {
                List<List<Nodo>> listas = EstructuraProyectoDAO.getEstructuraProyectoArbolCalculos(proyectoId, null);
                for (int i = listas.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < listas[i].Count; j++)
                    {
                        Nodo nodo = listas[i][j];
                        decimal costo = decimal.Zero;
                        DateTime fecha_maxima = new DateTime(0);
                        DateTime fecha_minima = new DateTime(new DateTime(2999, 12, 31, 0, 0, 0).Ticks);
                        DateTime fecha_maxima_real = default(DateTime);
                        DateTime fecha_minima_real = default(DateTime);
                        foreach (Nodo nodo_hijo in nodo.children)
                        {
                            costo += nodo_hijo.costo;
                            fecha_minima = (nodo_hijo.fecha_inicio.TimeOfDay < fecha_minima.TimeOfDay) ? nodo_hijo.fecha_inicio : fecha_minima;
                            fecha_maxima = (nodo_hijo.fecha_fin.TimeOfDay > fecha_maxima.TimeOfDay) ? nodo_hijo.fecha_fin : fecha_maxima;
                            fecha_minima_real = nodo_hijo.fecha_inicio_real != null ? fecha_minima_real != null ? ((nodo_hijo.fecha_inicio_real.TimeOfDay < fecha_minima_real.TimeOfDay) ? nodo_hijo.fecha_inicio_real : fecha_minima_real) : nodo_hijo.fecha_inicio_real : fecha_minima_real != null ? fecha_minima_real : fecha_minima_real;
                            fecha_maxima_real = nodo_hijo.fecha_fin_real != null ? fecha_maxima_real != null ? ((nodo_hijo.fecha_fin_real.TimeOfDay > fecha_maxima_real.TimeOfDay) ? nodo_hijo.fecha_fin_real : fecha_maxima_real) : nodo_hijo.fecha_fin_real : fecha_maxima_real != default(DateTime) ? fecha_maxima_real : default(DateTime);
                        }
                        nodo.objeto = ObjetoDAO.getObjetoPorIdyTipo(nodo.id, nodo.objeto_tipo);
                        if (nodo.children != null && nodo.children.Count > 0)
                        {
                            nodo.fecha_inicio = fecha_minima;
                            nodo.fecha_fin = fecha_maxima;
                            nodo.fecha_inicio_real = fecha_minima_real;
                            nodo.fecha_fin_real = fecha_maxima_real;
                            nodo.costo = costo;
                        }
                        else
                        {
                            decimal costo_temp = ObjetoDAO.calcularCostoPlan(nodo.objeto, nodo.objeto_tipo);
                            nodo.costo = (decimal)costo_temp;
                        }
                        nodo.duracion = Utils.getWorkingDays(nodo.fecha_inicio, nodo.fecha_fin);
                        setDatosCalculados(nodo.objeto, nodo.fecha_inicio, nodo.fecha_fin, nodo.costo, nodo.duracion, nodo.fecha_inicio_real, nodo.fecha_fin_real);
                    }
                    ret = true;
                }
                ret = ret && guardarProyectoBatch(listas);
            }
            catch (Exception e)
            {
                CLogger.write("19", "ProyectoDAO.class", e);
            }            	
            return ret;
        }

        private static void setDatosCalculados(Object objeto, DateTime fecha_inicio, DateTime fecha_fin, decimal costo, int duracion, DateTime fecha_inicio_real, DateTime fecha_fin_real)
        {
            try
            {
                if (objeto != null)
                {
                    Type objetoType = objeto.GetType();
                    var setFechaInicio = objetoType.GetProperty("fechaInicio");
                    var setFechaFin = objetoType.GetProperty("fechaFin");
                    var setCosto = objetoType.GetProperty("costo");
                    var setDuracion = objetoType.GetProperty("duracion");
                    var setFechaInicioReal = objetoType.GetProperty("fechaInicioReal");
                    var setFechaFinReal = objetoType.GetProperty("fechaFinReal");

                    if (fecha_inicio != null)
                        setFechaInicio.SetValue(objeto,fecha_inicio);
                    if (fecha_fin != null)
                        setFechaFin.SetValue(objeto, fecha_fin);
                    if (costo != default(decimal))
                        setCosto.SetValue(objeto, costo);
                    setDuracion.SetValue(objeto, duracion);
                    if (fecha_inicio_real != null)
                        setFechaInicioReal.SetValue(objeto, fecha_inicio_real);
                    if (fecha_fin_real != null)
                        setFechaFinReal.SetValue(objeto, fecha_fin_real);
                }
            }
            catch (Exception e)
            {
                CLogger.write("19", "ProyectoDAO.class", e);
            }
        }

        private static bool guardarProyectoBatch(List<List<Nodo>> listas)
        {
            bool ret = true;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    for (int i = 0; i < listas.Count - 1; i++)
                    {
                        for (int j = 0; j < listas[i].Count; j++)
                        {
                            switch (listas[i][j].objeto_tipo)
                            {
                                case 0:
                                    guardarProyecto((Proyecto)listas[i][j].objeto, false);
                                    break;
                                case 1:
                                    ComponenteDAO.guardarComponente((Componente)listas[i][j].objeto, false);
                                    break;
                                case 2:
                                    SubComponenteDAO.guardarSubComponente((Subcomponente)listas[i][j].objeto, false);
                                    break;
                                case 3:
                                    ProductoDAO.guardarProducto((Producto)listas[i][j].objeto, false);
                                    break;
                                case 4:
                                    SubproductoDAO.guardarSubproducto((Subproducto)listas[i][j].objeto, false);
                                    break;
                                case 5:
                                    ActividadDAO.guardarActividad((Actividad)listas[i][j].objeto, false);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ret = false;
                CLogger.write("20", "ProyectoDAO.class", e);
            }
            return ret;
        }

        public static PepDetalle getPepDetalle(int id)
        {
            PepDetalle ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<PepDetalle>("SELECT * FROM PEP_DETALLE p WHERE p.id=:id and p.estado=1", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("21", "ProyectoDAO.class", e);
            }
            return ret;
        }

        public static bool guardarPepDetalle(PepDetalle pepDetalle)
        {
            bool ret = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM PEP_DETALLE WHERE proyectoid=:proyectoid", new { proyectoid = pepDetalle.proyectoid });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE pep_detalle SET observaciones=:observaciones, alertivos=:alertivos, elaborado=:elaborado, aprobado=:aprobado, " +
                            "autoridad=:autoridad, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                            "estado=:estado WHERE proyectoid=:proyectoid", pepDetalle);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute("INSERT INTO pep_detalle VALUES (:proyectoid, :observaciones, :alertivos, :elaborado, :aprobado, :autoridad, :usuarioCreo, " +
                            ":usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado)", pepDetalle);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("22", "ProyectoDAO.class", e);
            }
            return ret;
        }
        
        public static Proyecto getProyectoHistory(int id, String lineaBase)
        {
            Proyecto ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    string query = String.Join(" ", "SELECT * FROM PROYECTO WHERE estado=1 AND id=:id",
                        lineaBase != null ? "AND p.linea_base LIKE '%" + lineaBase + "%'" : "AND p.actual = 1 ");
                    ret = db.QueryFirstOrDefault<Proyecto>(query, new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("21", "ProyectoDAO.class", e);
            }
            return ret;
        }

        public static List<Proyecto> getProyectosPorPrestamoHistory(int prestamoId, String lineaBase)
        {
            List<Proyecto> ret = new List<Proyecto>();
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = String.Join(" ", "SELECT * FROM proyecto p WHERE p.prestamoid=:prestamoId",
                        "AND p.estado = 1",
                        lineaBase != null ? "AND p.linea_base=:lineaBase" : "AND p.actual = 1");

                    ret = db.Query<Proyecto>(query, new { prestamoId = prestamoId, lineaBase = lineaBase }).AsList<Proyecto>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("22", "ProyectoDAO.class", e);
            }
            return ret;
        }

        public static Proyecto getProyectobyTreePath(String treePath)
        {
            Proyecto ret = null;
            try
            {
                String treePathProyecto = treePath.Substring(0, 8);
                int proyectoId = Convert.ToInt32(treePathProyecto) - 10000000;
                ret = getProyecto(proyectoId);
            }
            catch (Exception e)
            {
                CLogger.write("23", "ProyectoDAO.class", e);
            }

            return ret;
        }

        public static String getVersiones(int proyectoId)
        {
            String resultado = "";
            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = "SELECT DISTINCT(version) FROM proyecto "
                        + " WHERE id=" + proyectoId;

                    List<dynamic> versiones = db.Query<dynamic>(query).AsList<dynamic>();

                    if (versiones != null)
                    {
                        for (int i = 0; i < versiones.Count; i++)
                        {
                            if (resultado.Length > 0)
                            {
                                resultado += ",";
                            }
                            resultado += (int)versiones[i];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("24", "ProyectoDAO.class", e);
            }

            return resultado;
        }

        public static String getHistoria(int proyectoId, int version){
            String resultado = "";
            String query = "SELECT c.version, c.nombre, c.descripcion, ct.nombre tipo, ue.nombre unidad_ejecutora, c.costo, ac.nombre tipo_costo, "
                    + " c.programa, c.subprograma, c.proyecto, c.actividad, c.obra, c.renglon, c.ubicacion_geografica, c.latitud, c.longitud, "
                    + " c.fecha_inicio, c.fecha_fin, c.duracion, c.fecha_inicio_real, c.fecha_fin_real, "
                    + " c.fecha_creacion, c.usuario_creo, c.fecha_actualizacion, c.usuario_actualizo, "
                    + " CASE WHEN c.estado = 1 "
                    + " THEN 'Activo' "
                    + " ELSE 'Inactivo' "
                    + " END AS estado, ejecucion_fisica_real "
                    + " FROM proyecto c "
                    + " INNER JOIN sipro.unidad_ejecutora ue ON c.unidad_ejecutoraunidad_ejecutora = ue.unidad_ejecutora and c.entidad = ue.entidadentidad and c.ejercicio = ue.ejercicio  JOIN sipro_history.proyecto_tipo ct ON c.proyecto_tipoid = ct.id "
                    + " LEFT JOIN acumulacion_costo ac ON c.acumulacion_costoid = ac.id "
                    + " WHERE c.id = " + proyectoId
                    + " AND c.version = " +version;

            String [] campos = {"Version", "Nombre", "Descripción", "Tipo", "Unidad Ejecutora", "Monto Planificado", "Tipo Acumulación de Monto Planificado", 
                    "Programa", "Subprograma", "Proyecto", "Actividad", "Obra", "Renglon", "Ubicación Geográfica", "Latitud", "Longitud", 
                    "Fecha Inicio", "Fecha Fin", "Duración", "Fecha Inicio Real", "Fecha Fin Real", 
                    "Fecha Creación", "Usuario que creo", "Fecha Actualización", "Usuario que actualizó", 
                    "Estado", "Ejecución Fí­sica %"};

            resultado = CHistoria.getHistoria(query, campos);
            return resultado;
        }             
    }
}