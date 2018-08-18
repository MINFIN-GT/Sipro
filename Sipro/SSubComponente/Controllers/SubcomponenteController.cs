using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SSubComponente.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class SubcomponenteController : Controller
    {
        private class Stsubcomponente
        {
            public int id;
            public String nombre;
            public String descripcion;
            public int componenteid;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int subcomponenteTipoid;
            public String subcomponentetiponombre;
            public int estado;
            public long? snip;
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
            public bool tieneHijos;
            public String fechaInicioReal;
            public String fechaFinReal;
            public int congelado;
            public int inversionNueva;
        }

        private class Stdatadinamico
        {
            public String id;
            public String tipo;
            public String label;
            public String valor;
            public String valor_f;
        }

        [HttpGet("{pagina}/{numerosubcomponentes}")]
        [Authorize("Subcomponentes - Visualizar")]
        public IActionResult SubComponentesPagina(int pagina, int numerosubcomponentes)
        {
            try
            {
                List<Subcomponente> subcomponentes = SubComponenteDAO.getSubComponentesPagina(pagina, numerosubcomponentes, User.Identity.Name);
                List<Stsubcomponente> stsubcomponentes = new List<Stsubcomponente>();

                foreach (Subcomponente subcomponente in subcomponentes)
                {
                    Stsubcomponente temp = new Stsubcomponente();
                    temp.descripcion = subcomponente.descripcion;
                    temp.estado = subcomponente.estado;
                    temp.fechaActualizacion = subcomponente.fechaActualizacion != null ? subcomponente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = subcomponente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.id = subcomponente.id;
                    temp.nombre = subcomponente.nombre;
                    temp.usuarioActualizo = subcomponente.usuarioActualizo;
                    temp.usuarioCreo = subcomponente.usuarioCreo;

                    subcomponente.subcomponenteTipos = SubComponenteTipoDAO.getSubComponenteTipoPorId(subcomponente.subcomponenteTipoid);
                    temp.subcomponenteTipoid = subcomponente.subcomponenteTipoid;
                    temp.subcomponentetiponombre = subcomponente.subcomponenteTipos.nombre;

                    temp.snip = subcomponente.snip;
                    temp.programa = subcomponente.programa;
                    temp.subprograma = subcomponente.subprograma;
                    temp.proyecto = subcomponente.proyecto;
                    temp.actividad = subcomponente.actividad;
                    temp.renglon = subcomponente.renglon;
                    temp.ubicacionGeografica = subcomponente.ubicacionGeografica;
                    temp.duracion = subcomponente.duracion;
                    temp.duracionDimension = subcomponente.duracionDimension;
                    temp.fechaInicio = subcomponente.fechaInicio != null ? subcomponente.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFin = subcomponente.fechaFin != null ? subcomponente.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.obra = subcomponente.obra;

                    subcomponente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subcomponente.ejercicio ?? default(int), subcomponente.entidad ?? default(int), subcomponente.ueunidadEjecutora ?? default(int));
                    if (subcomponente.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = subcomponente.ueunidadEjecutora ?? default(int);
                        temp.ejercicio = subcomponente.ejercicio ?? default(int);

                        subcomponente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subcomponente.entidad ?? default(int), subcomponente.ejercicio ?? default(int));
                        temp.entidad = subcomponente.entidad ?? default(int);
                        temp.unidadejecutoranombre = subcomponente.unidadEjecutoras.nombre;
                        temp.entidadnombre = subcomponente.unidadEjecutoras.entidads.nombre;
                    }
                    else
                    {
                        Componente componente = ComponenteDAO.getComponente(subcomponente.componenteid);
                        componente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(componente.ejercicio, componente.entidad ?? default(int), componente.ueunidadEjecutora);
                        if (componente.unidadEjecutoras != null)
                        {
                            temp.ueunidadEjecutora = componente.ueunidadEjecutora;
                            temp.ejercicio = componente.ejercicio;

                            componente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(componente.entidad ?? default(int), componente.ejercicio);
                            temp.entidad = componente.entidad ?? default(int);
                            temp.unidadejecutoranombre = componente.unidadEjecutoras.nombre;
                            temp.entidadnombre = subcomponente.unidadEjecutoras.entidads.nombre;
                        }
                    }

                    temp.latitud = subcomponente.latitud;
                    temp.longitud = subcomponente.longitud;
                    temp.costo = subcomponente.costo ?? default(decimal);

                    subcomponente.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(subcomponente.acumulacionCostoid));
                    temp.acumulacionCostoid = Convert.ToInt32(subcomponente.acumulacionCostoid);
                    temp.acumulacionCostoNombre = subcomponente.acumulacionCostos.nombre;

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 2);
                    temp.fechaInicioReal = subcomponente.fechaInicioReal != null ? subcomponente.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFinReal = subcomponente.fechaFinReal != null ? subcomponente.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.inversionNueva = subcomponente.inversionNueva;
                    stsubcomponentes.Add(temp);
                }

                return Ok(new { success = true, subcomponentes = stsubcomponentes });
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        [Authorize("Subcomponentes - Visualizar")]
        public IActionResult SubComponentes()
        {
            try
            {
                List<Subcomponente> subcomponentes = SubComponenteDAO.getSubComponentes(User.Identity.Name);
                List<Stsubcomponente> stsubcomponentes = new List<Stsubcomponente>();
                foreach (Subcomponente subcomponente in subcomponentes)
                {
                    Stsubcomponente temp = new Stsubcomponente();
                    temp.descripcion = subcomponente.descripcion;
                    temp.estado = subcomponente.estado;
                    temp.fechaActualizacion = subcomponente.fechaActualizacion != null ? subcomponente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = subcomponente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.id = subcomponente.id;
                    temp.nombre = subcomponente.nombre;
                    temp.usuarioActualizo = subcomponente.usuarioActualizo;
                    temp.usuarioCreo = subcomponente.usuarioCreo;

                    subcomponente.subcomponenteTipos = SubComponenteTipoDAO.getSubComponenteTipoPorId(subcomponente.subcomponenteTipoid);
                    temp.subcomponenteTipoid = subcomponente.subcomponenteTipoid;
                    temp.subcomponentetiponombre = subcomponente.subcomponenteTipos.nombre;

                    temp.snip = subcomponente.snip;
                    temp.programa = subcomponente.programa;
                    temp.subprograma = subcomponente.subprograma;
                    temp.proyecto = subcomponente.proyecto;
                    temp.actividad = subcomponente.actividad;
                    temp.renglon = subcomponente.renglon;
                    temp.ubicacionGeografica = subcomponente.ubicacionGeografica;
                    temp.duracion = subcomponente.duracion;
                    temp.duracionDimension = subcomponente.duracionDimension;
                    temp.fechaInicio = subcomponente.fechaInicio != null ? subcomponente.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFin = subcomponente.fechaFin != null ? subcomponente.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.obra = subcomponente.obra;

                    subcomponente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subcomponente.ejercicio ?? default(int), subcomponente.entidad ?? default(int), subcomponente.ueunidadEjecutora ?? default(int));
                    if (subcomponente.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = subcomponente.ueunidadEjecutora ?? default(int);
                        temp.ejercicio = subcomponente.ejercicio ?? default(int);

                        subcomponente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subcomponente.entidad ?? default(int), subcomponente.ejercicio ?? default(int));
                        temp.entidad = subcomponente.entidad ?? default(int);
                        temp.unidadejecutoranombre = subcomponente.unidadEjecutoras.nombre;
                        temp.entidadnombre = subcomponente.unidadEjecutoras.entidads.nombre;
                    }
                    else
                    {
                        Componente componente = ComponenteDAO.getComponente(subcomponente.componenteid);
                        componente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(componente.ejercicio, componente.entidad ?? default(int), componente.ueunidadEjecutora);
                        if (componente.unidadEjecutoras != null)
                        {
                            temp.ueunidadEjecutora = componente.ueunidadEjecutora;
                            temp.ejercicio = componente.ejercicio;

                            componente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(componente.entidad ?? default(int), componente.ejercicio);
                            temp.entidad = componente.entidad ?? default(int);
                            temp.unidadejecutoranombre = componente.unidadEjecutoras.nombre;
                            temp.entidadnombre = subcomponente.unidadEjecutoras.entidads.nombre;
                        }
                    }

                    temp.latitud = subcomponente.latitud;
                    temp.longitud = subcomponente.longitud;
                    temp.costo = subcomponente.costo ?? default(decimal);

                    subcomponente.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(subcomponente.acumulacionCostoid));
                    temp.acumulacionCostoid = Convert.ToInt32(subcomponente.acumulacionCostoid);
                    temp.acumulacionCostoNombre = subcomponente.acumulacionCostos.nombre;

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 2);
                    temp.fechaInicioReal = subcomponente.fechaInicioReal != null ? subcomponente.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFinReal = subcomponente.fechaFinReal != null ? subcomponente.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.inversionNueva = subcomponente.inversionNueva;
                    stsubcomponentes.Add(temp);
                }

                return Ok(new { success = true, subcomponentes  = stsubcomponentes });
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subcomponentes - Crear")]
        public IActionResult SubComponente([FromBody]dynamic value)
        {
            try
            {
                bool result = false;
                SubcomponenteValidator validator = new SubcomponenteValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Subcomponente subComponente = new Subcomponente();
                    subComponente.nombre = value.nombre;
                    subComponente.descripcion = value.descripcion;
                    subComponente.componenteid = value.componenteid;
                    subComponente.subcomponenteTipoid = value.subcomponenteTipoid;
                    subComponente.ueunidadEjecutora = value.ueunidadEjecutora;
                    subComponente.ejercicio = value.ejercicio;
                    subComponente.entidad = value.entidad;
                    subComponente.snip = value.snip ?? null;
                    subComponente.programa = value.programa ?? null;
                    subComponente.subprograma = value.subprograma ?? null;
                    subComponente.proyecto = value.proyecto ?? null;
                    subComponente.actividad = value.actividad ?? null;
                    subComponente.obra = value.obra ?? null;
                    subComponente.renglon = value.renglon ?? null;
                    subComponente.ubicacionGeografica = value.ubicacionGeografica ?? null;
                    subComponente.latitud = value.latitud;
                    subComponente.longitud = value.longitud;
                    subComponente.costo = value.costo;
                    subComponente.acumulacionCostoid = value.acumulacionCostoid;
                    subComponente.fechaInicio = value.fechaInicio;

                    DateTime fechaFin;
                    DateTime.TryParse((string)value.fechaFin, out fechaFin);

                    subComponente.fechaFin = fechaFin;
                    subComponente.duracion = value.duracion;
                    subComponente.duracionDimension = value.duracionDimension;
                    subComponente.inversionNueva = value.inversionNueva;
                    subComponente.fechaCreacion = DateTime.Now;
                    subComponente.usuarioCreo = User.Identity.Name;
                    subComponente.estado = 1;
                    subComponente.fechaInicioReal = value.fechaInicioReal;
                    subComponente.fechaFinReal = value.fechaFinReal;

                    result = SubComponenteDAO.guardarSubComponente(subComponente, true);

                    if (result)
                    {
                        String pagosPlanificados = value.pagosPlanificados;
                        if (!subComponente.acumulacionCostoid.Equals(2) || pagosPlanificados != null && pagosPlanificados.Length > 0)
                        {
                            List<PagoPlanificado> pagosActuales = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(subComponente.id, 2);
                            foreach (PagoPlanificado pagoTemp in pagosActuales)
                            {
                                PagoPlanificadoDAO.eliminarTotalPagoPlanificado(pagoTemp);
                            }
                        }

                        if (subComponente.acumulacionCostoid.Equals(2) && pagosPlanificados != null && pagosPlanificados.Length > 0)
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
                                pagoPlanificado.objetoId = subComponente.id;
                                pagoPlanificado.objetoTipo = 2;
                                pagoPlanificado.usuarioCreo = User.Identity.Name;
                                pagoPlanificado.fechaCreacion = DateTime.Now;
                                pagoPlanificado.estado = 1;
                                result = result && PagoPlanificadoDAO.Guardar(pagoPlanificado);
                            }
                        }
                    }

                    if (result)
                    {
                        List<SubcomponentePropiedad> subcomponentePropiedades = SubComponentePropiedadDAO.getSubComponentePropiedadesPorTipoSubComponente(subComponente.subcomponenteTipoid);

                        foreach (SubcomponentePropiedad subComponentePropiedad in subcomponentePropiedades)
                        {
                            SubcomponentePropiedadValor subCompPropVal = SubcomponentePropiedadValorDAO.getValorPorSubComponenteYPropiedad(subComponentePropiedad.id, subComponente.id);
                            if (subCompPropVal != null)
                                result = result && SubcomponentePropiedadValorDAO.eliminarTotalSubComponentePropiedadValor(subCompPropVal);
                        }

                        JArray datosDinamicos = JArray.Parse((string)value.camposDinamicos);

                        for (int i = 0; i < datosDinamicos.Count; i++)
                        {
                            JObject data = (JObject)datosDinamicos[i];

                            if (data["valor"] != null && data["valor"].ToString().Length > 0 && data["valor"].ToString().CompareTo("null") != 0)
                            {
                                SubcomponentePropiedad subComponentePropiedad = SubComponentePropiedadDAO.getSubComponentePropiedadPorId(Convert.ToInt32(data["id"]));
                                SubcomponentePropiedadValor valor = new SubcomponentePropiedadValor();
                                valor.subcomponentes = subComponente;
                                valor.subcomponenteid = subComponente.id;
                                valor.subcomponentePropiedads = subComponentePropiedad;
                                valor.subcomponentePropiedadid = subComponentePropiedad.id;
                                valor.usuarioCreo = User.Identity.Name;
                                valor.fechaCreacion = DateTime.Now;

                                switch (subComponentePropiedad.datoTipoid)
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
                                result = (result && SubcomponentePropiedadValorDAO.guardarSubComponentePropiedadValor(valor));
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = result,
                        id = subComponente.id,
                        usuarioCreo = subComponente.usuarioCreo,
                        usuarioActualizo = subComponente.usuarioActualizo,
                        fechaCreacion = subComponente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = subComponente.fechaActualizacion != null ? subComponente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Subcomponentes - Editar")]
        public IActionResult SubComponente(int id, [FromBody]dynamic value)
        {
            try
            {
                bool result = false;
                SubcomponenteValidator validator = new SubcomponenteValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Subcomponente subComponente = SubComponenteDAO.getSubComponente(id);
                    subComponente.nombre = value.nombre;
                    subComponente.descripcion = value.descripcion;
                    subComponente.componenteid = value.componenteid;
                    subComponente.subcomponenteTipoid = value.subcomponenteTipoid;
                    subComponente.ueunidadEjecutora = value.ueunidadEjecutora;
                    subComponente.ejercicio = value.ejercicio;
                    subComponente.entidad = value.entidad;
                    subComponente.snip = value.snip ?? null;
                    subComponente.programa = (value.programa != null && value.programa != "") ? value.programa : null;
                    subComponente.subprograma = (value.subprograma != null && value.subprograma != "") ? value.subprograma : null;
                    subComponente.proyecto = (value.proyecto != null && value.proyecto != "") ? value.proyecto : null;
                    subComponente.actividad = (value.actividad  != null && value.actividad != "") ? value.actividad : null;
                    subComponente.obra = (value.obra != null && value.obra != "") ? value.obra : null;
                    subComponente.renglon = (value.renglon != null && value.renglon != "") ? value.renglon : null;
                    subComponente.ubicacionGeografica = (value.ubicacionGeografica != null && value.ubicacionGeografica != "") ? value.ubicacionGeografica : null;
                    subComponente.latitud = value.latitud;
                    subComponente.longitud = value.longitud;
                    subComponente.costo = value.costo;
                    subComponente.acumulacionCostoid = value.acumulacionCostoid;
                    subComponente.fechaInicio = value.fechaInicio;

                    DateTime fechaFin;
                    DateTime.TryParse((string)value.fechaFin, out fechaFin);

                    subComponente.fechaFin = fechaFin;

                    subComponente.duracion = value.duracion;
                    subComponente.duracionDimension = value.duracionDimension;
                    subComponente.inversionNueva = value.inversionNueva;
                    subComponente.fechaActualizacion = DateTime.Now;
                    subComponente.usuarioActualizo = User.Identity.Name;
                    subComponente.fechaInicioReal = value.fechaInicioReal;
                    subComponente.fechaFinReal = value.fechaFinReal;

                    result = SubComponenteDAO.guardarSubComponente(subComponente, true);

                    if (result)
                    {
                        String pagosPlanificados = value.pagosPlanificados;
                        if (!subComponente.acumulacionCostoid.Equals(2) || pagosPlanificados != null && pagosPlanificados.Length > 0)
                        {
                            List<PagoPlanificado> pagosActuales = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(subComponente.id, 2);
                            foreach (PagoPlanificado pagoTemp in pagosActuales)
                            {
                                PagoPlanificadoDAO.eliminarTotalPagoPlanificado(pagoTemp);
                            }
                        }

                        if (subComponente.acumulacionCostoid.Equals(2) && pagosPlanificados != null && pagosPlanificados.Length > 0)
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
                                pagoPlanificado.objetoId = subComponente.id;
                                pagoPlanificado.objetoTipo = 2;
                                pagoPlanificado.usuarioCreo = User.Identity.Name;
                                pagoPlanificado.fechaCreacion = DateTime.Now;
                                pagoPlanificado.estado = 1;
                                result = result && PagoPlanificadoDAO.Guardar(pagoPlanificado);
                            }
                        }
                    }

                    if (result)
                    {
                        List<SubcomponentePropiedad> subcomponentePropiedades = SubComponentePropiedadDAO.getSubComponentePropiedadesPorTipoSubComponente(subComponente.subcomponenteTipoid);

                        foreach (SubcomponentePropiedad subComponentePropiedad in subcomponentePropiedades)
                        {
                            SubcomponentePropiedadValor subCompPropVal = SubcomponentePropiedadValorDAO.getValorPorSubComponenteYPropiedad(subComponentePropiedad.id, subComponente.id);
                            if(subCompPropVal != null)
                                result = result && SubcomponentePropiedadValorDAO.eliminarTotalSubComponentePropiedadValor(subCompPropVal);
                        }

                        JArray datosDinamicos = JArray.Parse((string)value.camposDinamicos);

                        for (int i = 0; i < datosDinamicos.Count; i++)
                        {
                            JObject data = (JObject)datosDinamicos[i];

                            if (data["valor"] != null && data["valor"].ToString().Length > 0 && data["valor"].ToString().CompareTo("null") != 0)
                            {
                                SubcomponentePropiedad subComponentePropiedad = SubComponentePropiedadDAO.getSubComponentePropiedadPorId(Convert.ToInt32(data["id"]));
                                SubcomponentePropiedadValor valor = new SubcomponentePropiedadValor();
                                valor.subcomponentes = subComponente;
                                valor.subcomponenteid = subComponente.id;
                                valor.subcomponentePropiedads = subComponentePropiedad;
                                valor.subcomponentePropiedadid = subComponentePropiedad.id;
                                valor.usuarioCreo = User.Identity.Name;
                                valor.fechaCreacion = DateTime.Now;

                                switch (subComponentePropiedad.datoTipoid)
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
                                result = (result && SubcomponentePropiedadValorDAO.guardarSubComponentePropiedadValor(valor));
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = result,
                        id = subComponente.id,
                        usuarioCreo = subComponente.usuarioCreo,
                        usuarioActualizo = subComponente.usuarioActualizo,
                        fechaCreacion = subComponente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = subComponente.fechaActualizacion != null ? subComponente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        [Authorize("Subcomponentes - Visualizar")]
        public IActionResult NumeroSubComponentes()
        {
            try
            {
                long total = SubComponenteDAO.getTotalSubComponentes(User.Identity.Name);

                return Ok(new { success = true, totalsubcomponentes = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subcomponentes - Visualizar")]
        public IActionResult NumeroSubComponentesPorComponente([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda;
                int componenteId = value.componenteid != null ? (int)value.componenteid : default(int);
                long total = SubComponenteDAO.getTotalSubComponentesPorComponente(componenteId, filtro_busqueda, User.Identity.Name);

                return Ok(new { success = true, totalsubcomponentes = total });
            }
            catch (Exception e)
            {
                CLogger.write("6", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subcomponentes - Visualizar")]
        public IActionResult SubComponentesPaginaPorComponente([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int componenteId = value.componenteid != null ? (int)value.componenteid : default(int);
                int numeroSubComponentes = value.numerosubcomponentes != null ? (int)value.numerosubcomponentes : 20;
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;

                List<Subcomponente> subcomponentes = SubComponenteDAO.getSubComponentesPaginaPorComponente(pagina, numeroSubComponentes,
                        componenteId, filtro_busqueda, columna_ordenada, orden_direccion, User.Identity.Name);
                List<Stsubcomponente> stsubcomponentes = new List<Stsubcomponente>();
                foreach (Subcomponente subcomponente in subcomponentes)
                {
                    Stsubcomponente temp = new Stsubcomponente();
                    temp.descripcion = subcomponente.descripcion;
                    temp.estado = subcomponente.estado;
                    temp.fechaActualizacion = subcomponente.fechaActualizacion != null ? subcomponente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = subcomponente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.id = subcomponente.id;
                    temp.nombre = subcomponente.nombre;
                    temp.usuarioActualizo = subcomponente.usuarioActualizo;
                    temp.usuarioCreo = subcomponente.usuarioCreo;

                    subcomponente.subcomponenteTipos = SubComponenteTipoDAO.getSubComponenteTipoPorId(subcomponente.subcomponenteTipoid);
                    temp.subcomponenteTipoid = subcomponente.subcomponenteTipoid;
                    temp.subcomponentetiponombre = subcomponente.subcomponenteTipos.nombre;

                    temp.snip = subcomponente.snip;
                    temp.programa = subcomponente.programa;
                    temp.subprograma = subcomponente.subprograma;
                    temp.proyecto = subcomponente.proyecto;
                    temp.actividad = subcomponente.actividad;
                    temp.renglon = subcomponente.renglon;
                    temp.ubicacionGeografica = subcomponente.ubicacionGeografica;
                    temp.duracion = subcomponente.duracion;
                    temp.duracionDimension = subcomponente.duracionDimension;
                    temp.fechaInicio = subcomponente.fechaInicio != null ? subcomponente.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFin = subcomponente.fechaFin != null ? subcomponente.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.obra = subcomponente.obra;

                    subcomponente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subcomponente.ejercicio ?? default(int), subcomponente.entidad ?? default(int), subcomponente.ueunidadEjecutora ?? default(int));
                    if (subcomponente.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = subcomponente.ueunidadEjecutora ?? default(int);
                        temp.ejercicio = subcomponente.ejercicio ?? default(int);

                        subcomponente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subcomponente.entidad ?? default(int), subcomponente.ejercicio ?? default(int));
                        temp.entidad = subcomponente.entidad ?? default(int);
                        temp.unidadejecutoranombre = subcomponente.unidadEjecutoras.nombre;
                        temp.entidadnombre = subcomponente.unidadEjecutoras.entidads.nombre;
                    }
                    else
                    {
                        Componente componente = ComponenteDAO.getComponente(subcomponente.componenteid);
                        componente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(componente.ejercicio, componente.entidad ?? default(int), componente.ueunidadEjecutora);
                        if (componente.unidadEjecutoras != null)
                        {
                            temp.ueunidadEjecutora = componente.ueunidadEjecutora;
                            temp.ejercicio = componente.ejercicio;

                            componente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(componente.entidad ?? default(int), componente.ejercicio);
                            temp.entidad = componente.entidad ?? default(int);
                            temp.unidadejecutoranombre = componente.unidadEjecutoras.nombre;
                            temp.entidadnombre = subcomponente.unidadEjecutoras.entidads.nombre;
                        }
                    }

                    temp.latitud = subcomponente.latitud;
                    temp.longitud = subcomponente.longitud;
                    temp.costo = subcomponente.costo ?? default(decimal);

                    subcomponente.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(subcomponente.acumulacionCostoid));
                    temp.acumulacionCostoid = Convert.ToInt32(subcomponente.acumulacionCostoid);
                    temp.acumulacionCostoNombre = subcomponente.acumulacionCostos.nombre;

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 2);
                    temp.fechaInicioReal = subcomponente.fechaInicioReal != null ? subcomponente.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFinReal = subcomponente.fechaFinReal != null ? subcomponente.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.inversionNueva = subcomponente.inversionNueva;

                    stsubcomponentes.Add(temp);
                }

                return Ok(new { success = true, subcomponentes = stsubcomponentes });
            }
            catch (Exception e)
            {
                CLogger.write("7", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Subcomponentes - Visualizar")]
        public IActionResult ObtenerSubComponentePorId(int id)
        {
            try
            {
                Subcomponente subcomponente = SubComponenteDAO.getSubComponentePorId(id, User.Identity.Name);
                int congelado = 0;

                if (subcomponente != null)
                {
                    subcomponente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subcomponente.ejercicio ?? default(int), subcomponente.entidad ?? default(int), subcomponente.ueunidadEjecutora ?? default(int));
                    if (subcomponente.unidadEjecutoras != null)
                    {
                        subcomponente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subcomponente.entidad ?? default(int), subcomponente.ejercicio ?? default(int));
                    }

                    Proyecto proyecto = ProyectoDAO.getProyectobyTreePath(subcomponente.treepath);
                    congelado = proyecto.congelado ?? default(int);

                    return Ok(new
                    {
                        success = true,
                        id = subcomponente.id,
                        fechaInicio = subcomponente.fechaInicio != null ? subcomponente.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null,
                        prestamoId = proyecto.prestamoid,
                        ejercicio = subcomponente.ejercicio,
                        entidad = subcomponente.entidad,
                        entidadNombre = subcomponente.unidadEjecutoras.entidads.nombre,
                        unidadEjecutora = subcomponente.ueunidadEjecutora,
                        unidadEjecutoraNombre = subcomponente.unidadEjecutoras.nombre,
                        congelado = congelado,
                        nombre = subcomponente.nombre
                    });
                }
                else
                    return Ok(new { success = false });                
            }
            catch (Exception e)
            {
                CLogger.write("8", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Subcomponentes - Visualizar")]
        public IActionResult SubComponentePorId(int id)
        {
            try
            {
                Subcomponente subcomponente = SubComponenteDAO.getSubComponentePorId(id, User.Identity.Name);
                Stsubcomponente temp = new Stsubcomponente();
                temp.descripcion = subcomponente.descripcion;
                temp.estado = subcomponente.estado;
                temp.fechaActualizacion = subcomponente.fechaActualizacion != null ? subcomponente.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.fechaCreacion = subcomponente.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                temp.id = subcomponente.id;
                temp.nombre = subcomponente.nombre;
                temp.usuarioActualizo = subcomponente.usuarioActualizo;
                temp.usuarioCreo = subcomponente.usuarioCreo;

                subcomponente.subcomponenteTipos = SubComponenteTipoDAO.getSubComponenteTipoPorId(subcomponente.subcomponenteTipoid);
                temp.subcomponenteTipoid = subcomponente.subcomponenteTipoid;
                temp.subcomponentetiponombre = subcomponente.subcomponenteTipos.nombre;

                temp.snip = subcomponente.snip;
                temp.programa = subcomponente.programa;
                temp.subprograma = subcomponente.subprograma;
                temp.proyecto = subcomponente.proyecto;
                temp.actividad = subcomponente.actividad;
                temp.renglon = subcomponente.renglon;
                temp.ubicacionGeografica = subcomponente.ubicacionGeografica;
                temp.duracion = subcomponente.duracion;
                temp.duracionDimension = subcomponente.duracionDimension;
                temp.fechaInicio = subcomponente.fechaInicio != null ? subcomponente.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.fechaFin = subcomponente.fechaFin != null ? subcomponente.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.obra = subcomponente.obra;

                subcomponente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subcomponente.ejercicio ?? default(int), subcomponente.entidad ?? default(int), subcomponente.ueunidadEjecutora ?? default(int));
                if (subcomponente.unidadEjecutoras != null)
                {
                    temp.ueunidadEjecutora = subcomponente.ueunidadEjecutora ?? default(int);
                    temp.ejercicio = subcomponente.ejercicio ?? default(int);

                    subcomponente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subcomponente.entidad ?? default(int), subcomponente.ejercicio ?? default(int));
                    temp.entidad = subcomponente.entidad ?? default(int);
                    temp.unidadejecutoranombre = subcomponente.unidadEjecutoras.nombre;
                    temp.entidadnombre = subcomponente.unidadEjecutoras.entidads.nombre;
                }
                else
                {
                    Componente componente = ComponenteDAO.getComponente(subcomponente.componenteid);
                    componente.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(componente.ejercicio, componente.entidad ?? default(int), componente.ueunidadEjecutora);
                    if (componente.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = componente.ueunidadEjecutora;
                        temp.ejercicio = componente.ejercicio;

                        componente.unidadEjecutoras.entidads = EntidadDAO.getEntidad(componente.entidad ?? default(int), componente.ejercicio);
                        temp.entidad = componente.entidad ?? default(int);
                        temp.unidadejecutoranombre = componente.unidadEjecutoras.nombre;
                        temp.entidadnombre = subcomponente.unidadEjecutoras.entidads.nombre;
                    }
                }

                temp.latitud = subcomponente.latitud;
                temp.longitud = subcomponente.longitud;
                temp.costo = subcomponente.costo ?? default(decimal);

                subcomponente.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(subcomponente.acumulacionCostoid));
                temp.acumulacionCostoid = Convert.ToInt32(subcomponente.acumulacionCostoid);
                temp.acumulacionCostoNombre = subcomponente.acumulacionCostos.nombre;

                temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 2);
                temp.fechaInicioReal = subcomponente.fechaInicioReal != null ? subcomponente.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.fechaFinReal = subcomponente.fechaFinReal != null ? subcomponente.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.inversionNueva = subcomponente.inversionNueva;

                Proyecto proyecto = ProyectoDAO.getProyectobyTreePath(subcomponente.treepath);
                temp.congelado = proyecto.congelado ?? default(int);

                return Ok(new { success = true, subcomponente = temp });
            }
            catch (Exception e)
            {
                CLogger.write("9", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Subcomponentes - Eliminar")]
        public IActionResult SubComponente(int id)
        {
            try
            {
                Subcomponente subcomponente = SubComponenteDAO.getSubComponente(id);

                bool eliminado = ObjetoDAO.borrarHijos(subcomponente.treepath, 2, User.Identity.Name);

                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("10", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Subcomponentes - Visualizar")]
        public IActionResult CantidadHistoria(int id)
        {
            try
            {
                String resultado = SubComponenteDAO.getVersiones(id);

                return Ok(new { success = true, versiones = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("11", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}/{version}")]
        [Authorize("Subcomponentes - Visualizar")]
        public IActionResult Historia(int id, int version)
        {
            try
            {
                String resultado = SubComponenteDAO.getHistoria(id, version);

                return Ok(new { success = true, historia = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("12", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subcomponentes - Visualizar")]
        public IActionResult ValidacionAsignado([FromBody]dynamic value)
        {
            try
            {
                DateTime cal = new DateTime();
                int ejercicio = cal.Year;
                Subcomponente objSubComponente = SubComponenteDAO.getSubComponente((int)value.id);
                Proyecto objProyecto = ProyectoDAO.getProyectobyTreePath(objSubComponente.treepath);
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
                switch (objSubComponente.acumulacionCostoid)
                {
                    case 1:
                        cal = objSubComponente.fechaInicio ?? default(DateTime);
                        int ejercicioInicial = cal.Year;
                        if (ejercicio.Equals(ejercicioInicial))
                        {
                            planificado = objSubComponente.costo ?? default(decimal);
                        }
                        break;
                    case 2:
                        List<PagoPlanificado> lstPagos = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(objSubComponente.id, 2);
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
                        cal = objSubComponente.fechaFin ?? default(DateTime);
                        int ejercicioFinal = cal.Year;
                        if (ejercicio.Equals(ejercicioFinal))
                        {
                            planificado += objSubComponente.costo ?? default(decimal);
                        }
                        break;
                }

                bool sobrepaso = false;
                if ((asignado = (asignado - planificado)).CompareTo(decimal.Zero) == -1)
                    sobrepaso = true;

                return Ok(new { success = true, asignado = asignado, sobrepaso = sobrepaso });
            }
            catch (Exception e)
            {
                CLogger.write("13", "SubcomponenteController.class", e);
                return BadRequest(500);
            }
        }
    }
}
