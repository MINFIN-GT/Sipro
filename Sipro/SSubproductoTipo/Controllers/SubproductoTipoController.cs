using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SSubproductoTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class SubproductoTipoController : Controller
    {
        private class StSubproductoTipo
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
        [Authorize("Subproducto Tipos - Visualizar")]
        public IActionResult SubproductoTipoPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int registros = value.registros != null ? (int)value.registros : 20;
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;

                List<SubproductoTipo> subproductoTipos = SubproductoTipoDAO.getPagina(pagina, registros, filtro_busqueda, columna_ordenada, orden_direccion);
                List<StSubproductoTipo> lstStSubproductoTipos = new List<StSubproductoTipo>();

                foreach (SubproductoTipo subproductoTipo in subproductoTipos)
                {
                    StSubproductoTipo temp = new StSubproductoTipo();
                    temp.id = subproductoTipo.id;
                    temp.nombre = subproductoTipo.nombre;
                    temp.descripcion = subproductoTipo.descripcion;
                    temp.usuarioCreo = subproductoTipo.usuarioCreo;
                    temp.usuarioActualizo = subproductoTipo.usuarioActualizo;
                    temp.estado = subproductoTipo.estado;
                    temp.fechaCreacion = subproductoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = subproductoTipo.fechaActualizacion != null ? subproductoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    lstStSubproductoTipos.Add(temp);
                }

                return Ok(new { success = true, subproductoTipos = lstStSubproductoTipos });
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubproductoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subproducto Tipos - Crear")]
        public IActionResult SubproductoTipo([FromBody]dynamic value)
        {
            try
            {
                SubproductoTipoValidator validator = new SubproductoTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    SubproductoTipo subproductoTipo = new SubproductoTipo();
                    subproductoTipo.nombre = value.nombre;
                    subproductoTipo.descripcion = value.descripcion;
                    subproductoTipo.estado = 1;
                    subproductoTipo.fechaCreacion = DateTime.Now;
                    subproductoTipo.usuarioCreo = User.Identity.Name;

                    bool guardado = SubproductoTipoDAO.guardarSubproductoTipo(subproductoTipo);

                    if (guardado)
                    {
                        string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                        String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;

                        if (idsPropiedades != null && idsPropiedades.Length > 0)
                        {
                            foreach (String idPropiedad in idsPropiedades)
                            {
                                SubprodtipoPropiedad subprodtipoPropiedad = new SubprodtipoPropiedad();
                                subprodtipoPropiedad.subproductoTipoid = subproductoTipo.id;
                                subprodtipoPropiedad.subproductoPropiedadid = Convert.ToInt32(idPropiedad);
                                subprodtipoPropiedad.fechaCreacion = DateTime.Now;
                                subprodtipoPropiedad.usuarioCreo = User.Identity.Name;

                                guardado = guardado & SubprodTipoPropiedadDAO.guardarSubproductoTipoPropiedad(subprodtipoPropiedad);
                            }
                        }

                        return Ok(new
                        {
                            success = guardado,
                            id = subproductoTipo.id,
                            usuarioCreo = subproductoTipo.usuarioCreo,
                            usuarioActualizo = subproductoTipo.usuarioActualizo,
                            fechaCreacion = subproductoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                            fechaActualizacion = subproductoTipo.fechaActualizacion != null ? subproductoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
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
                CLogger.write("2", "SubproductoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Subproducto Tipos - Editar")]
        public IActionResult SubproductoTipo(int id, [FromBody]dynamic value)
        {
            try
            {
                SubproductoTipoValidator validator = new SubproductoTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    SubproductoTipo subproductoTipo = SubproductoTipoDAO.getSubproductoTipo(id);
                    subproductoTipo.nombre = value.nombre;
                    subproductoTipo.descripcion = value.descripcion;
                    subproductoTipo.fechaActualizacion = DateTime.Now;
                    subproductoTipo.usuarioActualizo = User.Identity.Name;

                    bool guardado = SubproductoTipoDAO.guardarSubproductoTipo(subproductoTipo);

                    if (guardado)
                    {
                        string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                        String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;

                        if (idsPropiedades != null && idsPropiedades.Length > 0)
                        {
                            foreach (String idPropiedad in idsPropiedades)
                            {
                                SubprodtipoPropiedad subprodtipoPropiedad = new SubprodtipoPropiedad();
                                subprodtipoPropiedad.subproductoTipoid = subproductoTipo.id;
                                subprodtipoPropiedad.subproductoPropiedadid = Convert.ToInt32(idPropiedad);
                                subprodtipoPropiedad.fechaCreacion = DateTime.Now;
                                subprodtipoPropiedad.usuarioCreo = User.Identity.Name;

                                guardado = guardado & SubprodTipoPropiedadDAO.guardarSubproductoTipoPropiedad(subprodtipoPropiedad);
                            }
                        }

                        return Ok(new
                        {
                            success = guardado,
                            id = subproductoTipo.id,
                            usuarioCreo = subproductoTipo.usuarioCreo,
                            usuarioActualizo = subproductoTipo.usuarioActualizo,
                            fechaCreacion = subproductoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                            fechaActualizacion = subproductoTipo.fechaActualizacion != null ? subproductoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
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
                CLogger.write("3", "SubproductoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Subproducto Tipos - Eliminar")]
        public IActionResult SubproductoTipo(int id)
        {
            try
            {
                SubproductoTipo subproductoTipo = SubproductoTipoDAO.getSubproductoTipo(id);
                subproductoTipo.usuarioActualizo = User.Identity.Name;
                bool eliminado = SubproductoTipoDAO.eliminarSubproductoTipo(subproductoTipo);
                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubproductoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subproducto Tipos - Visualizar")]
        public IActionResult TotalElementos([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda;
                long total = SubproductoTipoDAO.getTotal(filtro_busqueda);
                return Ok(new { success = true, total = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubproductoTipoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
