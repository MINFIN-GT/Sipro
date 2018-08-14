using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;
using FluentValidation.Results;
using Newtonsoft.Json.Linq;

namespace SComponente.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ComponenteController : Controller
    {
        private class Stcomponente
        {
            public int id;
            public String nombre;
            public String descripcion;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int componenteTipoid;
            public String componentetiponombre;
            public int estado;
            public Int64? snip;
            public int? programa;
            public int? subprograma;
            public int? proyecto;
            public int? obra;
            public int? actividad;
            public int? renglon;
            public int? ubicacionGeografica;
            public int duracion;
            public String duracionDimension;
            public String fechaInicio;
            public String fechaFin;
            public int ueunidadEjecutora;
            public int ejercicio;
            public int entidad;
            public String unidadejecutoranombre;
            public String entidadnombre;
            public String latitud;
            public String longitud;
            public decimal costo;
            public int acumulacionCostoid;
            public String acumulacionCostoNombre;
            public decimal fuentePrestamo;
            public decimal fuenteDonacion;
            public decimal fuenteNacional;
            public bool tieneHijos;
            public int esDeSigade;
            public int prestamoId;
            public String fechaInicioReal;
            public String fechaFinReal;
            public int inversionNueva;
            public int proyectoid;
            public int orden;
            public string treepath;
            public int nivel;
            public int componenteSigadeid;
        }

        private class Stdatadinamico
        {
            public String id;
            public String tipo;
            public String label;
            public String valor;
            public String valor_f;
        }

        [HttpPost]
        [Authorize("Componentes - Visualizar")]
        public IActionResult NumeroComponentesPorProyecto([FromBody]dynamic value)
        {
            try
            {
                string filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                int proyectoId = value.proyectoId != null ? (int)value.proyectoId : default(int);
                long total = ComponenteDAO.getTotalComponentesPorProyecto(proyectoId, filtro_busqueda, User.Identity.Name);
                return Ok(new { success = true, totalcomponentes = total });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Componentes - Visualizar")]
        public IActionResult ComponentesPaginaPorProyecto([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int numeroComponentes = value.numeroCooperantes != null ? (int)value.numeroCooperantes : 20;
                string filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                int proyectoId = value.proyectoId != null ? (int)value.proyectoId : default(int);
                string columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : null;
                string orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : null;
                bool esDeSigade = false;

                List<Componente> componentes = ComponenteDAO.getComponentesPaginaPorProyecto(pagina, numeroComponentes, proyectoId
                    , filtro_busqueda, columna_ordenada, orden_direccion, User.Identity.Name);

                List<Stcomponente> stcomponentes = new List<Stcomponente>();
                foreach (Componente componente in componentes)
                {
                    Stcomponente temp = new Stcomponente();
                    temp.descripcion = componente.descripcion;
                    temp.estado = componente.estado;
                    temp.fechaActualizacion = componente.fechaActualizacion != null ? componente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = componente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.id = componente.id;
                    temp.nombre = componente.nombre;
                    temp.usuarioActualizo = componente.usuarioActualizo;
                    temp.usuarioCreo = componente.usuarioCreo;
                    temp.componenteTipoid = componente.componenteTipoid;
                    componente.componenteTipos = ComponenteTipoDAO.getComponenteTipoPorId(componente.componenteTipoid);

                    temp.componentetiponombre = componente.componenteTipos.nombre;
                    temp.snip = componente.snip ?? null;
                    temp.programa = componente.programa ?? null;
                    temp.subprograma = componente.subprograma ?? null;
                    temp.proyecto = componente.proyecto ?? null;
                    temp.obra = componente.obra ?? null;
                    temp.renglon = componente.renglon ?? null;
                    temp.ubicacionGeografica = componente.ubicacionGeografica ?? null;
                    temp.actividad = componente.actividad ?? null;

                    componente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(componente.ejercicio, componente.entidad ?? default(int), componente.ueunidadEjecutora);

                    if (componente.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = componente.ueunidadEjecutora;
                        temp.ejercicio = componente.ejercicio;
                        temp.entidad = componente.entidad ?? default(int);
                        componente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(componente.entidad ?? default(int), componente.ejercicio);
                        temp.unidadejecutoranombre = componente.unidadEjecutoras.nombre;
                        temp.entidadnombre = componente.unidadEjecutoras.entidads.nombre;
                    }
                    else
                    {
                        Proyecto proyecto = ProyectoDAO.getProyecto(componente.proyectoid);
                        proyecto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(proyecto.ejercicio, proyecto.entidad ?? default(int), proyecto.ueunidadEjecutora);

                        temp.ueunidadEjecutora = proyecto.ueunidadEjecutora;
                        temp.ejercicio = proyecto.ejercicio;
                        temp.entidad = proyecto.entidad ?? default(int);

                        proyecto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(proyecto.entidad ?? default(int), proyecto.ejercicio);
                        temp.unidadejecutoranombre = proyecto.unidadEjecutoras.nombre;
                        temp.entidadnombre = proyecto.unidadEjecutoras.entidads.nombre;
                    }

                    temp.latitud = componente.latitud;
                    temp.longitud = componente.longitud;
                    temp.costo = componente.costo ?? default(decimal);
                    temp.acumulacionCostoid = (int)componente.acumulacionCostoid;

                    componente.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById((int)componente.acumulacionCostoid);

                    temp.acumulacionCostoNombre = componente.acumulacionCostos.nombre;
                    temp.fechaInicio = componente.fechaInicio != null ? componente.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFin = componente.fechaFin != null ? componente.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.duracion = componente.duracion;
                    temp.duracionDimension = componente.duracionDimension;

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 1);
                    temp.esDeSigade = componente.esDeSigade ?? default(int);
                    temp.fuentePrestamo = componente.fuentePrestamo ?? default(decimal);
                    temp.fuenteDonacion = componente.fuenteDonacion ?? default(decimal);
                    temp.fuenteNacional = componente.fuenteNacional ?? default(decimal);

                    esDeSigade = temp.esDeSigade==1 ? true : false;

                    temp.fechaInicioReal = componente.fechaInicioReal != null ? componente.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFinReal = componente.fechaFinReal != null ? componente.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.inversionNueva = componente.inversionNueva;
                    temp.proyectoid = componente.proyectoid;

                    stcomponentes.Add(temp);
                }

                return Ok(new { success = true, componentes = stcomponentes, esDeSigade = esDeSigade });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult Componentes()
        {
            try
            {
                List<Componente> componentes = ComponenteDAO.getComponentes(User.Identity.Name);
                List<Stcomponente> stcomponentes = new List<Stcomponente>();
                bool esDeSigade = false;
                foreach (Componente componente in componentes)
                {
                    Stcomponente temp = new Stcomponente();
                    temp.descripcion = componente.descripcion;
                    temp.estado = componente.estado;
                    temp.fechaActualizacion = componente.fechaActualizacion != null ? componente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = componente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.id = componente.id;
                    temp.nombre = componente.nombre;
                    temp.usuarioActualizo = componente.usuarioActualizo;
                    temp.usuarioCreo = componente.usuarioCreo;
                    temp.componenteTipoid = componente.componenteTipoid;
                    componente.componenteTipos = ComponenteTipoDAO.getComponenteTipoPorId(componente.componenteTipoid);

                    temp.componentetiponombre = componente.componenteTipos.nombre;
                    temp.snip = componente.snip != null ? (long)componente.snip : default(long);
                    temp.programa = componente.programa ?? default(int);
                    temp.subprograma = componente.subprograma ?? default(int);
                    temp.proyecto = componente.proyecto ?? default(int);
                    temp.obra = componente.obra ?? default(int);
                    temp.renglon = componente.renglon ?? default(int);
                    temp.ubicacionGeografica = componente.ubicacionGeografica ?? default(int);
                    temp.actividad = componente.actividad ?? default(int);

                    componente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(componente.ejercicio, componente.entidad ?? default(int), componente.ueunidadEjecutora);

                    if (componente.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = componente.ueunidadEjecutora;
                        temp.ejercicio = componente.ejercicio;
                        temp.entidad = componente.entidad ?? default(int);
                        componente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(componente.entidad ?? default(int), componente.ejercicio);
                        temp.unidadejecutoranombre = componente.unidadEjecutoras.nombre;
                        temp.entidadnombre = componente.unidadEjecutoras.entidads.nombre;
                    }
                    else
                    {
                        Proyecto proyecto = ProyectoDAO.getProyecto(componente.proyectoid);
                        proyecto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(proyecto.ejercicio, proyecto.entidad ?? default(int), proyecto.ueunidadEjecutora);

                        temp.ueunidadEjecutora = proyecto.ueunidadEjecutora;
                        temp.ejercicio = proyecto.ejercicio;
                        temp.entidad = proyecto.entidad ?? default(int);

                        proyecto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(proyecto.entidad ?? default(int), proyecto.ejercicio);
                        temp.unidadejecutoranombre = proyecto.unidadEjecutoras.nombre;
                        temp.entidadnombre = proyecto.unidadEjecutoras.entidads.nombre;
                    }

                    temp.latitud = componente.latitud;
                    temp.longitud = componente.longitud;
                    temp.costo = componente.costo ?? default(decimal);
                    temp.acumulacionCostoid = (int)componente.acumulacionCostoid;

                    componente.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById((int)componente.acumulacionCostoid);

                    temp.acumulacionCostoNombre = componente.acumulacionCostos.nombre;
                    temp.fechaInicio = componente.fechaInicio != null ? componente.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFin = componente.fechaFin != null ? componente.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.duracion = componente.duracion;
                    temp.duracionDimension = componente.duracionDimension;

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 1);
                    temp.esDeSigade = componente.esDeSigade ?? default(int);
                    temp.fuentePrestamo = componente.fuentePrestamo ?? default(decimal);
                    temp.fuenteDonacion = componente.fuenteDonacion ?? default(decimal);
                    temp.fuenteNacional = componente.fuenteNacional ?? default(decimal);

                    esDeSigade = temp.esDeSigade==1 ? true : false;

                    temp.fechaInicioReal = componente.fechaInicioReal != null ? componente.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFinReal = componente.fechaFinReal != null ? componente.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.inversionNueva = componente.inversionNueva;
                    temp.proyectoid = componente.proyectoid;

                    stcomponentes.Add(temp);
                }
                return Ok(new { success = true, componentes = stcomponentes });
            }
            catch (Exception e)
            {
                CLogger.write("3", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Componentes - Visualizar")]
        public IActionResult ComponentePorId(int id)
        {
            try
            {
                Componente componente = ComponenteDAO.getComponentePorId(id, User.Identity.Name);
                int congelado = 0;
                int prestamoid = 0;
                if (componente != null)
                {
                    Proyecto proyecto = ProyectoDAO.getProyectobyTreePath(componente.treepath);
                    congelado = proyecto.congelado ?? default(int);
                    prestamoid = proyecto.prestamoid ?? default(int);
                }

                componente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(componente.ejercicio, componente.entidad ?? default(int), componente.ueunidadEjecutora);

                if (componente.unidadEjecutoras != null)
                {
                    componente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(componente.entidad ?? default(int), componente.ejercicio);
                }

                return Ok(new {
                    id = componente.id,
                    ejercicio = componente.ejercicio,
                    entidad = componente.entidad,
                    entidadNombre = componente.unidadEjecutoras.entidads.nombre,
                    unidadEjecutora = componente.ueunidadEjecutora,
                    unidadEjecutoraNombre = componente.unidadEjecutoras.nombre,
                    prestamoId = prestamoid,
                    fechaInicio = componente.fechaInicio != null ? componente.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null,
                    congelado = congelado,
                    nombre = componente.nombre,
                    success = true
                });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Componentes - Visualizar")]
        public IActionResult ComponentePorIdCorto(int id)
        {
            try
            {
                bool esDeSigade = false;
                Componente componente = ComponenteDAO.getComponentePorId(id, User.Identity.Name);
                Stcomponente temp = new Stcomponente();
                temp.descripcion = componente.descripcion;
                temp.estado = componente.estado;
                temp.fechaActualizacion = componente.fechaActualizacion != null ? componente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.fechaCreacion = componente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                temp.id = componente.id;
                temp.nombre = componente.nombre;
                temp.usuarioActualizo = componente.usuarioActualizo;
                temp.usuarioCreo = componente.usuarioCreo;
                temp.componenteTipoid = componente.componenteTipoid;
                componente.componenteTipos = ComponenteTipoDAO.getComponenteTipoPorId(componente.componenteTipoid);

                temp.componentetiponombre = componente.componenteTipos.nombre;
                temp.snip = componente.snip != null ? (long)componente.snip : default(long);
                temp.programa = componente.programa ?? default(int);
                temp.subprograma = componente.subprograma ?? default(int);
                temp.proyecto = componente.proyecto ?? default(int);
                temp.obra = componente.obra ?? default(int);
                temp.renglon = componente.renglon ?? default(int);
                temp.ubicacionGeografica = componente.ubicacionGeografica ?? default(int);
                temp.actividad = componente.actividad ?? default(int);

                componente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(componente.ejercicio, componente.entidad ?? default(int), componente.ueunidadEjecutora);

                if (componente.unidadEjecutoras != null)
                {
                    temp.ueunidadEjecutora = componente.ueunidadEjecutora;
                    temp.ejercicio = componente.ejercicio;
                    temp.entidad = componente.entidad ?? default(int);
                    componente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(componente.entidad ?? default(int), componente.ejercicio);
                    temp.unidadejecutoranombre = componente.unidadEjecutoras.nombre;
                    temp.entidadnombre = componente.unidadEjecutoras.entidads.nombre;
                }
                else
                {
                    Proyecto proyecto = ProyectoDAO.getProyecto(componente.proyectoid);
                    proyecto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(proyecto.ejercicio, proyecto.entidad ?? default(int), proyecto.ueunidadEjecutora);

                    temp.ueunidadEjecutora = proyecto.ueunidadEjecutora;
                    temp.ejercicio = proyecto.ejercicio;
                    temp.entidad = proyecto.entidad ?? default(int);

                    proyecto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(proyecto.entidad ?? default(int), proyecto.ejercicio);
                    temp.unidadejecutoranombre = proyecto.unidadEjecutoras.nombre;
                    temp.entidadnombre = proyecto.unidadEjecutoras.entidads.nombre;
                }

                temp.latitud = componente.latitud;
                temp.longitud = componente.longitud;
                temp.costo = componente.costo ?? default(decimal);
                temp.acumulacionCostoid = (int)componente.acumulacionCostoid;

                componente.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById((int)componente.acumulacionCostoid);

                temp.acumulacionCostoNombre = componente.acumulacionCostos.nombre;
                temp.fechaInicio = componente.fechaInicio != null ? componente.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.fechaFin = componente.fechaFin != null ? componente.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.duracion = componente.duracion;
                temp.duracionDimension = componente.duracionDimension;

                temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 1);
                temp.esDeSigade = componente.esDeSigade ?? default(int);
                temp.fuentePrestamo = componente.fuentePrestamo ?? default(decimal);
                temp.fuenteDonacion = componente.fuenteDonacion ?? default(decimal);
                temp.fuenteNacional = componente.fuenteNacional ?? default(decimal);

                esDeSigade = temp.esDeSigade==1 ? true : false;

                temp.fechaInicioReal = componente.fechaInicioReal != null ? componente.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.fechaFinReal = componente.fechaFinReal != null ? componente.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.inversionNueva = componente.inversionNueva;
                temp.proyectoid = componente.proyectoid;

                return Ok(new { success = true, componente = temp });
            }
            catch (Exception e)
            {
                CLogger.write("5", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Componentes - Eliminar")]
        public IActionResult Componente(int id)
        {
            try
            {
                Componente componente = ComponenteDAO.getComponente(id);
                bool eliminado = ObjetoDAO.borrarHijos(componente.treepath, 1, User.Identity.Name);
                return Ok(new { success = eliminado});
            }
            catch (Exception e)
            {
                CLogger.write("6", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Componentes - Visualizar")]
        public IActionResult CantidadHistoria(int id)
        {
            try
            {
                String resultado = ComponenteDAO.getVersiones(id);
                return Ok(new { success = true, versiones = "[" + resultado + "]" });
            }
            catch (Exception e)
            {
                CLogger.write("7", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}/{version}")]
        [Authorize("Componentes - Visualizar")]
        public IActionResult Historia(int id, int version)
        {
            try
            {
                String resultado = ComponenteDAO.getHistoria(id, version);
                return Ok(new { success = true, historia = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("8", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Componentes - Visualizar")]
        public IActionResult ValidacionAsignado([FromBody]dynamic value)
        {
            try
            {
                DateTime cal = new DateTime();
                int ejercicio = cal.Year;
                Componente objComponente = ComponenteDAO.getComponente((int)value.id);
                Proyecto objProyecto = ProyectoDAO.getProyectobyTreePath(objComponente.treepath);
                int entidad = objProyecto.entidad ?? default(int);

                int programa = value.programa != null ? (int)value.programa : default(int);
                int subprograma = value.subprograma != null ? (int)value.subprograma : default(int);
                int proyecto = value.proyecto != null ? (int)value.proyecto : default(int);
                int actividad = value.actividad != null ? (int)value.actividad : default(int);
                int obra = value.obra != null ? (int)value.obra : default(int);
                int renglon = value.renglon != null ? (int)value.renglon : default(int);
                int geografico = value.geografico != null ? (int)value.geografico : default(int);
                decimal asignado = ObjetoDAO.getAsignadoPorLineaPresupuestaria(ejercicio, entidad, programa, subprograma, proyecto, actividad, obra, renglon, geografico);

                decimal planificado = decimal.Zero;
                switch (objComponente.acumulacionCostoid)
                {
                    case 1:
                        cal = objComponente.fechaInicio ?? default(DateTime);
                        int ejercicioInicial = cal.Year;
                        if (ejercicio.Equals(ejercicioInicial))
                        {
                            planificado = objComponente.costo ?? default(decimal);
                        }
                        break;
                    case 2:
                        List<PagoPlanificado> lstPagos = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(objComponente.id, 1);
                        foreach (PagoPlanificado pago in lstPagos)
                        {
                            cal = pago.fechaPago;
                            int ejercicioPago = cal.Year;
                            if (ejercicio.Equals(ejercicioPago))
                            {
                                planificado += pago.pago;
                            }
                        }
                        break;
                    case 3:
                        cal = objComponente.fechaFin ?? default(DateTime);
                        int ejercicioFinal = cal.Year;
                        if (ejercicio.Equals(ejercicioFinal))
                        {
                            planificado += objComponente.costo ?? default(decimal);
                        }
                        break;
                }

                bool sobrepaso = false;
                if ((asignado = (asignado - planificado)).CompareTo(decimal.Zero) == -1)
                    sobrepaso = true;

                return Ok(new { success = true, asignado = asignado, sobrepaso = sobrepaso});
            }
            catch (Exception e)
            {
                CLogger.write("9", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Componentes - Crear")]
        public IActionResult Componente([FromBody]dynamic value)
        {
            try
            {
                bool result = false;
                ComponenteValidator validator = new ComponenteValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Componente componente = new Componente();
                    componente.nombre = value.nombre;
                    componente.descripcion = value.descripcion;
                    componente.componenteTipoid = value.componenteTipoid;
                    componente.proyectoid = value.proyectoid;
                    componente.ueunidadEjecutora = value.ueunidadEjecutora;
                    componente.ejercicio = value.ejercicio;
                    componente.entidad = value.entidad;
                    componente.snip = value.snip;
                    componente.programa = value.programa;
                    componente.subprograma = value.subprograma;
                    componente.proyecto = value.proyecto;
                    componente.actividad = value.actividad;
                    componente.obra = value.obra;
                    componente.renglon = value.renglon;
                    componente.ubicacionGeografica = value.ubicacionGeografica;
                    componente.latitud = value.latitud;
                    componente.longitud = value.longitud;
                    componente.costo = value.costo;
                    componente.acumulacionCostoid = value.acumulacionCostoid;
                    componente.fechaInicio = value.fechaInicio;
                    componente.fechaFin = Convert.ToDateTime(value.fechaFin);
                    componente.duracion = value.duracion;
                    componente.duracionDimension = value.duracionDimension;
                    componente.esDeSigade = value.esDeSigade;
                    componente.inversionNueva = value.inversionNueva;
                    componente.fechaCreacion = DateTime.Now;
                    componente.usuarioCreo = User.Identity.Name;
                    componente.estado = 1;
                    componente.fuentePrestamo = value.fuentePrestamo;
                    componente.fuenteDonacion = value.fuenteDonacion;
                    componente.fuenteNacional = value.fuenteNacional;
                    componente.fechaInicioReal = value.fechaInicioReal;
                    componente.fechaFinReal = value.fechaFinReal;

                    result = ComponenteDAO.guardarComponente(componente, true);

                    if (result)
                    {
                        String pagosPlanificados = value.pagosPlanificados;
                        if (!componente.acumulacionCostoid.Equals(2) || pagosPlanificados != null && pagosPlanificados.Length > 0)
                        {
                            List<PagoPlanificado> pagosActuales = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(componente.id, 1);
                            foreach (PagoPlanificado pagoTemp in pagosActuales)
                            {
                                PagoPlanificadoDAO.eliminarTotalPagoPlanificado(pagoTemp);
                            }
                        }

                        if (componente.acumulacionCostoid.Equals(2) && pagosPlanificados != null && pagosPlanificados.Length > 0)
                        {
                            JArray pagosArreglo = JArray.Parse((string)value.pagosPlanificados);
                            for (int i = 0; i < pagosArreglo.Count; i++)
                            {
                                JObject objeto = (JObject)pagosArreglo[i];
                                DateTime fechaPago = objeto["fechaPago"] != null ? Convert.ToDateTime(objeto["fechaPago"].ToString()) : default(DateTime);
                                decimal monto = objeto["pago"] != null ? Convert.ToDecimal(objeto["pago"].ToString()) : default(decimal);


                                PagoPlanificado pagoPlanificado = new PagoPlanificado();
                                pagoPlanificado.fechaPago = fechaPago;
                                pagoPlanificado.pago = monto;
                                pagoPlanificado.objetoId = componente.id;
                                pagoPlanificado.objetoTipo = 1;
                                pagoPlanificado.usuarioCreo = User.Identity.Name;
                                pagoPlanificado.fechaCreacion = DateTime.Now;
                                pagoPlanificado.estado = 1;
                                result = result && PagoPlanificadoDAO.Guardar(pagoPlanificado);
                            }
                        }
                    }

                    if (result)
                    {
                        List<ComponentePropiedad> componentePropiedades = ComponentePropiedadDAO.getComponentePropiedadesPorTipoComponente(componente.componenteTipoid);

                        foreach (ComponentePropiedad componentePropiedad in componentePropiedades)
                        {
                            ComponentePropiedadValor compPropVal = ComponentePropiedadValorDAO.getValorPorComponenteYPropiedad(componentePropiedad.id, componente.id);
                            result = result && ComponentePropiedadValorDAO.eliminarTotalComponentePropiedadValor(compPropVal);
                        }

                        JArray datosDinamicos = JArray.Parse((string)value.camposDinamicos);

                        for (int i = 0; i < datosDinamicos.Count; i++)
                        {
                            JObject data = (JObject)datosDinamicos[i];

                            if (data["valor"] != null && data["valor"].ToString().Length > 0 && data["valor"].ToString().CompareTo("null") != 0)
                            {
                                ComponentePropiedad componentePropiedad = ComponentePropiedadDAO.getComponentePropiedadPorId(Convert.ToInt32(data["id"]));
                                ComponentePropiedadValor valor = new ComponentePropiedadValor();
                                valor.componentes = componente;
                                valor.componenteid = componente.id;
                                valor.componentePropiedads = componentePropiedad;
                                valor.componentePropiedadid = componentePropiedad.id;
                                valor.usuarioCreo = User.Identity.Name;
                                valor.fechaCreacion = DateTime.Now;

                                switch (componentePropiedad.datoTipoid)
                                {
                                    case 1:
                                        valor.valorString = data["valor"].ToString();
                                        break;
                                    case 2:
                                        valor.valorEntero = Convert.ToInt32(data["valor"].ToString());
                                        break;
                                    case 3:
                                        valor.valorDecimal = Convert.ToDecimal(data["valor"].ToString());
                                        break;
                                    case 4:
                                        valor.valorEntero = data["valor"].ToString() == "true" ? 1 : 0;
                                        break;
                                    case 5:
                                        valor.valorTiempo = Convert.ToDateTime(data["valor_f"].ToString());
                                        break;
                                }
                                result = (result && ComponentePropiedadValorDAO.guardarComponentePropiedadValor(valor));
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = true,
                        id = componente.id,
                        usuarioCreo = componente.usuarioCreo,
                        usuarioActualizo = componente.usuarioActualizo,
                        fechaCreacion = componente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = componente.fechaActualizacion != null ? componente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("10", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Componentes - Editar")]
        public IActionResult Componente(int id, [FromBody]dynamic value)
        {
            try
            {
                bool result = false;
                ComponenteValidator validator = new ComponenteValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Componente componente = ComponenteDAO.getComponentePorId(id, User.Identity.Name);
                    componente.nombre = value.nombre;
                    componente.descripcion = value.descripcion;
                    componente.componenteTipoid = value.componenteTipoid;
                    componente.proyectoid = value.proyectoid;
                    componente.ueunidadEjecutora = value.ueunidadEjecutora;
                    componente.ejercicio = value.ejercicio;
                    componente.entidad = value.entidad;
                    componente.snip = value.snip;
                    componente.programa = value.programa;
                    componente.subprograma = value.subprograma;
                    componente.proyecto = value.proyecto;
                    componente.actividad = value.actividad;
                    componente.obra = value.obra;
                    componente.renglon = value.renglon;
                    componente.ubicacionGeografica = value.ubicacionGeografica;
                    componente.latitud = value.latitud;
                    componente.longitud = value.longitud;
                    componente.costo = value.costo;
                    componente.acumulacionCostoid = value.acumulacionCostoid;
                    componente.fechaInicio = value.fechaInicio;
                    componente.fechaFin = value.fechaFin;
                    componente.duracion = value.duracion;
                    componente.duracionDimension = value.duracionDimension;
                    componente.esDeSigade = value.esDeSigade;
                    componente.inversionNueva = value.inversionNueva;
                    componente.fechaActualizacion = DateTime.Now;
                    componente.usuarioActualizo = User.Identity.Name;
                    componente.fuentePrestamo = value.fuentePrestamo;
                    componente.fuenteDonacion = value.fuenteDonacion;
                    componente.fuenteNacional = value.fuenteNacional;
                    componente.fechaInicioReal = value.fechaInicioReal;
                    componente.fechaFinReal = value.fechaFinReal;

                    result = ComponenteDAO.guardarComponente(componente, true);

                    if (result)
                    {
                        String pagosPlanificados = value.pagosPlanificados;
                        if (!componente.acumulacionCostoid.Equals(2) || pagosPlanificados != null && pagosPlanificados.Length > 0)
                        {
                            List<PagoPlanificado> pagosActuales = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(componente.id, 1);
                            foreach (PagoPlanificado pagoTemp in pagosActuales)
                            {
                                PagoPlanificadoDAO.eliminarTotalPagoPlanificado(pagoTemp);
                            }
                        }

                        if (componente.acumulacionCostoid.Equals(2) && pagosPlanificados != null && pagosPlanificados.Length > 0)
                        {
                            JArray pagosArreglo = JArray.Parse((string)value.pagosPlanificados);
                            for (int i = 0; i < pagosArreglo.Count; i++)
                            {
                                JObject objeto = (JObject)pagosArreglo[i];
                                DateTime fechaPago = objeto["fechaPago"] != null ? Convert.ToDateTime(objeto["fechaPago"].ToString()) : default(DateTime);
                                decimal monto = objeto["pago"] != null ? Convert.ToDecimal(objeto["pago"].ToString()) : default(decimal);


                                PagoPlanificado pagoPlanificado = new PagoPlanificado();
                                pagoPlanificado.fechaPago = fechaPago;
                                pagoPlanificado.pago = monto;
                                pagoPlanificado.objetoId = componente.id;
                                pagoPlanificado.objetoTipo = 1;
                                pagoPlanificado.usuarioCreo = User.Identity.Name;
                                pagoPlanificado.fechaCreacion = DateTime.Now;
                                pagoPlanificado.estado = 1;
                                result = result && PagoPlanificadoDAO.Guardar(pagoPlanificado);
                            }
                        }
                    }

                    if (result)
                    {
                        List<ComponentePropiedad> componentePropiedades = ComponentePropiedadDAO.getComponentePropiedadesPorTipoComponente(componente.componenteTipoid);

                        foreach (ComponentePropiedad componentePropiedad in componentePropiedades)
                        {
                            ComponentePropiedadValor compPropVal = ComponentePropiedadValorDAO.getValorPorComponenteYPropiedad(componentePropiedad.id, componente.id);
                            result &= ComponentePropiedadValorDAO.eliminarTotalComponentePropiedadValor(compPropVal);
                        }

                        JArray datosDinamicos = JArray.Parse((string)value.camposDinamicos);

                        for (int i = 0; i < datosDinamicos.Count; i++)
                        {
                            JObject data = (JObject)datosDinamicos[i];

                            if (data["valor"] != null && data["valor"].ToString().Length > 0 && data["valor"].ToString().CompareTo("null") != 0)
                            {
                                ComponentePropiedad componentePropiedad = ComponentePropiedadDAO.getComponentePropiedadPorId(Convert.ToInt32(data["id"]));
                                ComponentePropiedadValor valor = new ComponentePropiedadValor();
                                valor.componentes = componente;
                                valor.componenteid = componente.id;
                                valor.componentePropiedads = componentePropiedad;
                                valor.componentePropiedadid = componentePropiedad.id;
                                valor.usuarioCreo = User.Identity.Name;
                                valor.fechaCreacion = DateTime.Now;

                                switch (componentePropiedad.datoTipoid)
                                {
                                    case 1:
                                        valor.valorString = data["valor"].ToString();
                                        break;
                                    case 2:
                                        valor.valorEntero = Convert.ToInt32(data["valor"].ToString());
                                        break;
                                    case 3:
                                        valor.valorDecimal = Convert.ToDecimal(data["valor"].ToString());
                                        break;
                                    case 4:
                                        valor.valorEntero = data["valor"].ToString() == "true" ? 1 : 0;
                                        break;
                                    case 5:
                                        valor.valorTiempo = Convert.ToDateTime(data["valor_f"].ToString());
                                        break;
                                }
                                result &= ComponentePropiedadValorDAO.guardarComponentePropiedadValor(valor);
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = true,
                        id = componente.id,
                        usuarioCreo = componente.usuarioCreo,
                        usuarioActualizo = componente.usuarioActualizo,
                        fechaCreacion = componente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = componente.fechaActualizacion != null ? componente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("11", "ComponenteController.class", e);
                return BadRequest(500);
            }
        }
    }
}
