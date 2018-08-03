/*using net.sf.mpxj;
using net.sf.mpxj.reader;
using net.sf.mpxj.mpp;
using System;
using System.Collections.Generic;
using SiproModelCore.Models;
using SiproDAO.Dao;
using System.Reflection;
using net.sf.mpxj.mspdi;
using System.IO;
using System.Security.AccessControl;
*/
namespace Utilities
{
    public class CProject
    {
        /*ProjectFile project;
        ProjectReader reader;
        bool multiproyecto;
        private long proyectoId;
        Dictionary<int, Stitem> items;

        private class Stitem
        {
            public int id;
            public String contenido;
            public int indentacion;
            public bool expandido;
            public DateTime fechaInicial;
            public DateTime fechaFinal;
            public int duracion;
            public String unidades;
            public bool esHito;
            public int[] idPredecesores;
            public int objetoId;
            public int objetoTipo;
        }

        static int PROYECTO_TIPO_ID_DEFECTO = 1;
        static int ENTIDAD_ID_DEFECTO = 0;
        static int UNIDAD_EJECUTORA_ID_DEFECTO = 0;
        static int COMPONENTE_TIPO_ID_DEFECTO = 1;
        static int SUBCOMPONENTE_TIPO_ID_DEFECTO = 1;
        static int PRODUCTO_TIPO_ID_DEFECTO = 1;
        static int SUBPRODUCTO_TIPO__ID_DEFECTO = 1;
        static int ACTIVIDAD_TIPO_ID_DEFECTO = 1;
        static int PROYECTO_ETIQUETA_DEFECTO = 1;

        public CProject(String nombre)
        {
            try
            {
                //reader = new MPPReader();
                reader = ProjectReaderUtility.getProjectReader(nombre);
                project = reader.read(nombre);
            }
            catch (Exception e)
            {
                CLogger.write("1", "CProject.class", e);
            }
        }

        public ProjectReader GetReader()
        {
            return reader;
        }

        public void SetReader(ProjectReader reader)
        {
            this.reader = reader;
        }

        public ProjectFile GetProject()
        {
            return project;
        }

        public void SetProject(ProjectFile project)
        {
            this.project = project;
        }

        public long GetProyectoId()
        {
            return proyectoId;
        }

        public void SetProyectoId(long proyectoId)
        {
            this.proyectoId = proyectoId;
        }

        public bool ImporatarArchivo(ProjectFile projectFile, String usuario, bool multiproyecto, int proyectoId, bool marcarCargado, int prestamoId)
        {
            items = new Dictionary<int, Stitem>();
            this.multiproyecto = multiproyecto;

            return GetTask(projectFile, usuario, proyectoId, marcarCargado, prestamoId);
        }

        public Proyecto CrearProyecto(Task task, String usuario, int prestamoId) {
            ProyectoTipo proyectoTipo = ProyectoTipoDAO.getProyectoTipoPorId(PROYECTO_TIPO_ID_DEFECTO);
            Etiqueta etiqueta = new Etiqueta();
            etiqueta.id = PROYECTO_ETIQUETA_DEFECTO;

            AcumulacionCosto acumulacionCosto = null;

            switch (task.FixedCostAccrual.Value) {
                case 1:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(1);
                    break;
                case 2:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(2);
                    break;
                case 3:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(3);
                    break;
            }

            Prestamo prestamo = PrestamoDAO.getPrestamoById(prestamoId);

            UnidadEjecutora unidadEjecutora = UnidadEjecutoraDAO.getUnidadEjecutora(DateTime.Now.Year, ENTIDAD_ID_DEFECTO, UNIDAD_EJECUTORA_ID_DEFECTO);
            Proyecto proyecto = new Proyecto();
            proyecto.acumulacionCostoid = acumulacionCosto.id;
            proyecto.acumulacionCostos = acumulacionCosto;
            proyecto.directorProyecto = null;
            proyecto.proyectoClase = etiqueta.id;
            proyecto.prestamoid = prestamo.id;
            proyecto.proyectoTipos = proyectoTipo;
            proyecto.proyectoTipoid = proyectoTipo.id;
            proyecto.unidadEjecutoras = unidadEjecutora;
            proyecto.ueunidadEjecutora = unidadEjecutora.unidadEjecutora;
            proyecto.nombre = task.Name;
            proyecto.descripcion = null;
            proyecto.usuarioCreo = usuario;
            proyecto.usuarioActualizo = null;
            proyecto.fechaCreacion = DateTime.Now;
            proyecto.fechaActualizacion = null;
            proyecto.estado = 1;
            proyecto.snip = null;
            proyecto.programa = null;
            proyecto.subprograma = null;
            proyecto.proyecto = null;
            proyecto.actividad = null;
            proyecto.obra = null;
            proyecto.latitud = null;
            proyecto.longitud = null;
            proyecto.objetivo = null;
            proyecto.enunciadoAlcance = null;
            proyecto.costo = Convert.ToDecimal(task.Cost);
            proyecto.objetivoEspecifico = null;
            proyecto.visionGeneral = null;
            proyecto.renglon = null;
            proyecto.ubicacionGeografica = null;
            proyecto.fechaInicio = Convert.ToDateTime(task.Start);
            proyecto.fechaFin = Convert.ToDateTime(task.Finish);
            proyecto.duracion = Convert.ToInt32(task.Duration);
            proyecto.duracionDimension = task.DurationText;
            proyecto.orden = null;
            proyecto.treepath = null;
            proyecto.nivel = 0;
            proyecto.ejecucionFisicaReal = 0;
            proyecto.projectCargado = 0;
            proyecto.observaciones = null;
            proyecto.coordinador = null;
            proyecto.fechaElegibilidad = null;
            proyecto.fechaCierre = null;
            proyecto.fechaInicioReal = Convert.ToDateTime(task.ActualStart);
            proyecto.fechaFinReal = Convert.ToDateTime(task.ActualFinish);
            proyecto.congelado = null;
            return ProyectoDAO.guardarProyecto(proyecto, false) ? proyecto : null;
        }

        public Componente CrearComponente(Task task, Proyecto proyecto, String usuario)
        {
            ComponenteTipo componenteTipo = ComponenteTipoDAO.getComponenteTipoPorId(COMPONENTE_TIPO_ID_DEFECTO);
            int year = DateTime.Now.Year;
            UnidadEjecutora unidadEjecutora = UnidadEjecutoraDAO.getUnidadEjecutora(year, ENTIDAD_ID_DEFECTO, UNIDAD_EJECUTORA_ID_DEFECTO);
            AcumulacionCosto acumulacionCosto = null;

            switch (task.FixedCostAccrual.Value)
            {
                case 1:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(1);
                    break;
                case 2:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(2);
                    break;
                case 3:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(3);
                    break;
            }

            Componente componente = new Componente();
            componente.acumulacionCostos = acumulacionCosto;
            componente.acumulacionCostoid = acumulacionCosto.id;
            componente.componenteSigades = null;
            componente.componenteSigadeid = default(int);
            componente.componenteTipos = componenteTipo;
            componente.componenteTipoid = componenteTipo.id;
            componente.proyectos = proyecto;
            componente.proyectoid = proyecto.id;
            componente.unidadEjecutoras = unidadEjecutora;
            componente.ueunidadEjecutora = unidadEjecutora.unidadEjecutora;
            componente.nombre = task.Name;
            componente.descripcion = null;
            componente.usuarioCreo = usuario;
            componente.usuarioActualizo = null;
            componente.fechaCreacion = DateTime.Now;
            componente.fechaActualizacion = null;
            componente.estado = 1;
            componente.snip = null;
            componente.programa = null;
            componente.subprograma = null;
            componente.proyecto = null;
            componente.actividad = null;
            componente.obra = null;
            componente.latitud = null;
            componente.longitud = null;
            componente.costo = Convert.ToDecimal(task.Cost);
            componente.renglon = null;
            componente.ubicacionGeografica = null;
            componente.fechaInicio = Convert.ToDateTime(task.Start);
            componente.fechaFin = Convert.ToDateTime(task.Finish);
            componente.duracion = Convert.ToInt32(task.Duration);
            componente.duracionDimension = task.DurationText;
            componente.orden = null;
            componente.treepath = null;
            componente.nivel = 1;
            componente.esDeSigade = 0;
            componente.fuentePrestamo = null;
            componente.fuenteDonacion = null;
            componente.fuenteNacional = null;
            componente.fechaInicioReal = Convert.ToDateTime(task.ActualStart);
            componente.fechaFinReal = Convert.ToDateTime(task.ActualFinish);
            componente.inversionNueva = 0;
            return ComponenteDAO.guardarComponente(componente, false) ? componente : null;
        }

        public Subcomponente CrearSubComponente(Task task,Componente componente ,String usuario){


            SubcomponenteTipo componenteTipo = SubComponenteTipoDAO.getSubComponenteTipoPorId(SUBCOMPONENTE_TIPO_ID_DEFECTO);

            int year = DateTime.Now.Year;
            UnidadEjecutora unidadEjecutora = UnidadEjecutoraDAO.getUnidadEjecutora(year, ENTIDAD_ID_DEFECTO, UNIDAD_EJECUTORA_ID_DEFECTO);

            AcumulacionCosto acumulacionCosto = null;

            switch(task.FixedCostAccrual.Value){
                case 1:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(1);
                    break;
                case 2:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(2);
                    break;
                case 3:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(3);
                    break;
            }

            Subcomponente subcomponente = new Subcomponente();
            subcomponente.acumulacionCostos = acumulacionCosto;
            subcomponente.acumulacionCostoid = acumulacionCosto.id;
            subcomponente.componentes = componente;
            subcomponente.componenteid = componente.id;
            subcomponente.unidadEjecutoras = unidadEjecutora;
            subcomponente.ueunidadEjecutora = unidadEjecutora.unidadEjecutora;
            subcomponente.nombre = task.Name;
            subcomponente.descripcion = null;
            subcomponente.usuarioCreo = usuario;
            subcomponente.usuarioActualizo = null;
            subcomponente.fechaCreacion = DateTime.Now;
            subcomponente.fechaActualizacion = null;
            subcomponente.estado = 1;
            subcomponente.snip = null;
            subcomponente.programa = null;
            subcomponente.subcomponentes = null;
            subcomponente.proyecto = null;
            subcomponente.actividad = null;
            subcomponente.obra = null;
            subcomponente.latitud = null;
            subcomponente.longitud = null;
            subcomponente.costo = Convert.ToDecimal(task.Cost);
            subcomponente.renglon = null;
            subcomponente.ubicacionGeografica = null;
            subcomponente.fechaInicio = Convert.ToDateTime(task.Start);
            subcomponente.fechaFin = Convert.ToDateTime(task.Finish);
            subcomponente.duracion = Convert.ToInt32(task.Duration);
            subcomponente.duracionDimension = task.DurationText;
            subcomponente.orden = null;
            subcomponente.treepath = null;
            subcomponente.nivel = 2;
            subcomponente.fechaInicioReal = Convert.ToDateTime(task.ActualStart);
            subcomponente.fechaFinReal = Convert.ToDateTime(task.ActualFinish);
            subcomponente.inversionNueva = 0;

            return SubComponenteDAO.guardarSubComponente(subcomponente, false) ? subcomponente : null;
        }

        public Producto CrearProducto (Task task, Componente componente,Subcomponente subcomponente,String usuario){

            ProductoTipo productoTipo = new ProductoTipo();
            productoTipo.id = PRODUCTO_TIPO_ID_DEFECTO;
            UnidadEjecutora unidadEjecutora = UnidadEjecutoraDAO.getUnidadEjecutora(DateTime.Now.Year, ENTIDAD_ID_DEFECTO, UNIDAD_EJECUTORA_ID_DEFECTO);

            AcumulacionCosto acumulacionCosto = null;

            switch(task.FixedCostAccrual.Value){
                case 1:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(1);
                    break;
                case 2:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(2);
                    break;
                case 3:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(3);
                    break;
            }

            Producto producto = new Producto();
            producto.acumulacionCostos = acumulacionCosto;
            producto.acumulacionCostoid = acumulacionCosto.id;
            producto.componentes = null;
            producto.productoTipos = productoTipo;
            producto.productoTipoid = productoTipo.id;
            producto.subcomponentes = subcomponente;
            producto.subcomponenteid = subcomponente.id;
            producto.unidadEjecutoras = unidadEjecutora;
            producto.ueunidadEjecutora = unidadEjecutora.unidadEjecutora;
            producto.nombre = task.Name;
            producto.descripcion = null;
            producto.usuarioCreo = usuario;
            producto.usuarioActualizo = null;
            producto.fechaCreacion = DateTime.Now;
            producto.fechaActualizacion = null;
            producto.estado = 1;
            producto.programa = null;
            producto.subprograma = null;
            producto.proyecto = null;
            producto.actividad = null;
            producto.obra = null;
            producto.latitud = null;
            producto.longitud = null;
            producto.peso = null;
            producto.costo = Convert.ToDecimal(task.Cost);
            producto.renglon = null;
            producto.ubicacionGeografica = null;
            producto.fechaInicio = Utils.setDateCeroHoras(Convert.ToDateTime(task.Start));
            producto.fechaFin = Utils.setDateCeroHoras(Convert.ToDateTime(task.Finish));
            producto.duracion = Convert.ToInt32(task.Duration);
            producto.duracionDimension = task.DurationText;
            producto.orden = null;
            producto.treepath = null;
            producto.nivel = 3;
            producto.fechaInicioReal = Convert.ToDateTime(task.ActualStart);
            producto.fechaFinReal = Convert.ToDateTime(task.ActualFinish);
            producto.inversionNueva = 0;

            return ProductoDAO.guardarProducto(producto, false) ? producto : null;
        }

        public Subproducto CrearSubproducto(Task task, Producto producto, String usuario){

            SubproductoTipo subproductoTipo = new SubproductoTipo();
            subproductoTipo.id = SUBPRODUCTO_TIPO__ID_DEFECTO;

            UnidadEjecutora unidadEjecutroa = UnidadEjecutoraDAO.getUnidadEjecutora(DateTime.Now.Year, ENTIDAD_ID_DEFECTO, UNIDAD_EJECUTORA_ID_DEFECTO);

            AcumulacionCosto acumulacionCosto = null;

            switch(task.FixedCostAccrual.Value){
                case 1:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(1);
                    break;
                case 2:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(2);
                    break;
                case 3:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(3);
                    break;
            }

            Subproducto subproducto = new Subproducto();
            subproducto.acumulacionCostos = acumulacionCosto;
            subproducto.acumulacionCostoid = acumulacionCosto.id;
            subproducto.productos = producto;
            subproducto.productoid = producto.id;
            subproducto.subproductoTipos = subproductoTipo;
            subproducto.subproductoTipoid = subproductoTipo.id;
            subproducto.nombre = task.Name;
            subproducto.descripcion = null;
            subproducto.usuarioCreo = usuario;
            subproducto.usuarioActualizo = null;
            subproducto.fechaCreacion = DateTime.Now;
            subproducto.fechaActualizacion = null;
            subproducto.estado = 1;
            subproducto.programa = null;
            subproducto.subprograma = null;
            subproducto.proyecto = null;
            subproducto.actividad = null;
            subproducto.obra = null;
            subproducto.latitud = null;
            subproducto.longitud = null;
            subproducto.costo = Convert.ToDecimal(task.Cost);
            subproducto.renglon = null;
            subproducto.ubicacionGeografica = null;
            subproducto.fechaInicio = Utils.setDateCeroHoras(Convert.ToDateTime(task.Start));
            subproducto.fechaFin = Utils.setDateCeroHoras(Convert.ToDateTime(task.Finish));
            subproducto.duracion = Convert.ToInt32(task.Duration);
            subproducto.duracionDimension = task.DurationText;
            subproducto.orden = null;
            subproducto.treepath = null;
            subproducto.nivel = 4;
            subproducto.fechaInicioReal = Convert.ToDateTime(task.ActualStart);
            subproducto.fechaFinReal = Convert.ToDateTime(task.ActualFinish);
            subproducto.inversionNueva = 0;

            return SubproductoDAO.guardarSubproducto(subproducto, false) ? subproducto : null;
        }

        public Actividad CrearActividad(Task task, String usuario, int objetoId, int objetoTipo, int nivel, String path, int? proyectoBase, int? componenteBase,
            int? productoBase) {

            ActividadTipo actividadTipo = new ActividadTipo();
            actividadTipo.id = ACTIVIDAD_TIPO_ID_DEFECTO;

            int[] predecesores = GetListaPredecesores(task.Predecessors);

            Stitem itemPredecesor = null;
            if (predecesores != null && predecesores.Length > 0) {
                itemPredecesor = items[predecesores[0]];
            }

            AcumulacionCosto acumulacionCosto = null;

            switch (task.FixedCostAccrual.Value) {
                case 1:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(1);
                    break;
                case 2:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(2);
                    break;
                case 3:
                    acumulacionCosto = AcumulacionCostoDAO.getAcumulacionCostoById(3);
                    break;
            }

            int duracion = Convert.ToInt32(task.Duration);
            duracion = duracion > 0 ? duracion : 1;

            Actividad actividad = new Actividad();
            actividad.actividadTipos = actividadTipo;
            actividad.actividadTipoid = actividadTipo.id;
            actividad.acumulacionCostos = acumulacionCosto;
            actividad.acumulacionCosto = acumulacionCosto.id;
            actividad.nombre = task.Name;
            actividad.descripcion = null;
            actividad.fechaInicio = Utils.setDateCeroHoras(Convert.ToDateTime(task.Start));
            actividad.fechaFin = Utils.setDateCeroHoras(Convert.ToDateTime(task.Finish));
            actividad.usuarioCreo = usuario;
            actividad.usuarioActualizo = null;
            actividad.fechaCreacion = DateTime.Now;
            actividad.fechaActualizacion = null;
            actividad.estado = 1;
            actividad.snip = null;
            actividad.programa = null;
            actividad.subprograma = null;
            actividad.proyecto = null;
            actividad.actividad = null;
            actividad.obra = null;
            actividad.objetoId = objetoId;
            actividad.objetoTipo = objetoTipo;
            actividad.duracion = duracion;
            actividad.duracionDimension = task.DurationText;
            actividad.predObjetoId = itemPredecesor != null ? itemPredecesor.objetoId : default(int);
            actividad.predObjetoTipo = itemPredecesor != null ? itemPredecesor.objetoTipo : default(int);
            actividad.latitud = null;
            actividad.longitud = null;
            actividad.costo = Convert.ToDecimal(task.Cost);
            actividad.renglon = null;
            actividad.ubicacionGeografica = null;
            actividad.orden = null;
            actividad.treepath = null;
            actividad.nivel = nivel;
            actividad.proyectoBase = proyectoBase;
            actividad.componenteBase = componenteBase;
            actividad.productoBase = productoBase;
            actividad.fechaInicioReal = Convert.ToDateTime(task.ActualStart);
            actividad.fechaFinReal = Convert.ToDateTime(task.ActualFinish);
            actividad.inversionNueva = 0;

            return ActividadDAO.guardarActividad(actividad, false) ? actividad : null;
        }

        public void CargarItem(Task task, int objetoId, int objetoTipo, int indentacion) {
            Stitem item_ = new Stitem();
            item_.id = Convert.ToInt32(task.UniqueID);
            item_.contenido = task.Name;
            item_.indentacion = indentacion;
            item_.expandido = true;
            item_.fechaInicial = Convert.ToDateTime(task.Start);
            item_.fechaFinal = Convert.ToDateTime(task.Finish);
            item_.esHito = task.Milestone;
            item_.idPredecesores = GetListaPredecesores(task.Predecessors);
            item_.duracion = Convert.ToInt32(task.Duration);
            item_.unidades = task.DurationText;
            item_.objetoId = objetoId;
            item_.objetoTipo = objetoTipo;
            items.Add(Convert.ToInt32(task.UniqueID), item_);
        }


        public bool GetTask(ProjectFile projectFile, String usuario, int proyectoId, bool marcarCargado, int prestamoId)
        {
            items = new Dictionary<int, Stitem>();

            Proyecto proyecto = null;
            List<Componente> componentes = null;
            if (marcarCargado)
            {
                proyecto = ProyectoDAO.getProyecto(proyectoId);
                componentes = ComponenteDAO.getComponentesPorProyecto(proyectoId);

            }
            int ret = ListaJerarquica((Task)projectFile.ChildTasks.get(0), usuario, null, 0, multiproyecto ? -1 : 0,
                    proyecto, componentes, marcarCargado, 0, prestamoId);
            ProyectoDAO.calcularCostoyFechas(ret);
            return ret > 0;

        }

        private int ListaJerarquica(Task task, String usuario, Object objeto, int objetoTipo, int nivel, Proyecto proyecto_, List<Componente> componentes, bool marcarCargado, int posicionComponente, int prestamoId)
        {
            int ret = 1;
            try
            {
                bool tieneHijos = task.ChildTasks != null && task.ChildTasks.size() > 0;
                Object objeto_temp = null;
                MethodInfo getId = objeto != null ? objeto.GetType().GetMethod("getId") : null;
                MethodInfo getTreePath = objeto != null ? objeto.GetType().GetMethod("getTreePath") : null;

                switch (nivel)
                {
                    case 0:
                        Proyecto proyecto;
                        if (marcarCargado)
                        {
                            proyecto = proyecto_;
                            proyecto.projectCargado = 1;
                            ProyectoDAO.guardarProyecto(proyecto, false);
                        }
                        else
                        {
                            proyecto = CrearProyecto(task, usuario, prestamoId);
                        }

                        ret = proyecto.id;
                        objeto_temp = (Object)proyecto;
                        break;
                    case 1:
                        if (tieneHijos)
                        {
                            Componente componente = null;
                            if (marcarCargado)
                            {
                                componente = componentes.Count > posicionComponente ? componentes[posicionComponente] : null;
                            }
                            else
                            {
                                componente = CrearComponente(task, (Proyecto)objeto, usuario);
                            }

                            ret = componente != null ? componente.id : 1;
                            objeto_temp = (Object)componente;
                        }
                        else
                        {
                            if (objeto != null)
                            {
                                Proyecto objeto_padre = ((Proyecto)objeto);
                                Actividad actividad = CrearActividad(task, usuario, objeto_padre.id, 0, 2, 
                                    objeto_padre.treepath, objeto_padre.id, default(int), default(int));
                                CargarItem(task, actividad.id, 5, 2);
                                ret = actividad.id;
                                objeto_temp = (Object)actividad;
                            }
                        }
                        break;
                    case 2:
                        if (objeto != null)
                        {
                            if (tieneHijos)
                            {
                                Subcomponente subcomponente = null;
                                subcomponente = CrearSubComponente(task, (Componente)objeto, usuario);


                                ret = subcomponente.id;
                                objeto_temp = (Object)subcomponente;
                            }
                            else
                            {
                                if (objeto != null)
                                {
                                    Componente objeto_padre = ((Componente)objeto);
                                    Actividad actividad = CrearActividad(task, usuario, objeto_padre.id, 1, 2, 
                                        objeto_padre.treepath, objeto_padre.id, default(int), default(int));
                                    CargarItem(task, actividad.id, 5, 2);
                                    ret = actividad.id;
                                    objeto_temp = (Object)actividad;
                                }
                            }
                        }
                        break;
                    case 3:
                        if (objeto != null)
                        {
                            if (tieneHijos)
                            {
                                Producto producto = CrearProducto(task, null, (Subcomponente)objeto, usuario);
                                ret = producto.id;
                                objeto_temp = (Object)producto;
                            }
                            else
                            {
                                Actividad actividad = CrearActividad(task, usuario, (int)getId.Invoke(objeto,null), objetoTipo, 3, 
                                    (String)getTreePath.Invoke(objeto, null), null, null, null);
                                CargarItem(task, actividad.id, 5, 3);
                                ret = actividad.id;
                                objeto_temp = (Object)actividad;
                            }
                        }
                        break;
                    case 4:
                        if (objeto != null)
                        {
                            if (tieneHijos)
                            {
                                Subproducto subproducto = CrearSubproducto(task, (Producto)objeto, usuario);
                                ret = subproducto.id;
                                objeto_temp = (Object)subproducto;
                            }
                            else
                            {
                                Actividad actividad = CrearActividad(task, usuario, (int)getId.Invoke(objeto, null), objetoTipo, 4, 
                                    (String)getTreePath.Invoke(objeto, null), null, null, null);
                                CargarItem(task, actividad.id, 5, 4);
                                ret = actividad.id;
                                objeto_temp = (Object)actividad;
                            }
                        }
                        break;
                    default:
                        if (objeto != null)
                        {
                            Actividad actividad = CrearActividad(task, usuario, (int)getId.Invoke(objeto, null), objetoTipo , nivel, 
                                (String)getTreePath.Invoke(objeto, null), null, null, null);
                            CargarItem(task, actividad.id, 5, nivel);
                            ret = actividad.id;
                            objeto_temp = (Object)actividad;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "CProject.class", e);
                ret = -1;
            }

            return ret;
        }

        private int[] GetListaPredecesores(dynamic predecesores)
        {
            if (predecesores.Count > 0)
            {
                int[] arregloPrdecesores = new int[predecesores.Count];
                int i = 0;
                foreach (Relation relation in predecesores)
                {
                    arregloPrdecesores[i] = Convert.ToInt32(relation.TargetTask.UniqueID);
                }
                return arregloPrdecesores;
            }
            return null;
        }

        public String ExportarProject(int idProyecto, String lineaBase, String usuario)
        {
            string path = "";
            try
            {
                List<dynamic> estructuraProyecto = EstructuraProyectoDAO.getEstructuraProyecto(idProyecto, lineaBase);
                if (estructuraProyecto != null)
                {
                    project = new ProjectFile();

                    CustomFieldContainer cfc = project.CustomFields;
                    cfc.RegisterAliasValue("responsable", java.lang.Integer.valueOf(500), 500);

                    Task task0 = null;
                    Task task1 = null;
                    Task task2 = null;
                    Task task3 = null;
                    Task task4 = null;
                    Task task5 = null;


                    foreach (Object objeto in estructuraProyecto)
                    {
                        Object[] obj = (Object[])objeto;
                        int objPred = (int)obj[19];
                        java.lang.Number avance = (java.lang.Number)obj[18];

                        switch (((long)obj[2]))
                        {
                            case 0:
                                task0 = project.addTask();
                                task0.Name = (String)obj[1];
                                task0.Cost = (java.lang.Number)obj[8];
                                task0.ActualCost = (java.lang.Number)obj[8];
                                task0.FixedCost = (java.lang.Number)obj[8];
                                AsignarAcumulacionCosto(task0, obj[9] != null ? ((int)obj[9]) : 0);
                                break;
                            case 1:
                                task1 = task0.addTask();
                                task1.Name = (String)obj[1];
                                task1.Cost = (java.lang.Number)obj[8];
                                task1.ActualCost = (java.lang.Number)obj[8];
                                task1.FixedCost = (java.lang.Number)obj[8];
                                AsignarAcumulacionCosto(task1, obj[9] != null ? ((int)obj[9]) : 0);
                                break;
                            case 2:
                                task2 = task1.addTask();
                                task2.Name = (String)obj[1];
                                task2.Cost = (java.lang.Number)obj[8];
                                task2.ActualCost = (java.lang.Number)obj[8];
                                task2.FixedCost = (java.lang.Number)obj[8];
                                AsignarAcumulacionCosto(task2, obj[9] != null ? ((int)obj[9]) : 0);
                                break;
                            case 3:
                                task3 = task2.addTask();
                                task3.Name = (String)obj[1];
                                task3.Cost = (java.lang.Number)obj[8];
                                task3.ActualCost = (java.lang.Number)obj[8];
                                task3.FixedCost = (java.lang.Number)obj[8];
                                AsignarAcumulacionCosto(task3, obj[9] != null ? ((int)obj[9]) : 0);
                                break;
                            case 4:
                                task4 = task3.addTask();
                                task4.Name = (String)obj[1];
                                task4.Cost = (java.lang.Number)obj[8];
                                task4.ActualCost = (java.lang.Number)obj[8];
                                task4.FixedCost = (java.lang.Number)obj[8];
                                AsignarAcumulacionCosto(task4, obj[9] != null ? ((int)obj[9]) : 0);
                                break;
                            case 5:
                                if (objPred == 0)
                                {
                                    task5 = task0.addTask();
                                    task5.Name = (String)obj[1];
                                    task5.Start = (java.util.Date)obj[4];
                                    task5.Finish = (java.util.Date)obj[5];
                                    task5.Cost = (java.lang.Number)obj[8];
                                    task5.ActualCost = (java.lang.Number)obj[8];
                                    task5.FixedCost = (java.lang.Number)obj[8];
                                    task5.PercentageComplete = avance;
                                    task5.Duration = Duration.getInstance((int)obj[6], TimeUnit.DAYS);
                                    task5.ActualStart = (java.util.Date)obj[16];
                                    task5.ActualFinish = (java.util.Date)obj[17];
                                    task5.Milestone = false;
                                    AsignarAcumulacionCosto(task5, obj[9] != null ? ((int)obj[9]) : 0);
                                    AsignarColumnasAdicionales(task5, ((int)obj[2]), (int)obj[0]);

                                }
                                else if (objPred == 1)
                                {
                                    task5 = task1.addTask();
                                    task5.Name = (String)obj[1];
                                    task5.Start = (java.util.Date)obj[4];
                                    task5.Finish = (java.util.Date)obj[5];
                                    task5.Cost = (java.lang.Number)obj[8];
                                    task5.ActualCost = (java.lang.Number)obj[8];
                                    task5.FixedCost = (java.lang.Number)obj[8];
                                    task5.PercentageComplete = avance;
                                    task5.Duration = Duration.getInstance((int)obj[6], TimeUnit.DAYS);
                                    task5.ActualStart = (java.util.Date)obj[16];
                                    task5.ActualFinish = (java.util.Date)obj[17];
                                    AsignarAcumulacionCosto(task5, obj[9] != null ? ((int)obj[9]) : 0);
                                    AsignarColumnasAdicionales(task5, ((int)obj[2]), (int)obj[0]);

                                }
                                else if (objPred == 2)
                                {
                                    task5 = task2.addTask();
                                    task5.Name = (String)obj[1];
                                    task5.Start = (java.util.Date)obj[4];
                                    task5.Finish = (java.util.Date)obj[5];
                                    task5.Cost = (java.lang.Number)obj[8];
                                    task5.ActualCost = (java.lang.Number)obj[8];
                                    task5.FixedCost = (java.lang.Number)obj[8];
                                    task5.PercentageComplete = avance;
                                    task5.Duration = Duration.getInstance((int)obj[6], TimeUnit.DAYS);
                                    task5.ActualStart = (java.util.Date)obj[16];
                                    task5.ActualFinish = (java.util.Date)obj[17];
                                    AsignarAcumulacionCosto(task5, obj[9] != null ? ((int)obj[9]) : 0);

                                }
                                else if (objPred == 3)
                                {
                                    task5 = task3.addTask();
                                    task5.Name = (String)obj[1];
                                    task5.Start = (java.util.Date)obj[4];
                                    task5.Finish = (java.util.Date)obj[5];
                                    task5.Cost = (java.lang.Number)obj[8];
                                    task5.ActualCost = (java.lang.Number)obj[8];
                                    task5.FixedCost = (java.lang.Number)obj[8];
                                    task5.PercentageComplete = avance;
                                    task5.Duration = Duration.getInstance((int)obj[6], TimeUnit.DAYS);
                                    task5.ActualStart = (java.util.Date)obj[16];
                                    task5.ActualFinish = (java.util.Date)obj[17];
                                    AsignarAcumulacionCosto(task5, obj[9] != null ? ((int)obj[9]) : 0);
                                    AsignarColumnasAdicionales(task5, ((int)obj[2]), (int)obj[0]);

                                }
                                else if (objPred == 4)
                                {
                                    task5 = task4.addTask();
                                    task5.Name = (String)obj[1];
                                    task5.Start = (java.util.Date)obj[4];
                                    task5.Finish = (java.util.Date)obj[5];
                                    task5.Cost = (java.lang.Number)obj[8];
                                    task5.ActualCost = (java.lang.Number)obj[8];
                                    task5.FixedCost = (java.lang.Number)obj[8];
                                    task5.PercentageComplete = avance;
                                    task5.Duration = Duration.getInstance((int)obj[6], TimeUnit.DAYS);
                                    task5.ActualStart = (java.util.Date)obj[16];
                                    task5.ActualFinish = (java.util.Date)obj[17];
                                    AsignarAcumulacionCosto(task5, obj[9] != null ? ((int)obj[9]) : 0);
                                    AsignarColumnasAdicionales(task5, ((int)obj[2]), (int)obj[0]);
                                }
                                else if (objPred == 5)
                                {
                                    Task task6 = task5.addTask();
                                    task6.Name = (String)obj[1];
                                    task6.Start = (java.util.Date)obj[4];
                                    task6.Finish = (java.util.Date)obj[5];
                                    task6.Cost = (java.lang.Number)obj[8];
                                    task6.ActualCost = (java.lang.Number)obj[8];
                                    task6.FixedCost = (java.lang.Number)obj[8];
                                    task6.PercentageComplete = avance;
                                    task6.Duration = Duration.getInstance((int)obj[6], TimeUnit.DAYS);
                                    task6.ActualStart = (java.util.Date)obj[16];
                                    task6.ActualFinish = (java.util.Date)obj[17];
                                    AsignarAcumulacionCosto(task6, obj[9] != null ? ((int)obj[9]) : 0);
                                    AsignarColumnasAdicionales(task6, ((int)obj[2]), (int)obj[0]);
                                }
                                break;
                        }
                    }
                }

                MSPDIWriter writer = new MSPDIWriter();
                path = "/SIPRO/archivos/temporales/Programa.xml";
                writer.write(project, path);
            }
            catch (Exception e)
            {
                CLogger.write("1", "CProject.class", e);
            }


            return path;
        }

        public void AsignarColumnasAdicionales(Task task, int objetoTipo, int objetoId)
        {
            switch (objetoTipo)
            {
                case 5:
                    String nombre = "";
                    AsignacionRaci recurso = AsignacionRaciDAO.getAsignacionPorRolTarea(objetoId, objetoTipo, "r", null);
                    if (recurso != null)
                    {
                        nombre = recurso.colaboradors.pnombre
                                + (recurso.colaboradors.snombre != null ? " " + recurso.colaboradors.snombre : "")
                                + recurso.colaboradors.papellido +
                                (recurso.colaboradors.sapellido != null ? " " + recurso.colaboradors.sapellido : "");
                        task.ResourceNames = nombre;
                    }

                    if (nombre.Length > 0)
                        task.setText(1, nombre);
                    break;
            }
        }

        public void AsignarAcumulacionCosto(Task task, int tipoAcumulacion)
        {
            if (tipoAcumulacion == 1)
                task.FixedCostAccrual = AccrueType.START;
            else if (tipoAcumulacion == 2)
                task.FixedCostAccrual = AccrueType.PRORATED;
            else
                task.FixedCostAccrual = AccrueType.END;

        }*/
    }
}
