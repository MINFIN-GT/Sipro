using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;
using FluentValidation.Results;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace SProyecto.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ProyectoController : Controller
    {
        private class datos
        {
            public int id;
            public String nombre;
            public String objetivo;
            public String descripcion;
            public long snip;
            public int proyectotipoid;
            public String proyectotipo;
            public String unidadejecutora;
            public int unidadejecutoraid;
            public int entidadentidad;
            public String entidadnombre;
            public int ejercicio;
            public String fechaCreacion;
            public String usuarioCreo;
            public String fechaactualizacion;
            public String usuarioactualizo;
            public int? programa;
            public int? subprograma;
            public int? proyecto;
            public int? actividad;
            public int? obra;
            public int? renglon;
            public int? ubicacionGeografica;
            public String longitud;
            public String latitud;
            public int directorProyectoId;
            public String directorProyectoNmbre;
            public decimal costo;
            public int acumulacionCosto;
            public String acumulacionCostoNombre;
            public String objetivoEspecifico;
            public String visionGeneral;
            public int ejecucionFisicaReal;
            public int proyectoClase;
            public int projectCargado;
            public int prestamoId;
            public String fechaInicio;
            public String fechaFin;
            public String observaciones;
            public String fechaInicioReal;
            public String fechaFinReal;
            public int congelado;
            public int coordinador;
            public int porcentajeAvance;
            public bool permisoEditarCongelar;
            public int lineaBaseId;
        };

        private class stdatadinamico
        {
            public String id;
            public String tipo;
            public String label;
            public String valor;
            public String valor_f;
        }

        private class stcomponentessigade
        {
            public int id;
            public String nombre;
            public String tipoMoneda;
            public decimal techo;
            public int orden;
            public List<stunidadejecutora> unidadesEjecutoras;
        }

        private class stunidadejecutora
        {
            public int id;
            public String nombre;
            public String entidad;
            public int ejercicio;
            public decimal prestamo;
            public decimal donacion;
            public decimal nacional;
        }

        private class stpepdetalle
        {
            public int proyectoid;
            public String observaciones;
            public String alertivos;
            public String elaborado;
            public String aprobado;
            public String autoridad;
        }

        private class stlineasbase
        {
            public int id;
            public int proyectoid;
            public String nombre;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public bool sobreescribir;
        }

        // GET api/Proyecto/Proyectos
        [HttpGet("{prestamoid}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult Proyecto(int prestamoid)
        {
            try
            {
                List<Proyecto> proyectos = prestamoid > 0 ? ProyectoDAO.getProyectos(prestamoid, User.Identity.Name) : ProyectoDAO.getProyectos(User.Identity.Name);

                List<datos> datos_ = new List<datos>();
                foreach (Proyecto proyecto in proyectos)
                {
                    datos dato = new datos();
                    dato.id = proyecto.id;
                    dato.nombre = proyecto.nombre;
                    dato.objetivo = proyecto.objetivo;
                    dato.descripcion = proyecto.descripcion;
                    dato.snip = Convert.ToInt64(proyecto.snip);

                    proyecto.proyectoTipos = ProyectoTipoDAO.getProyectoTipoPorId(proyecto.proyectoTipoid);
                    dato.proyectotipo = proyecto.proyectoTipos.nombre;
                    dato.proyectotipoid = proyecto.proyectoTipos.id;

                    proyecto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(proyecto.ejercicio, proyecto.entidad ?? default(int), proyecto.ueunidadEjecutora);
                    dato.unidadejecutora = proyecto.unidadEjecutoras != null ? proyecto.unidadEjecutoras.nombre : "";

                    if (proyecto.unidadEjecutoras != null)
                    {
                        proyecto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(proyecto.unidadEjecutoras.entidadentidad, proyecto.ejercicio);
                    }

                    dato.unidadejecutoraid = proyecto.unidadEjecutoras != null ? proyecto.unidadEjecutoras.unidadEjecutora : default(int);
                    dato.entidadentidad = proyecto.unidadEjecutoras != null ? proyecto.unidadEjecutoras.entidadentidad : default(int);

                    dato.entidadnombre = proyecto.unidadEjecutoras.entidads != null ? proyecto.unidadEjecutoras.entidads.nombre : default(string);
                    dato.ejercicio = proyecto.unidadEjecutoras != null ? proyecto.unidadEjecutoras.ejercicio : default(int);

                    dato.fechaCreacion = proyecto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    dato.usuarioCreo = proyecto.usuarioCreo;
                    dato.fechaactualizacion = proyecto.fechaActualizacion != null ? proyecto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.usuarioactualizo = proyecto.usuarioActualizo;
                    dato.programa = proyecto.programa ?? default(int);
                    dato.subprograma = proyecto.subprograma ?? default(int);
                    dato.proyecto = proyecto.proyecto ?? default(int);
                    dato.obra = proyecto.obra ?? default(int);
                    dato.actividad = proyecto.actividad ?? default(int);
                    dato.longitud = proyecto.longitud ?? default(string);
                    dato.latitud = proyecto.latitud ?? default(string);
                    dato.costo = proyecto.costo ?? default(decimal);

                    proyecto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(proyecto.acumulacionCostoid));
                    dato.acumulacionCosto = Convert.ToInt32(proyecto.acumulacionCostos.id);
                    dato.acumulacionCostoNombre = proyecto.acumulacionCostos.nombre;
                    dato.objetivoEspecifico = proyecto.objetivoEspecifico;
                    dato.visionGeneral = proyecto.visionGeneral;

                    dato.directorProyectoId = proyecto.directorProyecto ?? default(int);

                    Colaborador colaborador = ColaboradorDAO.getColaborador(proyecto.directorProyecto ?? default(int));
                    dato.directorProyectoNmbre = colaborador != null ? (colaborador.pnombre
                                            + " " + colaborador.snombre
                                            + " " + colaborador.papellido
                                            + " " + colaborador.sapellido) : null;
                    dato.ejecucionFisicaReal = proyecto.ejecucionFisicaReal ?? default(int);

                    Etiqueta etiqueta = EtiquetaDAO.getEtiquetaPorId(proyecto.proyectoClase);

                    dato.proyectoClase = etiqueta.id;
                    dato.projectCargado = proyecto.projectCargado ?? default(int);
                    dato.prestamoId = proyecto.prestamoid ?? default(int);
                    dato.fechaInicio = proyecto.fechaInicio != null ? proyecto.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.fechaFin = proyecto.fechaFin != null ? proyecto.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.observaciones = proyecto.observaciones;
                    dato.fechaInicioReal = proyecto.fechaInicioReal != null ? proyecto.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.fechaFinReal = proyecto.fechaFinReal != null ? proyecto.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.congelado = proyecto.congelado ?? default(int);
                    dato.coordinador = proyecto.coordinador ?? default(int);
                    datos_.Add(dato);
                }

                return Ok(new { success = true, entidades = datos_ });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/values/5
        [HttpGet("{unidadEjecutoraId}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ProyectosPorUnidadEjecutora(int unidadEjecutoraId)
        {
            try
            {
                List<Proyecto> proyectos = ProyectoDAO.getProyectosPorUnidadEjecutora(User.Identity.Name, unidadEjecutoraId);

                List<datos> datos_ = new List<datos>();
                foreach (Proyecto proyecto in proyectos)
                {
                    datos dato = new datos();
                    dato.id = proyecto.id;
                    dato.nombre = proyecto.nombre;
                    dato.objetivo = proyecto.objetivo;
                    dato.descripcion = proyecto.descripcion;
                    dato.snip = Convert.ToInt64(proyecto.snip);

                    proyecto.proyectoTipos = ProyectoTipoDAO.getProyectoTipoPorId(proyecto.proyectoTipoid);
                    dato.proyectotipo = proyecto.proyectoTipos.nombre;
                    dato.proyectotipoid = proyecto.proyectoTipos.id;

                    proyecto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(proyecto.ejercicio, proyecto.entidad ?? default(int), proyecto.ueunidadEjecutora);
                    dato.unidadejecutora = proyecto.unidadEjecutoras != null ? proyecto.unidadEjecutoras.nombre : "";

                    if (proyecto.unidadEjecutoras != null)
                    {
                        proyecto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(proyecto.unidadEjecutoras.entidadentidad, proyecto.ejercicio);
                    }

                    dato.unidadejecutoraid = proyecto.unidadEjecutoras != null ? proyecto.unidadEjecutoras.unidadEjecutora : default(int);
                    dato.entidadentidad = proyecto.unidadEjecutoras != null ? proyecto.unidadEjecutoras.entidadentidad : default(int);

                    dato.entidadnombre = proyecto.unidadEjecutoras.entidads != null ? proyecto.unidadEjecutoras.entidads.nombre : default(string);
                    dato.ejercicio = proyecto.unidadEjecutoras != null ? proyecto.unidadEjecutoras.ejercicio : default(int);

                    dato.fechaCreacion = proyecto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    dato.usuarioCreo = proyecto.usuarioCreo;
                    dato.fechaactualizacion = proyecto.fechaActualizacion != null ? proyecto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.usuarioactualizo = proyecto.usuarioActualizo;
                    dato.programa = proyecto.programa ?? default(int);
                    dato.subprograma = proyecto.subprograma ?? default(int);
                    dato.proyecto = proyecto.proyecto ?? default(int);
                    dato.obra = proyecto.obra ?? default(int);
                    dato.actividad = proyecto.actividad ?? default(int);
                    dato.longitud = proyecto.longitud ?? default(string);
                    dato.latitud = proyecto.latitud ?? default(string);
                    dato.costo = proyecto.costo ?? default(decimal);

                    proyecto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(proyecto.acumulacionCostoid));
                    dato.acumulacionCosto = Convert.ToInt32(proyecto.acumulacionCostos.id);
                    dato.acumulacionCostoNombre = proyecto.acumulacionCostos.nombre;
                    dato.objetivoEspecifico = proyecto.objetivoEspecifico;
                    dato.visionGeneral = proyecto.visionGeneral;

                    dato.directorProyectoId = proyecto.directorProyecto ?? default(int);

                    Colaborador colaborador = ColaboradorDAO.getColaborador(proyecto.directorProyecto ?? default(int));
                    dato.directorProyectoNmbre = colaborador != null ? (colaborador.pnombre
                                            + " " + colaborador.snombre
                                            + " " + colaborador.papellido
                                            + " " + colaborador.sapellido) : null;
                    dato.ejecucionFisicaReal = proyecto.ejecucionFisicaReal ?? default(int);

                    Etiqueta etiqueta = EtiquetaDAO.getEtiquetaPorId(proyecto.proyectoClase);

                    dato.proyectoClase = etiqueta.id;
                    dato.projectCargado = proyecto.projectCargado ?? default(int);
                    dato.prestamoId = proyecto.prestamoid ?? default(int);
                    dato.fechaInicio = proyecto.fechaInicio != null ? proyecto.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.fechaFin = proyecto.fechaFin != null ? proyecto.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.observaciones = proyecto.observaciones;
                    dato.fechaInicioReal = proyecto.fechaInicioReal != null ? proyecto.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.fechaFinReal = proyecto.fechaFinReal != null ? proyecto.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.congelado = proyecto.congelado ?? default(int);
                    dato.coordinador = proyecto.coordinador ?? default(int);
                    datos_.Add(dato);
                }

                return Ok(new { success = true, entidades = datos_ });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/values
        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ProyectoPagina([FromBody]dynamic value)
        {
            try
            {
                int prestamoId = value.prestamoId != null ? (int)value.prestamoId : 0;
                int pagina = value.pagina != null ? (int)value.pagina : 0;
                int numeroProyecto = value.numeroproyecto != null ? (int)value.numeroproyecto : 0;
                String filtro_nombre = value.filtro_nombre;
                String filtro_usuario_creo = value.filtro_usuario_creo;
                String filtro_fecha_creacion = value.filtro_fecha_creacion;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;

                List<Proyecto> proyectos = ProyectoDAO.getProyectosPagina(pagina, numeroProyecto,
                        filtro_nombre, filtro_usuario_creo, filtro_fecha_creacion, columna_ordenada, orden_direccion, User.Identity.Name, prestamoId);
                List<datos> datos_ = new List<datos>();
                foreach (Proyecto proyecto in proyectos)
                {
                    datos dato = new datos();
                    dato.id = proyecto.id;
                    dato.nombre = proyecto.nombre;
                    dato.objetivo = proyecto.objetivo;
                    dato.descripcion = proyecto.descripcion;
                    dato.snip = Convert.ToInt64(proyecto.snip);

                    proyecto.proyectoTipos = ProyectoTipoDAO.getProyectoTipoPorId(proyecto.proyectoTipoid);
                    dato.proyectotipo = proyecto.proyectoTipos.nombre;
                    dato.proyectotipoid = proyecto.proyectoTipos.id;

                    proyecto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(proyecto.ejercicio, proyecto.entidad ?? default(int), proyecto.ueunidadEjecutora);
                    if (proyecto.unidadEjecutoras != null)
                    {
                        dato.unidadejecutora = proyecto.unidadEjecutoras.nombre;
                        dato.unidadejecutoraid = proyecto.unidadEjecutoras.unidadEjecutora;
                        dato.entidadentidad = proyecto.unidadEjecutoras.entidadentidad;

                        proyecto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(proyecto.unidadEjecutoras.entidadentidad, proyecto.unidadEjecutoras.ejercicio);

                        if (proyecto.unidadEjecutoras.entidads != null)
                            dato.entidadnombre = proyecto.unidadEjecutoras.entidads.nombre;

                        dato.ejercicio = proyecto.unidadEjecutoras.ejercicio;
                    }

                    dato.fechaCreacion = proyecto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    dato.usuarioCreo = proyecto.usuarioCreo;
                    dato.fechaactualizacion = proyecto.fechaActualizacion != null ? proyecto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.usuarioactualizo = proyecto.usuarioActualizo;
                    dato.programa = proyecto.programa;
                    dato.subprograma = proyecto.subprograma;
                    dato.proyecto = proyecto.proyecto;
                    dato.obra = proyecto.obra;
                    dato.actividad = proyecto.actividad;
                    dato.renglon = proyecto.renglon;
                    dato.ubicacionGeografica = proyecto.ubicacionGeografica;
                    dato.longitud = proyecto.longitud;
                    dato.latitud = proyecto.latitud;

                    proyecto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(proyecto.acumulacionCostoid));
                    dato.acumulacionCosto = proyecto.acumulacionCostos != null ? Convert.ToInt32(proyecto.acumulacionCostos.id) : default(int);
                    dato.acumulacionCostoNombre = proyecto.acumulacionCostos != null ? proyecto.acumulacionCostos.nombre : null;
                    dato.objetivoEspecifico = proyecto.objetivoEspecifico;
                    dato.visionGeneral = proyecto.visionGeneral;

                    dato.directorProyectoId = proyecto.directorProyecto ?? default(int);

                    Colaborador colaborador = ColaboradorDAO.getColaborador(proyecto.directorProyecto ?? default(int));

                    dato.directorProyectoNmbre = colaborador != null ? (colaborador.pnombre
                                            + " " + colaborador.snombre
                                            + " " + colaborador.papellido
                                            + " " + colaborador.sapellido) : null;

                    dato.ejecucionFisicaReal = proyecto.ejecucionFisicaReal ?? default(int);


                    Etiqueta etiqueta = EtiquetaDAO.getEtiquetaPorId(proyecto.proyectoClase);
                    dato.proyectoClase = etiqueta.id;

                    dato.projectCargado = proyecto.projectCargado ?? default(int);
                    dato.prestamoId = proyecto.prestamoid ?? default(int);
                    dato.costo = proyecto.costo ?? default(decimal);
                    dato.fechaInicio = proyecto.fechaInicio != null ? proyecto.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.fechaFin = proyecto.fechaFin != null ? proyecto.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.observaciones = proyecto.observaciones;
                    dato.fechaInicioReal = proyecto.fechaInicioReal != null ? proyecto.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.fechaFinReal = proyecto.fechaFinReal != null ? proyecto.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.congelado = proyecto.congelado ?? default(int);
                    dato.coordinador = proyecto.coordinador ?? default(int);
                    datos_.Add(dato);
                }

                return Ok(new { success = true, proyectos = datos_ });
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ProyectoPaginaDisponibles([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 0;
                int numeroProyecto = value.numeroproyecto != null ? value.numeroproyecto : 0;
                String filtro_nombre = value.filtro_nombre;
                String filtro_usuario_creo = value.filtro_usuario_creo;
                String filtro_fecha_creacion = value.filtro_fecha_creacion;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;
                String idsProyectos = value.idsproyectos;
                List<Proyecto> proyectos = ProyectoDAO.getProyectosPaginaDisponibles(pagina, numeroProyecto,
                        filtro_nombre, filtro_usuario_creo, filtro_fecha_creacion, columna_ordenada, orden_direccion, idsProyectos);
                List<datos> datos_ = new List<datos>();
                foreach (Proyecto proyecto in proyectos)
                {
                    datos dato = new datos();
                    dato.id = proyecto.id;
                    dato.nombre = proyecto.nombre;
                    dato.objetivo = proyecto.objetivo;
                    dato.descripcion = proyecto.descripcion;
                    dato.snip = Convert.ToInt64(proyecto.snip);

                    proyecto.proyectoTipos = ProyectoTipoDAO.getProyectoTipoPorId(proyecto.proyectoTipoid);
                    dato.proyectotipo = proyecto.proyectoTipos.nombre;
                    dato.proyectotipoid = proyecto.proyectoTipos.id;

                    proyecto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(proyecto.ejercicio, proyecto.entidad ?? default(int), proyecto.ueunidadEjecutora);
                    if (proyecto.unidadEjecutoras != null)
                    {
                        dato.unidadejecutora = proyecto.unidadEjecutoras.nombre;
                        dato.unidadejecutoraid = proyecto.unidadEjecutoras.unidadEjecutora;
                        dato.entidadentidad = proyecto.unidadEjecutoras.entidadentidad;

                        proyecto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(proyecto.unidadEjecutoras.entidadentidad, proyecto.unidadEjecutoras.ejercicio);

                        if (proyecto.unidadEjecutoras.entidads != null)
                            dato.entidadnombre = proyecto.unidadEjecutoras.entidads.nombre;

                        dato.ejercicio = proyecto.unidadEjecutoras.ejercicio;
                    }

                    dato.fechaCreacion = proyecto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    dato.usuarioCreo = proyecto.usuarioCreo;
                    dato.fechaactualizacion = proyecto.fechaActualizacion != null ? proyecto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.usuarioactualizo = proyecto.usuarioActualizo;
                    dato.programa = proyecto.programa;
                    dato.subprograma = proyecto.subprograma;
                    dato.proyecto = proyecto.proyecto;
                    dato.obra = proyecto.obra;
                    dato.actividad = proyecto.actividad;
                    dato.renglon = proyecto.renglon;
                    dato.ubicacionGeografica = proyecto.ubicacionGeografica;
                    dato.longitud = proyecto.longitud;
                    dato.latitud = proyecto.latitud;

                    proyecto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(proyecto.acumulacionCostoid));
                    dato.acumulacionCosto = proyecto.acumulacionCostos != null ? Convert.ToInt32(proyecto.acumulacionCostos.id) : default(int);
                    dato.acumulacionCostoNombre = proyecto.acumulacionCostos != null ? proyecto.acumulacionCostos.nombre : null;
                    dato.objetivoEspecifico = proyecto.objetivoEspecifico;
                    dato.visionGeneral = proyecto.visionGeneral;

                    dato.directorProyectoId = proyecto.directorProyecto ?? default(int);

                    Colaborador colaborador = ColaboradorDAO.getColaborador(proyecto.directorProyecto ?? default(int));

                    dato.directorProyectoNmbre = colaborador != null ? (colaborador.pnombre
                                            + " " + colaborador.snombre
                                            + " " + colaborador.papellido
                                            + " " + colaborador.sapellido) : null;

                    dato.ejecucionFisicaReal = proyecto.ejecucionFisicaReal ?? default(int);


                    Etiqueta etiqueta = EtiquetaDAO.getEtiquetaPorId(proyecto.proyectoClase);
                    dato.proyectoClase = etiqueta.id;

                    dato.projectCargado = proyecto.projectCargado ?? default(int);
                    dato.prestamoId = proyecto.prestamoid ?? default(int);
                    dato.costo = proyecto.costo ?? default(decimal);
                    dato.fechaInicio = proyecto.fechaInicio != null ? proyecto.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.fechaFin = proyecto.fechaFin != null ? proyecto.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.observaciones = proyecto.observaciones;
                    dato.fechaInicioReal = proyecto.fechaInicioReal != null ? proyecto.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.fechaFinReal = proyecto.fechaFinReal != null ? proyecto.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.congelado = proyecto.congelado ?? default(int);
                    dato.coordinador = proyecto.coordinador ?? default(int);
                    datos_.Add(dato);
                }

                return Ok(new { success = true, proyectos = datos_ });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Préstamos o Proyectos - Crear")]
        public IActionResult Proyecto([FromBody]dynamic value)
        {
            try
            {
                ProyectoValidator validator = new ProyectoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    bool result = false;
                    bool esnuevo = value.esnuevo == 1 ? true : false;
                    int id = value.id;
                    String nombre = value.nombre;
                    long snip = value.snip;
                    String objetivo = value.objetivo;
                    String descripcion = value.descripcion;
                    int ejercicio = value.ejercicio ?? default(int);

                    int programa = value.programa;
                    int subPrograma = value.subprograma;
                    int proyecto_ = value.proyecto_;
                    int actividad = value.actividad;
                    int obra = value.obra;
                    String longitud = value.longitud;
                    String latitud = value.latitud;
                    int renglon = value.renglon;
                    int ubicacionGeografica = value.ubicacionGeografica;
                    decimal costo = value.costo;
                    String objetivoEspecifico = value.objetoivoEspecifico;
                    String visionGeneral = value.visionGeneral;
                    int unidad_ejecutora = value.unidadejecutoraid;
                    int entidad = value.entidadid;
                    int ejecucionFisicaReal = value.ejecucionFisicaReal;
                    int proyectoClase = value.proyectoClase == null ? 1 : value.proyectoClase;
                    Etiqueta etiqueta = EtiquetaDAO.getEtiquetaPorId(proyectoClase);
                    int projectCargado = value.projectCargado == null ? 0 : value.projectCargado;
                    int? prestamoId = value.prestamoId;
                    String observaciones = value.observaciones;
                    int porcentajeAvance = value.porcentajeAvance == null ? 0 : value.porcentajeAvance;
                    Prestamo prestamo = null;
                    if (prestamoId != null)
                    {
                        prestamo = PrestamoDAO.getPrestamoById(prestamoId ?? default(int));
                    }

                    AcumulacionCosto acumulacionCosto = null;
                    if (value.acumulacionCosto != null)
                    {
                        acumulacionCosto = new AcumulacionCosto();
                        acumulacionCosto.id = value.acumulacionCosto;
                    }

                    String enunciadoAlcance = value.enunciadoAlcance;

                    ProyectoTipo proyectoTipo = new ProyectoTipo();
                    proyectoTipo.id = value.proyectotipoid;

                    UnidadEjecutora unidadEjecutora = UnidadEjecutoraDAO.getUnidadEjecutora(ejercicio, entidad, unidad_ejecutora);

                    Colaborador directorProyecto = null;
                    if (value.directorProyecto != null && value.directorProyecto.Length > 0)
                    {
                        directorProyecto = new Colaborador();
                        directorProyecto.id = value.directorProyecto;
                    }

                    Proyecto proyecto = new Proyecto();
                    proyecto.acumulacionCostos = acumulacionCosto;
                    proyecto.acumulacionCostoid = acumulacionCosto.id;
                    proyecto.directorProyecto = directorProyecto.id;
                    proyecto.proyectoClase = etiqueta.id;
                    proyecto.prestamos = prestamo;
                    proyecto.prestamoid = prestamo.id;
                    proyecto.proyectoTipos = proyectoTipo;
                    proyecto.proyectoTipoid = proyectoTipo.id;
                    proyecto.unidadEjecutoras = unidadEjecutora;
                    proyecto.ueunidadEjecutora = unidadEjecutora.unidadEjecutora;
                    proyecto.nombre = nombre;
                    proyecto.descripcion = descripcion;
                    proyecto.usuarioCreo = User.Identity.Name;
                    proyecto.fechaCreacion = DateTime.Now;
                    proyecto.estado = 1;
                    proyecto.snip = Convert.ToInt32(snip);
                    proyecto.programa = programa;
                    proyecto.subprograma = subPrograma;
                    proyecto.proyecto = proyecto_;
                    proyecto.actividad = actividad;
                    proyecto.obra = obra;
                    proyecto.latitud = latitud;
                    proyecto.longitud = longitud;
                    proyecto.objetivo = objetivo;
                    proyecto.enunciadoAlcance = enunciadoAlcance;
                    proyecto.costo = costo;
                    proyecto.objetivoEspecifico = objetivoEspecifico;
                    proyecto.visionGeneral = visionGeneral;
                    proyecto.renglon = renglon;
                    proyecto.ubicacionGeografica = ubicacionGeografica;
                    proyecto.duracion = 0;
                    proyecto.ejecucionFisicaReal = ejecucionFisicaReal;
                    proyecto.projectCargado = projectCargado;
                    proyecto.observaciones = observaciones;

                    result = ProyectoDAO.guardarProyecto(proyecto, false);

                    if (result && proyecto.coordinador != null && proyecto.coordinador.Equals(1))
                    {
                        if (!prestamo.porcentajeAvance.Equals(porcentajeAvance))
                        {
                            prestamo.porcentajeAvance = porcentajeAvance;
                            result = result && PrestamoDAO.guardarPrestamo(prestamo);
                        }
                    }
                    if (result)
                    {
                        List<stdatadinamico> datos = JsonConvert.DeserializeObject<stdatadinamico>(value.datadinamica);

                        foreach (stdatadinamico data in datos)
                        {
                            if (data.valor != null && data.valor.Length > 0 && data.valor.CompareTo("null") != 0)
                            {
                                ProyectoPropiedad proyectoPropiedad = ProyectoPropiedadDAO.getProyectoPropiedadPorId(Convert.ToInt32(data.id));
                                ProyectoPropiedadValor valor = new ProyectoPropiedadValor();
                                valor.proyectos = proyecto;
                                valor.proyectoid = proyecto.id;
                                valor.proyectoPropiedads = proyectoPropiedad;
                                valor.proyectoPropiedadid = proyectoPropiedad.id;
                                valor.usuarioCreo = User.Identity.Name;
                                valor.fechaCreacion = DateTime.Now;
                                valor.estado = 1;

                                switch (proyectoPropiedad.datoTipoid)
                                {
                                    case 1:
                                        valor.valorString = data.valor;
                                        break;
                                    case 2:
                                        valor.valorEntero = Convert.ToInt32(data.valor);
                                        break;
                                    case 3:
                                        valor.valorDecimal = Convert.ToDecimal(data.valor);
                                        break;
                                    case 4:
                                        valor.valorEntero = data.valor == "true" ? 1 : 0;
                                        break;
                                    case 5:
                                        valor.valorTiempo = Convert.ToDateTime(data.valor_f);
                                        break;
                                }
                                result = (result && ProyectoPropiedadValorDAO.guardarProyectoPropiedadValor(valor));
                            }
                        }
                    }
                    if (result)
                    {
                        String[] impactos = value.impactos != null && value.impactos.Length > 0 ? value.impactos.toString().Split("~") : null;
                        if (impactos != null && impactos.Length > 0)
                        {
                            foreach (String impacto in impactos)
                            {
                                String[] temp = impacto.Trim().Split(",");
                                Entidad tentidad = EntidadDAO.getEntidad(Convert.ToInt32(temp[0]), ejercicio);
                                ProyectoImpacto proyImpacto = new ProyectoImpacto();
                                proyImpacto.entidads = tentidad;
                                proyImpacto.entidadentidad = tentidad.entidad;
                                proyImpacto.proyectos = proyecto;
                                proyImpacto.proyectoid = proyecto.id;
                                proyImpacto.impacto = temp[1];
                                proyImpacto.usuarioCreo = User.Identity.Name;
                                proyImpacto.fechaCreacion = DateTime.Now;

                                result = ProyectoImpactoDAO.guardarProyectoImpacto(proyImpacto);
                            }
                        }
                    }
                    if (result)
                    {
                        String[] miembroIds = value.miembros != null && value.miembros.Length > 0 ? value.miembros.ToString().Split(",") : null;
                        if (miembroIds != null && miembroIds.Length > 0)
                        {
                            foreach (String miembroId in miembroIds)
                            {
                                Colaborador colaborador = new Colaborador();
                                colaborador.id = Convert.ToInt32(miembroId);
                                ProyectoMiembro proyMiembro = new ProyectoMiembro();
                                proyMiembro.proyectoid = proyecto.id;
                                proyMiembro.colaboradors = colaborador;
                                proyMiembro.colaboradorid = colaborador.id;
                                proyMiembro.estado = 1;
                                proyMiembro.usuarioCreo = User.Identity.Name;
                                proyMiembro.fechaCreacion = DateTime.Now;

                                result = ProyectoMiembroDAO.guardarProyectoMiembro(proyMiembro);
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = result ? true : false,
                        id = proyecto.id,
                        usuarioCreo = proyecto.usuarioCreo,
                        fechaCreacion = proyecto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioactualizo = proyecto.usuarioActualizo,
                        fechaactualizacion = proyecto.fechaActualizacion != null ? proyecto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize("Préstamos o Proyectos - Editar")]
        public IActionResult Proyecto(int id, [FromBody]dynamic value)
        {
            try
            {
                ProyectoValidator validator = new ProyectoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    bool result = false;
                    bool esnuevo = value.esnuevo == 1 ? true : false;
                    String nombre = value.nombre;
                    long snip = value.snip;
                    String objetivo = value.objetivo;
                    String descripcion = value.descripcion;
                    int ejercicio = value.ejercicio;

                    int programa = value.programa;
                    int subPrograma = value.subprograma;
                    int proyecto_ = value.proyecto_;
                    int actividad = value.actividad;
                    int obra = value.obra;
                    String longitud = value.longitud;
                    String latitud = value.latitud;
                    int renglon = value.renglon;
                    int ubicacionGeografica = value.ubicacionGeografica;
                    decimal costo = value.costo;
                    String objetivoEspecifico = value.objetoivoEspecifico;
                    String visionGeneral = value.visionGeneral;
                    int unidad_ejecutora = value.unidadejecutoraid;
                    int entidad = value.entidadid;
                    int ejecucionFisicaReal = value.ejecucionFisicaReal;
                    int proyectoClase = value.proyectoClase == null ? 1 : value.proyectoClase;
                    Etiqueta etiqueta = EtiquetaDAO.getEtiquetaPorId(proyectoClase);
                    int projectCargado = value.projectCargado == null ? 0 : value.projectCargado;
                    int? prestamoId = value.prestamoId;
                    String observaciones = value.observaciones;
                    int porcentajeAvance = value.porcentajeAvance;
                    Prestamo prestamo = null;
                    if (prestamoId != null)
                    {
                        prestamo = PrestamoDAO.getPrestamoById(prestamoId ?? default(int));
                    }

                    AcumulacionCosto acumulacionCosto = null;
                    if (value.acumulacionCosto != null)
                    {
                        acumulacionCosto = new AcumulacionCosto();
                        acumulacionCosto.id = value.acumulacionCosto;
                    }

                    String enunciadoAlcance = value.enunciadoAlcance;

                    ProyectoTipo proyectoTipo = new ProyectoTipo();
                    proyectoTipo.id = value.proyectotipoid;

                    UnidadEjecutora unidadEjecutora = UnidadEjecutoraDAO.getUnidadEjecutora(ejercicio, entidad, unidad_ejecutora);

                    Colaborador directorProyecto = null;
                    if (value.directorProyecto != null && value.directorProyecto.Length > 0)
                    {
                        directorProyecto = new Colaborador();
                        directorProyecto.id = value.directorProyecto;
                    }

                    Proyecto proyecto = ProyectoDAO.getProyectoPorId(id, User.Identity.Name);
                    proyecto.nombre = nombre;
                    proyecto.objetivo = objetivo;
                    proyecto.descripcion = descripcion;
                    proyecto.snip = Convert.ToInt32(snip);
                    proyecto.proyectoTipos = proyectoTipo;
                    proyecto.proyectoTipoid = proyectoTipo.id;
                    proyecto.unidadEjecutoras = unidadEjecutora;
                    proyecto.usuarioActualizo = User.Identity.Name;
                    proyecto.fechaActualizacion = DateTime.Now;
                    proyecto.programa = programa;
                    proyecto.subprograma = subPrograma;
                    proyecto.proyecto = proyecto_;
                    proyecto.actividad = actividad;
                    proyecto.obra = obra;
                    proyecto.longitud = longitud;
                    proyecto.latitud = latitud;
                    proyecto.directorProyecto = directorProyecto.id;
                    proyecto.enunciadoAlcance = enunciadoAlcance;
                    proyecto.costo = costo;
                    proyecto.acumulacionCostos = acumulacionCosto;
                    proyecto.acumulacionCostoid = acumulacionCosto.id;
                    proyecto.objetivoEspecifico = objetivoEspecifico;
                    proyecto.visionGeneral = visionGeneral;
                    proyecto.ejecucionFisicaReal = ejecucionFisicaReal;
                    proyecto.proyectoClase = etiqueta.id;
                    proyecto.projectCargado = projectCargado;
                    proyecto.prestamos = prestamo;
                    proyecto.prestamoid = prestamo.id;
                    proyecto.observaciones = observaciones;

                    List<ProyectoPropiedadValor> valores_temp = ProyectoPropiedadValorDAO.getProyectoPropiedadadesValoresPorProyecto(proyecto.id);

                    if (valores_temp != null)
                    {
                        foreach (ProyectoPropiedadValor valor in valores_temp)
                        {
                            valor.fechaActualizacion = DateTime.Now;
                            valor.usuarioActualizo = User.Identity.Name;
                            ProyectoPropiedadValorDAO.eliminarProyectoPropiedadValor(valor);
                        }
                    }

                    List<ProyectoImpacto> impactos_temp = ProyectoImpactoDAO.getProyectoImpactoPorProyecto(proyecto.id);
                    if (impactos_temp != null)
                    {
                        foreach (ProyectoImpacto pi in impactos_temp)
                            ProyectoImpactoDAO.eliminarTotalProyectoImpacto(pi);
                    }

                    List<ProyectoMiembro> miembros_temp = ProyectoMiembroDAO.getProyectoMiembroPorProyecto(proyecto.id);

                    if (miembros_temp != null)
                    {
                        foreach (ProyectoMiembro pm in miembros_temp)
                            ProyectoMiembroDAO.eliminarProyectoMiembro(pm);
                    }

                    result = ProyectoDAO.guardarProyecto(proyecto, false);

                    if (result && proyecto.coordinador != null && proyecto.coordinador.Equals(1))
                    {
                        if (!prestamo.porcentajeAvance.Equals(porcentajeAvance))
                        {
                            prestamo.porcentajeAvance = porcentajeAvance;
                            result = result && PrestamoDAO.guardarPrestamo(prestamo);
                        }
                    }
                    if (result)
                    {
                        List<stdatadinamico> datos = JsonConvert.DeserializeObject<stdatadinamico>(value.datadinamica);

                        foreach (stdatadinamico data in datos)
                        {
                            if (data.valor != null && data.valor.Length > 0 && data.valor.CompareTo("null") != 0)
                            {
                                ProyectoPropiedad proyectoPropiedad = ProyectoPropiedadDAO.getProyectoPropiedadPorId(Convert.ToInt32(data.id));
                                ProyectoPropiedadValor valor = new ProyectoPropiedadValor();
                                valor.proyectos = proyecto;
                                valor.proyectoid = proyecto.id;
                                valor.proyectoPropiedads = proyectoPropiedad;
                                valor.proyectoPropiedadid = proyectoPropiedad.id;
                                valor.usuarioCreo = User.Identity.Name;
                                valor.fechaCreacion = DateTime.Now;
                                valor.estado = 1;

                                switch (proyectoPropiedad.datoTipoid)
                                {
                                    case 1:
                                        valor.valorString = data.valor;
                                        break;
                                    case 2:
                                        valor.valorEntero = Convert.ToInt32(data.valor);
                                        break;
                                    case 3:
                                        valor.valorDecimal = Convert.ToDecimal(data.valor);
                                        break;
                                    case 4:
                                        valor.valorEntero = data.valor == "true" ? 1 : 0;
                                        break;
                                    case 5:
                                        valor.valorTiempo = Convert.ToDateTime(data.valor_f);
                                        break;
                                }
                                result = (result && ProyectoPropiedadValorDAO.guardarProyectoPropiedadValor(valor));
                            }
                        }
                    }
                    if (result)
                    {
                        String[] impactos = value.impactos != null && value.impactos.Length > 0 ? value.impactos.toString().Split("~") : null;
                        if (impactos != null && impactos.Length > 0)
                        {
                            foreach (String impacto in impactos)
                            {
                                String[] temp = impacto.Trim().Split(",");
                                Entidad tentidad = EntidadDAO.getEntidad(Convert.ToInt32(temp[0]), ejercicio);
                                ProyectoImpacto proyImpacto = new ProyectoImpacto();
                                proyImpacto.entidads = tentidad;
                                proyImpacto.entidadentidad = tentidad.entidad;
                                proyImpacto.proyectos = proyecto;
                                proyImpacto.proyectoid = proyecto.id;
                                proyImpacto.impacto = temp[1];
                                proyImpacto.usuarioCreo = User.Identity.Name;
                                proyImpacto.fechaCreacion = DateTime.Now;

                                result = ProyectoImpactoDAO.guardarProyectoImpacto(proyImpacto);
                            }
                        }
                    }
                    if (result)
                    {
                        String[] miembroIds = value.miembros != null && value.miembros.Length > 0 ? value.miembros.ToString().Split(",") : null;
                        if (miembroIds != null && miembroIds.Length > 0)
                        {
                            foreach (String miembroId in miembroIds)
                            {
                                Colaborador colaborador = new Colaborador();
                                colaborador.id = Convert.ToInt32(miembroId);
                                ProyectoMiembro proyMiembro = new ProyectoMiembro();
                                proyMiembro.proyectoid = proyecto.id;
                                proyMiembro.colaboradors = colaborador;
                                proyMiembro.colaboradorid = colaborador.id;
                                proyMiembro.estado = 1;
                                proyMiembro.usuarioCreo = User.Identity.Name;
                                proyMiembro.fechaCreacion = DateTime.Now;

                                result = ProyectoMiembroDAO.guardarProyectoMiembro(proyMiembro);
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = result ? true : false,
                        id = proyecto.id,
                        usuarioCreo = proyecto.usuarioCreo,
                        fechaCreacion = proyecto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioactualizo = proyecto.usuarioActualizo,
                        fechaactualizacion = proyecto.fechaActualizacion != null ? proyecto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("6", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{proyectoId}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult LineasBase(int proyectoId)
        {
            try
            {
                List<LineaBase> lstLineasBase = LineaBaseDAO.getLineasBaseById(proyectoId);
                List<stlineasbase> lstlineabase = new List<stlineasbase>();
                stlineasbase temp = new stlineasbase();
                temp.id = default(int);
                temp.nombre = "Actual";
                lstlineabase.Add(temp);

                foreach (LineaBase lineaBase in lstLineasBase)
                {
                    temp = new stlineasbase();
                    temp.id = lineaBase.id;
                    temp.nombre = lineaBase.nombre;
                    temp.proyectoid = lineaBase.proyectoid;
                    temp.usuarioCreo = lineaBase.usuarioCreo;
                    temp.usuarioActualizo = lineaBase.usuarioActualizo;
                    temp.fechaCreacion = lineaBase.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = lineaBase.fechaActualizacion != null ? lineaBase.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.sobreescribir = lineaBase.sobreescribir != null && lineaBase.sobreescribir == 1;
                    lstlineabase.Add(temp);
                }

                return Ok(new { success = true, lineasBase = lstlineabase });
            }
            catch (Exception e)
            {
                CLogger.write("7", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{proyectoId}/{tipoLineaBase}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult LineasBasePorTipo(int proyectoId, int tipoLineaBase)
        {
            try
            {
                List<LineaBase> lstLineasBase = LineaBaseDAO.getLineasBaseByIdProyectoTipo(proyectoId, tipoLineaBase);

                List<stlineasbase> lstlineabase = new List<stlineasbase>();

                stlineasbase temp = new stlineasbase();
                temp.id = default(int);
                temp.nombre = "Actual";
                lstlineabase.Add(temp);

                foreach (LineaBase lineaBase in lstLineasBase)
                {
                    temp = new stlineasbase();
                    temp.id = lineaBase.id;
                    temp.nombre = lineaBase.nombre;
                    temp.proyectoid = lineaBase.proyectoid;
                    temp.usuarioCreo = lineaBase.usuarioCreo;
                    temp.usuarioActualizo = lineaBase.usuarioActualizo;
                    temp.fechaCreacion = lineaBase.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = lineaBase.fechaActualizacion != null ? lineaBase.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.sobreescribir = lineaBase.sobreescribir != null && lineaBase.sobreescribir == 1;
                    lstlineabase.Add(temp);
                }

                return Ok(new { success = true, lineasBase = lstlineabase, anioActual = DateTime.Now.Year });
            }
            catch (Exception e)
            {
                CLogger.write("8", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Préstamos o Proyectos - Crear")]
        public IActionResult Modal([FromBody]dynamic value)
        {
            try
            {
                ModalValidator validator = new ModalValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Proyecto proyecto;

                    int ejercicio = value.ejercicio;
                    int unidad_ejecutora = value.unidadejecutoraid;
                    int entidad = value.entidadid;

                    ProyectoTipo proyectoTipo = new ProyectoTipo();
                    proyectoTipo.id = value.proyectotipoid;

                    UnidadEjecutora unidadEjecutora = UnidadEjecutoraDAO.getUnidadEjecutora(ejercicio, entidad, unidad_ejecutora);

                    proyecto = ProyectoDAO.getProyectoPorId(value.id, User.Identity.Name);
                    proyecto.nombre = value.nombre;
                    proyecto.proyectoTipos = proyectoTipo;
                    proyecto.proyectoTipoid = proyectoTipo.id;
                    proyecto.unidadEjecutoras = unidadEjecutora;
                    proyecto.ueunidadEjecutora = unidadEjecutora.unidadEjecutora;
                    proyecto.usuarioActualizo = User.Identity.Name;
                    proyecto.fechaActualizacion = DateTime.Now;

                    ProyectoDAO.guardarProyecto(proyecto, false);

                    datos temp = new datos();
                    temp.id = proyecto.id;
                    temp.nombre = proyecto.nombre;
                    temp.proyectotipoid = proyecto.proyectoTipoid;
                    temp.proyectotipo = proyecto.proyectoTipos.nombre;
                    temp.unidadejecutora = proyecto.unidadEjecutoras.nombre;
                    temp.unidadejecutoraid = proyecto.unidadEjecutoras.unidadEjecutora;
                    temp.entidadentidad = proyecto.unidadEjecutoras.entidadentidad;
                    temp.ejercicio = proyecto.unidadEjecutoras.ejercicio;

                    return Ok(new { success = true, proyecto = temp });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("9", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize("Préstamos o Proyectos - Eliminar")]
        public IActionResult ProyectoE(int id)
        {
            try
            {
                Proyecto proyecto = ProyectoDAO.getProyectoPorId(id, User.Identity.Name);
                List<ProyectoPropiedadValor> valores_temp = ProyectoPropiedadValorDAO.getProyectoPropiedadadesValoresPorProyecto(proyecto.id);
                if (valores_temp != null)
                {
                    foreach (ProyectoPropiedadValor valor in valores_temp)
                    {
                        valor.fechaActualizacion = DateTime.Now;
                        valor.usuarioActualizo = User.Identity.Name;
                        ProyectoPropiedadValorDAO.eliminarProyectoPropiedadValor(valor);
                    }
                }

                bool elimando = ObjetoDAO.borrarHijos(proyecto.treepath, 0, User.Identity.Name);

                return Ok(new { success = elimando });
            }
            catch (Exception e)
            {
                CLogger.write("10", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult NumeroProyectos([FromBody]dynamic value)
        {
            try
            {
                long total = ProyectoDAO.getTotalProyectos(value.filtro_nombre, value.filtro_usuario_creo, value.filtro_fecha_creacion,
                    User.Identity.Name, value.prestamoId);

                return Ok(new { success = true, totalproyectos = total });
            }
            catch (Exception e)
            {
                CLogger.write("11", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult NumeroProyectosDisponibles([FromBody]dynamic value)
        {
            try
            {
                long total = ProyectoDAO.getTotalProyectosDisponibles(value.filtro_nombre, value.filtro_usuario_creo, value.filtro_fecha_creacion, value.idsProyectos);
                return Ok(new { success = true, totalproyectos = true });
            }
            catch (Exception e)
            {
                CLogger.write("12", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ObtenerProyectoPorId(int id)
        {
            try
            {
                Proyecto proyecto = ProyectoDAO.getProyectoPorId(id, User.Identity.Name);

                if (proyecto != null)
                {
                    return Ok(new
                    {
                        success = true,
                        id = proyecto.id,
                        ejercicio = proyecto.ejercicio,
                        entidad = proyecto.unidadEjecutoras.entidadentidad,
                        entidadNombre = proyecto.unidadEjecutoras.entidads.nombre,
                        unidadEjecutora = proyecto.unidadEjecutoras.unidadEjecutora,
                        unidadEjecutoraNombre = proyecto.unidadEjecutoras.nombre,
                        prestamoId = proyecto.prestamoid,
                        congelado = proyecto.congelado,
                        nombre = proyecto.nombre
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("13", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ProyectoPorId(int id)
        {
            try
            {
                Proyecto proyecto = ProyectoDAO.getProyectoPorId(id, User.Identity.Name);

                datos dato = new datos();
                if (proyecto != null)
                {
                    dato.id = proyecto.id;
                    dato.nombre = proyecto.nombre;
                    dato.objetivo = proyecto.objetivo;
                    dato.descripcion = proyecto.descripcion;
                    dato.snip = Convert.ToInt64(proyecto.snip);

                    proyecto.proyectoTipos = ProyectoTipoDAO.getProyectoTipoPorId(proyecto.proyectoTipoid);
                    dato.proyectotipo = proyecto.proyectoTipos.nombre;
                    dato.proyectotipoid = proyecto.proyectoTipos.id;

                    proyecto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(proyecto.ejercicio, proyecto.entidad ?? default(int), proyecto.ueunidadEjecutora);
                    if (proyecto.unidadEjecutoras != null)
                    {
                        dato.unidadejecutora = proyecto.unidadEjecutoras.nombre;
                        dato.unidadejecutoraid = proyecto.unidadEjecutoras.unidadEjecutora;
                        dato.entidadentidad = proyecto.unidadEjecutoras.entidadentidad;

                        proyecto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(proyecto.unidadEjecutoras.entidadentidad, proyecto.unidadEjecutoras.ejercicio);

                        if (proyecto.unidadEjecutoras.entidads != null)
                            dato.entidadnombre = proyecto.unidadEjecutoras.entidads.nombre;

                        dato.ejercicio = proyecto.unidadEjecutoras.ejercicio;
                    }

                    dato.fechaCreacion = proyecto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    dato.usuarioCreo = proyecto.usuarioCreo;
                    dato.fechaactualizacion = proyecto.fechaActualizacion != null ? proyecto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.usuarioactualizo = proyecto.usuarioActualizo;
                    dato.programa = proyecto.programa;
                    dato.subprograma = proyecto.subprograma;
                    dato.proyecto = proyecto.proyecto;
                    dato.obra = proyecto.obra;
                    dato.actividad = proyecto.actividad;
                    dato.renglon = proyecto.renglon;
                    dato.ubicacionGeografica = proyecto.ubicacionGeografica;
                    dato.longitud = proyecto.longitud;
                    dato.latitud = proyecto.latitud;

                    proyecto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(proyecto.acumulacionCostoid));
                    dato.acumulacionCosto = proyecto.acumulacionCostos != null ? Convert.ToInt32(proyecto.acumulacionCostos.id) : default(int);
                    dato.acumulacionCostoNombre = proyecto.acumulacionCostos != null ? proyecto.acumulacionCostos.nombre : null;
                    dato.objetivoEspecifico = proyecto.objetivoEspecifico;
                    dato.visionGeneral = proyecto.visionGeneral;

                    dato.directorProyectoId = proyecto.directorProyecto ?? default(int);

                    Colaborador colaborador = ColaboradorDAO.getColaborador(proyecto.directorProyecto ?? default(int));

                    dato.directorProyectoNmbre = colaborador != null ? (colaborador.pnombre
                                            + " " + colaborador.snombre
                                            + " " + colaborador.papellido
                                            + " " + colaborador.sapellido) : null;

                    dato.ejecucionFisicaReal = proyecto.ejecucionFisicaReal ?? default(int);


                    Etiqueta etiqueta = EtiquetaDAO.getEtiquetaPorId(proyecto.proyectoClase);
                    dato.proyectoClase = etiqueta.id;

                    dato.projectCargado = proyecto.projectCargado ?? default(int);
                    dato.prestamoId = proyecto.prestamoid ?? default(int);
                    dato.costo = proyecto.costo ?? default(decimal);
                    dato.fechaInicio = proyecto.fechaInicio != null ? proyecto.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.fechaFin = proyecto.fechaFin != null ? proyecto.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.observaciones = proyecto.observaciones;
                    dato.fechaInicioReal = proyecto.fechaInicioReal != null ? proyecto.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.fechaFinReal = proyecto.fechaFinReal != null ? proyecto.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    dato.congelado = proyecto.congelado ?? default(int);
                    dato.coordinador = proyecto.coordinador ?? default(int);
                }

                return Ok(new { success = true, proyecto = dato });
            }
            catch (Exception e)
            {
                CLogger.write("13", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ControlArbol(int id)
        {
            try
            {
                Nodo arbol = EstructuraProyectoDAO.getEstructuraProyectoArbol(id, null, User.Identity.Name);
                Nodo root = new Nodo();
                root.id = 0;
                root.objeto_tipo = 0;
                root.nombre = "";
                root.nivel = 0;
                root.children = new List<Nodo>();
                root.parent = null;
                root.estado = false;

                arbol.parent = root;
                root.children.Add(arbol);

                return Ok(new { success = true, proyecto = root });
            }
            catch (Exception e)
            {
                CLogger.write("14", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{usuario}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ControlArbolTodosProyectos(string usuario)
        {
            try
            {
                List<Nodo> proyectos = EstructuraProyectoDAO.getEstructuraPrestamosArbol(usuario, null);
                Nodo root = new Nodo();
                root.id = 0;
                root.objeto_tipo = 0;
                root.nombre = "";
                root.nivel = 0;
                root.children = new List<Nodo>();
                root.parent = null;
                root.estado = false;

                if (proyectos != null)
                {
                    for (int i = 0; i < proyectos.Count; i++)
                    {
                        proyectos[i].parent = root;
                    }
                    root.children = proyectos;
                }

                return Ok(new { success = true, proyectos = root });
            }
            catch (Exception e)
            {
                CLogger.write("15", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{proyectoId}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult CalcularCostoFecha(int proyectoId)
        {
            try
            {
                bool calculado = ProyectoDAO.calcularCostoyFechas(proyectoId);
                return Ok(new { success = calculado });
            }
            catch (Exception e)
            {
                CLogger.write("16", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{proyectoId}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult Matriz(int proyectoId)
        {
            try
            {
                List<Componente> componentes = ComponenteDAO.getComponentesPorProyecto(proyectoId);
                List<stcomponentessigade> stcomponentes = new List<stcomponentessigade>();
                stunidadejecutora stunidad = new stunidadejecutora();
                List<stunidadejecutora> unidadesEjecutroas = new List<stunidadejecutora>();


                foreach (Componente componente in componentes)
                {
                    stcomponentessigade temp = new stcomponentessigade();
                    temp.id = componente.id;
                    temp.nombre = componente.nombre;
                    temp.techo = componente.componenteSigades != null ? componente.componenteSigades.montoComponente : default(decimal);
                    stcomponentes.Add(temp);

                    List<stunidadejecutora> stunidades = new List<stunidadejecutora>();
                    stunidad = new stunidadejecutora();
                    stunidad.donacion = componente.fuenteDonacion ?? default(decimal);
                    stunidad.ejercicio = componente.unidadEjecutoras != null ? componente.unidadEjecutoras.ejercicio : default(int);
                    stunidad.nacional = componente.fuenteNacional ?? default(decimal);
                    stunidad.prestamo = componente.fuentePrestamo ?? default(decimal);
                    stunidad.id = componente.unidadEjecutoras != null ? componente.unidadEjecutoras.unidadEjecutora : default(int);
                    stunidad.nombre = componente.unidadEjecutoras != null ? componente.unidadEjecutoras.nombre : null;
                    stunidades.Add(stunidad);
                    temp.unidadesEjecutoras = stunidades;
                    if (unidadesEjecutroas.Count == 0)
                        unidadesEjecutroas.Add(stunidad);
                }

                return Ok(new
                {
                    success = true,
                    unidadesEjecutoras = unidadesEjecutroas,
                    componentes = stcomponentes
                });
            }
            catch (Exception e)
            {
                CLogger.write("16", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult MontoTechos(int id)
        {
            try
            {
                decimal techoTotal = decimal.Zero;
                Proyecto proyecto = ProyectoDAO.getProyecto(id);
                if (proyecto.prestamos != null)
                {
                    List<Componente> componentes = ComponenteDAO.getComponentesPorProyecto(proyecto.id);
                    foreach (Componente componente in componentes)
                    {
                        techoTotal += componente.fuentePrestamo != null && componente.fuenteDonacion != null && componente.fuenteNacional != null ?
                                    componente.fuentePrestamo ?? default(decimal) + componente.fuenteNacional ?? default(decimal) +
                                    componente.fuenteDonacion ?? default(decimal) : techoTotal;
                    }
                }

                return Ok(new { success = true, techoPep = techoTotal });
            }
            catch (Exception e)
            {
                CLogger.write("17", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult PepDetalle(int id)
        {
            try
            {
                PepDetalle detalle = ProyectoDAO.getPepDetalle(id);
                stpepdetalle pepdetalle = null;
                if (detalle != null)
                {
                    pepdetalle = new stpepdetalle();
                    pepdetalle.proyectoid = id;
                    pepdetalle.observaciones = detalle.observaciones;
                    pepdetalle.alertivos = detalle.alertivos;
                    pepdetalle.elaborado = detalle.elaborado;
                    pepdetalle.aprobado = detalle.aprobado;
                    pepdetalle.autoridad = detalle.autoridad;
                }

                return Ok(new { success = true, detalle = pepdetalle });
            }
            catch (Exception e)
            {
                CLogger.write("18", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Préstamos o Proyectos - Crear")]
        public IActionResult PepDetalle([FromBody]dynamic value)
        {
            try
            {
                PepDetalleValidator validators = new PepDetalleValidator();
                ValidationResult results = validators.Validate(value);

                if (results.IsValid)
                {
                    Proyecto proyecto = ProyectoDAO.getProyecto(value.proyectoid);
                    PepDetalle detalle = ProyectoDAO.getPepDetalle(value.proyectoid);

                    if (detalle == null)
                    {
                        detalle = new PepDetalle();
                        detalle.usuarioCreo = User.Identity.Name;
                        detalle.fechaCreacion = DateTime.Now;
                        detalle.estado = 1;
                    }
                    else
                    {
                        detalle.fechaActualizacion = DateTime.Now;
                        detalle.usuarioActualizo = User.Identity.Name;
                    }

                    detalle.observaciones = value.observaciones;
                    detalle.alertivos = value.alertivos;
                    detalle.elaborado = value.elaborado;
                    detalle.aprobado = value.aprobado;
                    detalle.autoridad = value.autoridad;

                    bool resultado = ProyectoDAO.guardarPepDetalle(detalle);

                    return Ok(new { success = resultado });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("19", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Préstamos o Proyectos - Crear")]
        public IActionResult Congelar([FromBody]dynamic value)
        {
            try
            {
                LineaBaseValidator validator = new LineaBaseValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    bool ret = false;
                    int pepId = value.id ?? default(int);
                    String nombre = value.nombre;
                    Proyecto proyecto = ProyectoDAO.getProyecto(pepId);
                    int nuevaLinaBase = value.nuevo;
                    int lineaBaseId = value.lineaBaseId;
                    int tipoLinea = value.tipoLineaBase != null ? value.tipoLineaBase : 1;
                    String mes = value.mes;
                    String anio = value.anio;
                    String lineaBaseEditar = null;
                    LineaBase lineaTemp = null;

                    switch (tipoLinea)
                    {
                        case 1:
                            if (nuevaLinaBase.Equals(2) && lineaBaseId > 0)
                            {
                                lineaTemp = LineaBaseDAO.getLineaBasePorId(lineaBaseId);
                                nombre = lineaTemp != null ? lineaTemp.nombre : "";
                                lineaBaseEditar = lineaTemp != null ? "|lb" + lineaTemp.id + "|" : null;
                            }

                            LineaBase lineaBase = new LineaBase();
                            lineaBase.proyectos = proyecto;
                            lineaBase.proyectoid = proyecto.id;
                            lineaBase.nombre = nombre;
                            lineaBase.usuarioCreo = User.Identity.Name;
                            lineaBase.fechaCreacion = DateTime.Now;
                            lineaBase.tipoLineaBase = tipoLinea;
                            lineaBase.sobreescribir = 1;

                            if (lineaTemp != null)
                                ret = LineaBaseDAO.eliminarTotalLineaBase(lineaTemp);
                            ret = LineaBaseDAO.guardarLineaBase(lineaBase, lineaBaseEditar);
                            return Ok(new { success = ret });
                        case 2:
                            proyecto.congelado = 1;
                            ret = ProyectoDAO.guardarProyecto(proyecto, false);
                            nombre = mes + "_" + anio;
                            lineaTemp = LineaBaseDAO.getLineasBaseByNombre(proyecto.id, nombre);
                            lineaBaseEditar = lineaTemp != null ? "|lb" + lineaTemp.id + "|" : null;

                            lineaBase = new LineaBase();
                            lineaBase.proyectos = proyecto;
                            lineaBase.proyectoid = proyecto.id;
                            lineaBase.nombre = nombre;
                            lineaBase.usuarioActualizo = User.Identity.Name;
                            lineaBase.fechaActualizacion = DateTime.Now;
                            lineaBase.tipoLineaBase = tipoLinea;
                            lineaBase.sobreescribir = 0;

                            if (lineaTemp != null)
                            {
                                if (lineaTemp.sobreescribir != null && lineaTemp.sobreescribir == 1)
                                {
                                    ret = LineaBaseDAO.eliminarTotalLineaBase(lineaTemp);

                                    ret = LineaBaseDAO.guardarLineaBase(lineaBase, lineaBaseEditar);
                                    return Ok(new { success = ret });
                                }

                                return Ok(new { success = false, mensaje = "No tiene permiso para editar el congelamiento" });
                            }
                            else
                            {
                                ret = LineaBaseDAO.guardarLineaBase(lineaBase, lineaBaseEditar);
                                return Ok(new { success = ret });
                            }
                    }

                    return Ok(new { success = false });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("20", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult CantidadHistoria(int id)
        {
            try
            {
                String resultado = ProyectoDAO.getVersiones(id);
                return Ok(new { success = true, versiones = "[" + resultado + "]" });
            }
            catch (Exception e)
            {
                CLogger.write("21", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}/{version}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult Historia(int id, int version)
        {
            try
            {
                String resultado = ProyectoDAO.getHistoria(id, version);
                return Ok(new { success = true, historia = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("22", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{prestamoid}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ProyectosLineaBase(int? prestamoid)
        {
            try
            {
                List<Proyecto> proyectos = (prestamoid != null) ? ProyectoDAO.getProyectos(prestamoid ?? default(int), User.Identity.Name) :
                    ProyectoDAO.getProyectos(User.Identity.Name);

                List<datos> datos_ = new List<datos>();
                foreach (Proyecto proyecto in proyectos)
                {
                    datos dato = new datos();
                    dato.id = proyecto.id;
                    dato.nombre = proyecto.nombre;
                    dato.congelado = proyecto.congelado ?? default(int);
                    LineaBase lineaBase = LineaBaseDAO.getUltimaLinaBasePorProyecto(dato.id, 2);
                    dato.permisoEditarCongelar = lineaBase != null && lineaBase.sobreescribir != null
                            && lineaBase.sobreescribir.Equals(1) && dato.congelado.Equals(1);
                    dato.lineaBaseId = lineaBase.id;
                    datos_.Add(dato);
                }

                return Ok(new { success = true, entidades = datos_ });
            }
            catch (Exception e)
            {
                CLogger.write("23", "ProyectoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
