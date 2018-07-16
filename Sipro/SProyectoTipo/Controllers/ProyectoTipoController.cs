using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;
using FluentValidation.Results;
using SProyectoTipo.Validators;

namespace SProyectoTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ProyectoTipoController : Controller
    {
        private class stproyectotipo
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
        [Authorize("Préstamo o Proyecto Tipos - Visualizar")]
        public IActionResult ProyectoTipoPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                int numeroProyectoTipos = value.numeroproyectotipo != null ? (int)value.numeroproyectotipo : default(int);
                string filtro_busqueda = value.filtro_busqueda != null ? value.filtro_busqueda : default(string);
                string columnaOrdenada = value.columnaOrdenada != null ? (string)value.columnaOrdenada : default(string);
                string ordenDireccion = value.ordenDireccion != null ? (string)value.ordenDireccion : default(string);

                List<ProyectoTipo> proyectotipos = ProyectoTipoDAO.getProyectosTipoPagina(pagina, numeroProyectoTipos, filtro_busqueda, columnaOrdenada, ordenDireccion);
                List<stproyectotipo> stcooperantes = new List<stproyectotipo>();

                foreach (ProyectoTipo proyectotipo in proyectotipos)
                {
                    stproyectotipo temp = new stproyectotipo();
                    temp.id = proyectotipo.id;
                    temp.nombre = proyectotipo.nombre;
                    temp.descripcion = proyectotipo.descripcion;

                    temp.estado = proyectotipo.estado;
                    temp.fechaActualizacion = proyectotipo.fechaActualizacion != null ? proyectotipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = proyectotipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = proyectotipo.usuarioActualizo;
                    temp.usuarioCreo = proyectotipo.usarioCreo;
                    stcooperantes.Add(temp);
                }

                return Ok(new { success = true, proyectotipos = stcooperantes });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProyectoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Préstamo o Proyecto Tipos - Visualizar")]
        public IActionResult NumeroProyectoTipos([FromBody]dynamic value)
        {
            try
            {
                string filtro_busqueda = value.filtro_busqueda != null ? value.filtro_busqueda : default(string);
                long total = ProyectoTipoDAO.getTotalProyectoTipos(filtro_busqueda);

                return Ok(new { success = true, totalproyectotipos = total });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProyectoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Préstamo o Proyecto Tipos - Crear")]
        public IActionResult Proyectotipo([FromBody]dynamic value)
        {
            try
            {
                ProyectoTipoValidator validator = new ProyectoTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    ProyectoTipo proyectoTipo = new ProyectoTipo();
                    proyectoTipo.nombre = value.nombre;
                    proyectoTipo.descripcion = value.descripcion;
                    proyectoTipo.fechaCreacion = DateTime.Now;
                    proyectoTipo.usarioCreo = User.Identity.Name;
                    proyectoTipo.estado = 1;

                    bool guardado = ProyectoTipoDAO.guardarProyectoTipo(proyectoTipo);

                    if (guardado)
                    {
                        string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                        String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;
                        if (idsPropiedades != null && idsPropiedades.Length > 0)
                        {
                            foreach (String idPropiedad in idsPropiedades)
                            {
                                PtipoPropiedad ptipoPropiedad = new PtipoPropiedad();
                                ptipoPropiedad.proyectoTipoid = proyectoTipo.id;
                                ptipoPropiedad.proyectoPropiedadid = Convert.ToInt32(idPropiedad);
                                ptipoPropiedad.fechaCreacion = DateTime.Now;
                                ptipoPropiedad.usuarioCreo = User.Identity.Name;
                                ptipoPropiedad.estado = 1;

                                guardado = guardado & PtipoPropiedadDAO.guardarPtipoPropiedad(ptipoPropiedad);
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = guardado,
                        id = proyectoTipo.id,
                        usuarioCreo = proyectoTipo.usarioCreo,
                        fechaCreacion = proyectoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioActualizo = proyectoTipo.usuarioActualizo,
                        fechaActualizacion = proyectoTipo.fechaActualizacion != null ? proyectoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Préstamo o Proyecto Tipos - Editar")]
        public IActionResult Proyectotipo(int id, [FromBody]dynamic value)
        {
            try
            {
                ProyectoTipoValidator validator = new ProyectoTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    ProyectoTipo proyectoTipo = ProyectoTipoDAO.getProyectoTipoPorId(id);
                    proyectoTipo.nombre = value.nombre;
                    proyectoTipo.descripcion = value.descripcion;
                    proyectoTipo.fechaActualizacion = DateTime.Now;
                    proyectoTipo.usuarioActualizo = User.Identity.Name;

                    List<PtipoPropiedad> propiedades_temp = PtipoPropiedadDAO.getPtipoPropiedades(proyectoTipo.id);

                    if (propiedades_temp != null)
                    {
                        foreach (PtipoPropiedad ptipoPropiedad in propiedades_temp)
                        {
                            PtipoPropiedadDAO.eliminarTotalPtipoPropiedad(ptipoPropiedad);
                        }
                    }

                    bool guardado = ProyectoTipoDAO.guardarProyectoTipo(proyectoTipo);

                    if (guardado)
                    {
                        string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                        String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;
                        if (idsPropiedades != null && idsPropiedades.Length > 0)
                        {
                            foreach (String idPropiedad in idsPropiedades)
                            {
                                PtipoPropiedad ptipoPropiedad = new PtipoPropiedad();
                                ptipoPropiedad.proyectoTipoid = proyectoTipo.id;
                                ptipoPropiedad.proyectoPropiedadid = Convert.ToInt32(idPropiedad);
                                ptipoPropiedad.fechaCreacion = DateTime.Now;
                                ptipoPropiedad.usuarioCreo = User.Identity.Name;
                                ptipoPropiedad.estado = 1;

                                guardado = guardado & PtipoPropiedadDAO.guardarPtipoPropiedad(ptipoPropiedad);
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = guardado,
                        id = proyectoTipo.id,
                        usuarioCreo = proyectoTipo.usarioCreo,
                        fechaCreacion = proyectoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioActualizo = proyectoTipo.usuarioActualizo,
                        fechaActualizacion = proyectoTipo.fechaActualizacion != null ? proyectoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProyectoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Préstamo o Proyecto Tipos - Eliminar")]
        public IActionResult ProyectoTipo(int id)
        {
            try
            {
                ProyectoTipo proyectoTipo = ProyectoTipoDAO.getProyectoTipoPorId(id);
                bool eliminado = ProyectoTipoDAO.eliminarProyectoTipo(proyectoTipo);

                return Ok(new { success = eliminado });

            }
            catch (Exception e)
            {
                CLogger.write("5", "ProyectoTipoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
