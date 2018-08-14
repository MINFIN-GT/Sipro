using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SSubComponentePropiedad.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class SubcomponentePropiedadController : Controller
    {
        private class Stsubcomponentepropiedad
        {
            public int id;
            public String nombre;
            public String descripcion;
            public int datoTipoid;
            public String datotiponombre;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }


        [HttpGet("{idSubComponenteTipo}")]
        [Authorize("Subcomponentes Propiedades - Visualizar")]
        public IActionResult SubComponentePropiedadPorTipo(int idSubComponenteTipo)
        {
            try
            {
                List<SubcomponentePropiedad> subcompoentepropiedades = SubComponentePropiedadDAO.getSubComponentePropiedadesPorTipoSubComponente(idSubComponenteTipo);
                List<Stsubcomponentepropiedad> stsubcomponentepropiedad = new List<Stsubcomponentepropiedad>();
                foreach (SubcomponentePropiedad subcomponentepropiedad in subcompoentepropiedades)
                {
                    Stsubcomponentepropiedad temp = new Stsubcomponentepropiedad();
                    temp.id = subcomponentepropiedad.id;
                    temp.nombre = subcomponentepropiedad.nombre;
                    temp.descripcion = subcomponentepropiedad.descripcion;

                    subcomponentepropiedad.datoTipos = DatoTipoDAO.getDatoTipo(subcomponentepropiedad.datoTipoid);

                    temp.datoTipoid = subcomponentepropiedad.datoTipoid;
                    temp.datotiponombre = subcomponentepropiedad.datoTipos.nombre;
                    temp.fechaActualizacion = subcomponentepropiedad.fechaActualizacion != null ? subcomponentepropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = subcomponentepropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = subcomponentepropiedad.usuarioActualizo;
                    temp.usuarioCreo = subcomponentepropiedad.usuarioCreo;
                    temp.estado = subcomponentepropiedad.estado;
                    stsubcomponentepropiedad.Add(temp);
                }

                return Ok(new { success = true, subcomponentepropiedades = stsubcomponentepropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubcomponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subcomponentes Propiedades - Visualizar")]
        public IActionResult SubComponentePropiedadPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int numeroSubComponentePropiedad = value.numeroSubComponentePropiedad != null ? (int)value.numeroSubComponentePropiedad : 20;
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;
                List<SubcomponentePropiedad> subcompoentepropiedades = SubComponentePropiedadDAO.getSubComponentePropiedadesPagina(pagina, numeroSubComponentePropiedad,
                        filtro_busqueda, columna_ordenada, orden_direccion);
                List<Stsubcomponentepropiedad> stsubcomponentepropiedad = new List<Stsubcomponentepropiedad>();
                foreach (SubcomponentePropiedad subcomponentepropiedad in subcompoentepropiedades)
                {
                    Stsubcomponentepropiedad temp = new Stsubcomponentepropiedad();
                    temp.id = subcomponentepropiedad.id;
                    temp.nombre = subcomponentepropiedad.nombre;
                    temp.descripcion = subcomponentepropiedad.descripcion;

                    subcomponentepropiedad.datoTipos = DatoTipoDAO.getDatoTipo(subcomponentepropiedad.datoTipoid);

                    temp.datoTipoid = subcomponentepropiedad.datoTipoid;
                    temp.datotiponombre = subcomponentepropiedad.datoTipos.nombre;
                    temp.fechaActualizacion = subcomponentepropiedad.fechaActualizacion != null ? subcomponentepropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = subcomponentepropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = subcomponentepropiedad.usuarioActualizo;
                    temp.usuarioCreo = subcomponentepropiedad.usuarioCreo;
                    temp.estado = subcomponentepropiedad.estado;
                    stsubcomponentepropiedad.Add(temp);
                }
                return Ok(new { success = true, subcomponentepropiedades = stsubcomponentepropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubcomponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subcomponentes Propiedades - Visualizar")]
        public IActionResult SubComponentePropiedadesTotalDisponibles([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                String idsPropiedades = value.idspropiedades != null ? value.idspropiedades : "0";
                int numeroSubComponentePropiedad = value.numeroSubComponentePropiedad != null ? (int)value.numeroSubComponentePropiedad : 20;
                List<SubcomponentePropiedad> subcomponentepropiedades = SubComponentePropiedadDAO.getSubComponentePropiedadPaginaTotalDisponibles(pagina, numeroSubComponentePropiedad, idsPropiedades);
                List<Stsubcomponentepropiedad> stsubcomponentepropiedad = new List<Stsubcomponentepropiedad>();
                foreach (SubcomponentePropiedad subcomponentepropiedad in subcomponentepropiedades)
                {
                    Stsubcomponentepropiedad temp = new Stsubcomponentepropiedad();
                    temp.id = subcomponentepropiedad.id;
                    temp.nombre = subcomponentepropiedad.nombre;
                    temp.descripcion = subcomponentepropiedad.descripcion;

                    subcomponentepropiedad.datoTipos = DatoTipoDAO.getDatoTipo(subcomponentepropiedad.datoTipoid);

                    temp.datoTipoid = subcomponentepropiedad.datoTipoid;
                    temp.datotiponombre = subcomponentepropiedad.datoTipos.nombre;
                    temp.fechaActualizacion = subcomponentepropiedad.fechaActualizacion != null ? subcomponentepropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = subcomponentepropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = subcomponentepropiedad.usuarioActualizo;
                    temp.usuarioCreo = subcomponentepropiedad.usuarioCreo;
                    temp.estado = subcomponentepropiedad.estado;
                    stsubcomponentepropiedad.Add(temp);
                }
                return Ok(new { success = true, subcomponentepropiedades = stsubcomponentepropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubcomponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        [Authorize("Subcomponentes Propiedades - Visualizar")]
        public IActionResult NumeroSubComponentePropiedadesDisponibles([FromBody]dynamic value)
        {
            try
            {
                long total = SubComponentePropiedadDAO.getTotalSubComponentePropiedades();

                return Ok(new { success = true, totalsubcomponentepropiedades = total });
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubcomponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subcomponentes Propiedades - Visualizar")]
        public IActionResult NumeroSubComponentePropiedades([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda;

                long total = SubComponentePropiedadDAO.getTotalSubComponentePropiedad(filtro_busqueda);
                
                return Ok(new { success = true, totalsubcomponentepropiedades = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubcomponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subcomponentes Propiedades - Crear")]
        public IActionResult SubComponentePropiedad([FromBody]dynamic value)
        {
            try
            {
                bool result = false;
                SubcomponentePropiedadValidator validator = new SubcomponentePropiedadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    SubcomponentePropiedad subComponentePropiedad = new SubcomponentePropiedad();
                    subComponentePropiedad.nombre = value.nombre;
                    subComponentePropiedad.descripcion = value.descripcion;
                    subComponentePropiedad.usuarioCreo = User.Identity.Name;
                    subComponentePropiedad.fechaCreacion = DateTime.Now;
                    subComponentePropiedad.estado = 1;
                    subComponentePropiedad.datoTipoid = value.datoTipoid;
                    result = SubComponentePropiedadDAO.guardarSubComponentePropiedad(subComponentePropiedad);

                    return Ok(new
                    {
                        success = result,
                        id = subComponentePropiedad.id,
                        usuarioCreo = subComponentePropiedad.usuarioCreo,
                        usuarioActualizo = subComponentePropiedad.usuarioActualizo,
                        fechaCreacion = subComponentePropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = subComponentePropiedad.fechaActualizacion != null ? subComponentePropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("6", "SubcomponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Subcomponentes Propiedades - Editar")]
        public IActionResult SubComponentePropiedad(int id, [FromBody]dynamic value)
        {
            try
            {
                bool result = false;
                SubcomponentePropiedadValidator validator = new SubcomponentePropiedadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    SubcomponentePropiedad subComponentePropiedad = SubComponentePropiedadDAO.getSubComponentePropiedadPorId(id);
                    subComponentePropiedad.nombre = value.nombre;
                    subComponentePropiedad.descripcion = value.descripcion;
                    subComponentePropiedad.usuarioActualizo = User.Identity.Name;
                    subComponentePropiedad.fechaActualizacion = DateTime.Now;
                    subComponentePropiedad.datoTipoid = value.datoTipoid;
                    result = SubComponentePropiedadDAO.guardarSubComponentePropiedad(subComponentePropiedad);

                    return Ok(new
                    {
                        success = result,
                        id = subComponentePropiedad.id,
                        usuarioCreo = subComponentePropiedad.usuarioCreo,
                        usuarioActualizo = subComponentePropiedad.usuarioActualizo,
                        fechaCreacion = subComponentePropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = subComponentePropiedad.fechaActualizacion != null ? subComponentePropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("7", "SubcomponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Subcomponentes Propiedades - Eliminar")]
        public IActionResult SubComponentePropiedad(int id)
        {
            try
            {
                SubcomponentePropiedad subcomponentePropiedad = SubComponentePropiedadDAO.getSubComponentePropiedadPorId(id);
                subcomponentePropiedad.usuarioActualizo = User.Identity.Name;
                bool eliminado = SubComponentePropiedadDAO.eliminarSubComponentePropiedad(subcomponentePropiedad);

                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("8", "SubcomponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{idSubComponente}/{idSubComponenteTipo}")]
        [Authorize("Subcomponentes Propiedades - Visualizar")]
        public IActionResult SubComponentePropiedadPorTipo(int idSubComponente, int idSubComponenteTipo)
        {
            try
            {
                List<SubcomponentePropiedad> subCompoentepropiedades = SubComponentePropiedadDAO.getSubComponentePropiedadesPorTipoComponente(idSubComponenteTipo);

                List<Dictionary<String, Object>> campos = new List<Dictionary<String, Object>>();
                foreach (SubcomponentePropiedad subComponentepropiedad in subCompoentepropiedades)
                {
                    Dictionary<String, Object> campo = new Dictionary<String, Object>();
                    campo.Add("id", subComponentepropiedad.id);
                    campo.Add("nombre", subComponentepropiedad.nombre);
                    campo.Add("tipo", subComponentepropiedad.datoTipoid);
                    SubcomponentePropiedadValor subComponentePropiedadValor = SubcomponentePropiedadValorDAO.getValorPorSubComponenteYPropiedad(subComponentepropiedad.id, idSubComponente);
                    if (subComponentePropiedadValor != null)
                    {
                        switch (subComponentepropiedad.datoTipoid)
                        {
                            case 1:
                                campo.Add("valor", subComponentePropiedadValor.valorString);
                                break;
                            case 2:
                                campo.Add("valor", subComponentePropiedadValor.valorEntero);
                                break;
                            case 3:
                                campo.Add("valor", subComponentePropiedadValor.valorDecimal);
                                break;
                            case 4:
                                campo.Add("valor", subComponentePropiedadValor.valorEntero == 1 ? true : false);
                                break;
                            case 5:
                                campo.Add("valor", subComponentePropiedadValor.valorTiempo != null ? subComponentePropiedadValor.valorTiempo.Value.ToString("dd/MM/yyyy H:mm:ss") : null);
                                break;
                        }
                    }
                    else
                    {
                        campo.Add("valor", "");
                    }
                    campos.Add(campo);
                }

                List<object> estructuraCamposDinamicos = CFormaDinamica.convertirEstructura(campos);

                return Ok(new { success = true, subcomponentepropiedades = estructuraCamposDinamicos });
            }
            catch (Exception e)
            {
                CLogger.write("9", "SubcomponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }
    }
}
