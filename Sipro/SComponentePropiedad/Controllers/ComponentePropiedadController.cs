using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SComponentePropiedad.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ComponentePropiedadController : Controller
    {
        private class stcomponentepropiedad
        {
            public int id;
            public String nombre;
            public String descripcion;
            public int datoTipoid;
            public String datoTipoNombre;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }

        [HttpGet("{idComponenteTipo}")]
        [Authorize("Componente Propiedades - Visualizar")]
        public IActionResult ComponentePropiedadPorTipo(int idComponenteTipo)
        {
            try
            {
                List<ComponentePropiedad> compoentepropiedades = ComponentePropiedadDAO.getComponentePropiedadesPorTipoComponentePagina(idComponenteTipo);
                List<stcomponentepropiedad> stcomponentepropiedad = new List<stcomponentepropiedad>();
                foreach (ComponentePropiedad componentepropiedad in compoentepropiedades)
                {
                    stcomponentepropiedad temp = new stcomponentepropiedad();
                    temp.id = componentepropiedad.id;
                    temp.nombre = componentepropiedad.nombre;
                    temp.descripcion = componentepropiedad.descripcion;
                    componentepropiedad.datoTipos = DatoTipoDAO.getDatoTipo(componentepropiedad.datoTipoid);
                    temp.datoTipoid = componentepropiedad.datoTipoid;
                    temp.datoTipoNombre = componentepropiedad.datoTipos.nombre;
                    temp.fechaActualizacion = componentepropiedad.fechaActualizacion != null ? componentepropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = componentepropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = componentepropiedad.usuarioActualizo;
                    temp.usuarioCreo = componentepropiedad.usuarioCreo;
                    temp.estado = componentepropiedad.estado;
                    stcomponentepropiedad.Add(temp);
                }

                return Ok(new { success = true, componentepropiedades = stcomponentepropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ComponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Componente Propiedades - Visualizar")]
        public IActionResult ComponentePropiedadPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int numeroComponentePropiedad = value.numerocomponentepropiedades != null ? (int)value.numerocomponentepropiedades : 20;
                String filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                String columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : null;
                String orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : null;
                List<ComponentePropiedad> compoentepropiedades = ComponentePropiedadDAO.getComponentePropiedadesPagina(pagina, numeroComponentePropiedad,
                        filtro_busqueda, columna_ordenada, orden_direccion);
                List<stcomponentepropiedad> stcomponentepropiedad = new List<stcomponentepropiedad>();
                foreach (ComponentePropiedad componentepropiedad in compoentepropiedades)
                {
                    stcomponentepropiedad temp = new stcomponentepropiedad();
                    temp.id = componentepropiedad.id;
                    temp.nombre = componentepropiedad.nombre;
                    temp.descripcion = componentepropiedad.descripcion;
                    componentepropiedad.datoTipos = DatoTipoDAO.getDatoTipo(componentepropiedad.datoTipoid);
                    temp.datoTipoid = componentepropiedad.datoTipoid;
                    temp.datoTipoNombre = componentepropiedad.datoTipos.nombre;
                    temp.fechaActualizacion = componentepropiedad.fechaActualizacion != null ? componentepropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = componentepropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = componentepropiedad.usuarioActualizo;
                    temp.usuarioCreo = componentepropiedad.usuarioCreo;
                    temp.estado = componentepropiedad.estado;
                    stcomponentepropiedad.Add(temp);
                }

                return Ok(new { success = true, componentepropiedades = stcomponentepropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ComponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Componente Propiedades - Visualizar")]
        public IActionResult ComponentePropiedadesTotalDisponibles([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                String idsPropiedades = value.idspropiedades != null ? (string)value.idspropiedades : "0";
                int numeroComponentePropiedad = value.numerocomponentepropiedades != null ? (int)value.numerocomponentepropiedades : 0;
                List<ComponentePropiedad> componentepropiedades = ComponentePropiedadDAO.getComponentePropiedadPaginaTotalDisponibles(pagina, numeroComponentePropiedad, idsPropiedades);
                List<stcomponentepropiedad> stcomponentepropiedad = new List<stcomponentepropiedad>();
                foreach (ComponentePropiedad componentepropiedad in componentepropiedades)
                {
                    stcomponentepropiedad temp = new stcomponentepropiedad();
                    temp.id = componentepropiedad.id;
                    temp.nombre = componentepropiedad.nombre;
                    temp.descripcion = componentepropiedad.descripcion;
                    componentepropiedad.datoTipos = DatoTipoDAO.getDatoTipo(componentepropiedad.datoTipoid);
                    temp.datoTipoid = componentepropiedad.datoTipoid;
                    temp.datoTipoNombre = componentepropiedad.datoTipos.nombre;
                    temp.fechaActualizacion = componentepropiedad.fechaActualizacion != null ? componentepropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = componentepropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = componentepropiedad.usuarioActualizo;
                    temp.usuarioCreo = componentepropiedad.usuarioCreo;
                    temp.estado = componentepropiedad.estado;
                    stcomponentepropiedad.Add(temp);
                }

                return Ok(new { success = true, componentepropiedades = stcomponentepropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("3", "ComponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        [Authorize("Componente Propiedades - Visualizar")]
        public IActionResult NumeroComponentePropiedadesDisponibles([FromBody]dynamic value)
        {
            try
            {
                long total = ComponentePropiedadDAO.getTotalComponentePropiedades();
                return Ok(new { success = true, totalcomponentepropiedades = total });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ComponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Componente Propiedades - Visualizar")]
        public IActionResult NumeroComponentePropiedades([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                long total = ComponentePropiedadDAO.getTotalComponentePropiedad(filtro_busqueda);
                return Ok(new { success = true, totalcomponentepropiedades = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "ComponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Componente Propiedades - Crear")]
        public IActionResult ComponentePropiedad([FromBody]dynamic value)
        {
            try
            {
                ComponentePropiedadValidator validator = new ComponentePropiedadValidator();
                ValidationResult results = validator.Validate(value);
                if (results.IsValid)
                {
                    String nombre = value.nombre;
                    String descripcion = value.descripcion;
                    int datoTipoId = (int)value.datoTipoid;

                    ComponentePropiedad componentePropiedad = new ComponentePropiedad();
                    componentePropiedad.nombre = nombre;
                    componentePropiedad.usuarioCreo = User.Identity.Name;
                    componentePropiedad.fechaCreacion = DateTime.Now;
                    componentePropiedad.estado = 1;
                    componentePropiedad.descripcion = descripcion;
                    componentePropiedad.datoTipoid = datoTipoId;

                    bool result = ComponentePropiedadDAO.guardarComponentePropiedad(componentePropiedad);

                    return Ok(new
                    {
                        success = result,
                        id = componentePropiedad.id,
                        usuarioCreo = componentePropiedad.usuarioCreo,
                        fechaCreacion = componentePropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioActualizo = componentePropiedad.usuarioActualizo,
                        fechaActualizacion = componentePropiedad.fechaActualizacion != null ? componentePropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("6", "ComponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Componente Propiedades - Editar")]
        public IActionResult ComponentePropiedad(int id, [FromBody]dynamic value)
        {
            try
            {
                ComponentePropiedadValidator validator = new ComponentePropiedadValidator();
                ValidationResult results = validator.Validate(value);
                if (results.IsValid)
                {
                    String nombre = value.nombre;
                    String descripcion = value.descripcion;
                    int datoTipoId = (int)value.datoTipoid;

                    ComponentePropiedad componentePropiedad = ComponentePropiedadDAO.getComponentePropiedadPorId(id);
                    componentePropiedad.nombre = nombre;
                    componentePropiedad.usuarioActualizo = User.Identity.Name;
                    componentePropiedad.fechaActualizacion = DateTime.Now;
                    componentePropiedad.descripcion = descripcion;
                    componentePropiedad.datoTipoid = datoTipoId;

                    bool result = ComponentePropiedadDAO.guardarComponentePropiedad(componentePropiedad);

                    return Ok(new
                    {
                        success = result,
                        id = componentePropiedad.id,
                        usuarioCreo = componentePropiedad.usuarioCreo,
                        fechaCreacion = componentePropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioActualizo = componentePropiedad.usuarioActualizo,
                        fechaActualizacion = componentePropiedad.fechaActualizacion != null ? componentePropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("7", "ComponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Componente Propiedades - Eliminar")]
        public IActionResult ComponentePropiedad(int id)
        {
            try
            {
                ComponentePropiedad componentePropiedad = ComponentePropiedadDAO.getComponentePropiedadPorId(id);
                componentePropiedad.usuarioActualizo = User.Identity.Name;
                componentePropiedad.fechaActualizacion = DateTime.Now;
                bool eliminado = ComponentePropiedadDAO.eliminarComponentePropiedad(componentePropiedad);
                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("8", "ComponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{idComponente}/{idComponenteTipo}")]
        [Authorize("Componente Propiedades - Visualizar")]
        public IActionResult ComponentePropiedadPorTipo(int idComponente, int idComponenteTipo)
        {
            try
            {
                List<ComponentePropiedad> compoentepropiedades = ComponentePropiedadDAO.getComponentePropiedadesPorTipoComponente(idComponenteTipo);

                List<Dictionary<String, Object>> campos = new List<Dictionary<String, Object>>();
                foreach (ComponentePropiedad componentepropiedad in compoentepropiedades)
                {
                    Dictionary<String, Object> campo = new Dictionary<String, Object>();
                    campo.Add("id", componentepropiedad.id);
                    campo.Add("nombre", componentepropiedad.nombre);
                    campo.Add("tipo", componentepropiedad.datoTipoid);
                    ComponentePropiedadValor coomponentePropiedadValor = ComponentePropiedadValorDAO.getValorPorComponenteYPropiedad(componentepropiedad.id, idComponente);
                    if (coomponentePropiedadValor != null)
                    {
                        switch (componentepropiedad.datoTipoid)
                        {
                            case 1:
                                campo.Add("valor", coomponentePropiedadValor.valorString);
                                break;
                            case 2:
                                campo.Add("valor", coomponentePropiedadValor.valorEntero);
                                break;
                            case 3:
                                campo.Add("valor", coomponentePropiedadValor.valorDecimal);
                                break;
                            case 4:
                                campo.Add("valor", coomponentePropiedadValor.valorEntero == 1 ? true : false);
                                break;
                            case 5:
                                campo.Add("valor", coomponentePropiedadValor.valorTiempo != null ? coomponentePropiedadValor.valorTiempo.Value.ToString("dd/MM/yyyy H:mm:ss") : null);
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

                return Ok(new { success = true, componentepropiedades = estructuraCamposDinamicos });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ComponentePropiedadController.class", e);
                return BadRequest(500);
            }
        }
    }
}
