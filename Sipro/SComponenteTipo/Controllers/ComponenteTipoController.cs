using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace SComponenteTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ComponenteTipoController : Controller
    {
        private class stcomponentetipo
        {
            public int id;
            public String nombre;
            public String descripcion;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }

        [HttpPost]
        [Authorize("Componente Tipos - Visualizar")]
        public IActionResult ComponentetiposPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int numeroComponenteTipo = value.numerocomponentetipos != null ? (int)value.numerocomponentetipos : 20;
                String filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                String columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : null;
                String orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : null;
                List<ComponenteTipo> componentetipos = ComponenteTipoDAO.getComponenteTiposPagina(pagina, numeroComponenteTipo
                        , filtro_busqueda, columna_ordenada, orden_direccion);
                List<stcomponentetipo> stcomponentetipos = new List<stcomponentetipo>();
                foreach (ComponenteTipo componentetipo in componentetipos)
                {
                    stcomponentetipo temp = new stcomponentetipo();
                    temp.descripcion = componentetipo.descripcion;
                    temp.estado = componentetipo.estado;
                    temp.fechaActualizacion = componentetipo.fechaActualizacion != null ? componentetipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = componentetipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.id = componentetipo.id;
                    temp.nombre = componentetipo.nombre;
                    temp.usuarioActualizo = componentetipo.usuarioActualizo;
                    temp.usuarioCreo = componentetipo.usuarioCreo;
                    stcomponentetipos.Add(temp);
                }

                return Ok(new { success = true, componentetipos = stcomponentetipos });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ComponenteTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Componente Tipos - Visualizar")]
        public IActionResult NumeroComponenteTipos([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                long total = ComponenteTipoDAO.getTotalComponenteTipo(filtro_busqueda);
                
                return Ok(new { success = true, totalcomponentetipos = total });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ComponenteTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Componente Propiedades - Crear")]
        public IActionResult ComponenteTipo([FromBody]dynamic value)
        {
            try
            {
                ComponenteTipoValidator validator = new ComponenteTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    ComponenteTipo componenteTipo = new ComponenteTipo();
                    componenteTipo.nombre = value.nombre;
                    componenteTipo.descripcion = value.descripcion;
                    componenteTipo.fechaCreacion = DateTime.Now;
                    componenteTipo.usuarioCreo = User.Identity.Name;
                    componenteTipo.estado = 1;

                    bool guardado = false;
                    guardado = ComponenteTipoDAO.guardarComponenteTipo(componenteTipo);

                    if (guardado)
                    {
                        string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                        String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;

                        if (idsPropiedades != null && idsPropiedades.Length > 0)
                        {
                            foreach (String idPropiedad in idsPropiedades)
                            {
                                CtipoPropiedad ctipoPropiedad = new CtipoPropiedad();
                                ctipoPropiedad.componenteTipoid = componenteTipo.id;
                                ctipoPropiedad.componentePropiedadid = Convert.ToInt32(idPropiedad);
                                ctipoPropiedad.fechaCreacion = DateTime.Now;
                                ctipoPropiedad.usuarioCreo = User.Identity.Name;

                                guardado = guardado & CtipoPropiedadDAO.guardarCtipoPropiedad(ctipoPropiedad);
                            }
                        }

                        return Ok(new
                        {
                            success = guardado,
                            id = componenteTipo.id,
                            usuarioCreo = componenteTipo.usuarioCreo,
                            fechaCreacion = componenteTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                            usuarioActualizo = componenteTipo.usuarioActualizo,
                            fechaActualizacion = componenteTipo.fechaActualizacion != null ? componenteTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                        });
                    }
                    else
                        return Ok(new { success = false });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "ComponenteTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Componente Tipos - Editar")]
        public IActionResult ComponenteTipo(int id, [FromBody]dynamic value)
        {
            try
            {
                ComponenteTipoValidator validator = new ComponenteTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    ComponenteTipo componenteTipo = ComponenteTipoDAO.getComponenteTipoPorId(id);
                    componenteTipo.nombre = value.nombre;
                    componenteTipo.descripcion = value.descripcion;
                    componenteTipo.fechaActualizacion = DateTime.Now;
                    componenteTipo.usuarioActualizo = User.Identity.Name;

                    bool guardado = false;
                    guardado = ComponenteTipoDAO.guardarComponenteTipo(componenteTipo);

                    if (guardado)
                    {
                        List<CtipoPropiedad> propiedades_temp = CtipoPropiedadDAO.getCtipoPropiedades(componenteTipo.id);

                        if (propiedades_temp != null)
                        {
                            foreach (CtipoPropiedad ctipoPropiedad in propiedades_temp)
                            {
                                guardado = guardado & CtipoPropiedadDAO.eliminarTotalCtipoPropiedad(ctipoPropiedad);
                            }

                            if (guardado)
                            {
                                string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                                String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;

                                if (idsPropiedades != null && idsPropiedades.Length > 0)
                                {
                                    foreach (String idPropiedad in idsPropiedades)
                                    {
                                        CtipoPropiedad ctipoPropiedad = new CtipoPropiedad();
                                        ctipoPropiedad.componenteTipoid = componenteTipo.id;
                                        ctipoPropiedad.componentePropiedadid = Convert.ToInt32(idPropiedad);
                                        ctipoPropiedad.fechaCreacion = DateTime.Now;
                                        ctipoPropiedad.usuarioCreo = User.Identity.Name;

                                        guardado = guardado & CtipoPropiedadDAO.guardarCtipoPropiedad(ctipoPropiedad);
                                    }
                                }                                
                            }
                            else
                                return Ok(new { success = false });
                        }

                        return Ok(new
                        {
                            success = guardado,
                            id = componenteTipo.id,
                            usuarioCreo = componenteTipo.usuarioCreo,
                            fechaCreacion = componenteTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                            usuarioActualizo = componenteTipo.usuarioActualizo,
                            fechaActualizacion = componenteTipo.fechaActualizacion != null ? componenteTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                        });
                    }
                    else
                        return Ok(new { success = false });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ComponenteTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Componente Tipos - Eliminar")]
        public IActionResult ComponenteTipo(int id)
        {
            try
            {
                ComponenteTipo componenteTipo = ComponenteTipoDAO.getComponenteTipoPorId(id);
                componenteTipo.usuarioActualizo = User.Identity.Name;
                bool eliminado = ComponenteTipoDAO.eliminarComponenteTipo(componenteTipo);

                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ComponenteTipoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
