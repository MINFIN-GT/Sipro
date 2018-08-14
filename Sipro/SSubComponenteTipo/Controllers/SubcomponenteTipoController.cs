using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SSubComponenteTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class SubcomponenteTipoController : Controller
    {
        private class Stsubcomponentetipo
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
        [Authorize("Subcomponentes Tipos - Visualizar")]
        public IActionResult SubComponentetiposPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int numeroSubComponenteTipo = value.numerosubcomponentetipos != null ? (int)value.numerosubcomponentetipos : 20;
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;
                List<SubcomponenteTipo> subcomponentetipos = SubComponenteTipoDAO.getSubComponenteTiposPagina(pagina, numeroSubComponenteTipo
                        , filtro_busqueda, columna_ordenada, orden_direccion);
                List<Stsubcomponentetipo> stsubcomponentetipos = new List<Stsubcomponentetipo>();
                foreach (SubcomponenteTipo subcomponentetipo in subcomponentetipos)
                {
                    Stsubcomponentetipo temp = new Stsubcomponentetipo();
                    temp.descripcion = subcomponentetipo.descripcion;
                    temp.estado = subcomponentetipo.estado;
                    temp.fechaActualizacion = subcomponentetipo.fechaActualizacion != null ? subcomponentetipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = subcomponentetipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.id = subcomponentetipo.id;
                    temp.nombre = subcomponentetipo.nombre;
                    temp.usuarioActualizo = subcomponentetipo.usuarioActualizo;
                    temp.usuarioCreo = subcomponentetipo.usuarioCreo;
                    stsubcomponentetipos.Add(temp);
                }
                return Ok(new { success = true, subcomponentetipos = stsubcomponentetipos });
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubcomponenteTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subcomponentes Tipos - Visualizar")]
        public IActionResult NumeroSubComponenteTipos([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda;

                long total = SubComponenteTipoDAO.getTotalSubComponenteTipo(filtro_busqueda);

                return Ok(new { success = true, totalsubcomponentetipos = total });
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubcomponenteTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subcomponentes Tipos - Crear")]
        public IActionResult SubComponenteTipo([FromBody]dynamic value)
        {
            try
            {
                SubcomponenteTipoValidator validator = new SubcomponenteTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    SubcomponenteTipo subcomponenteTipo = new SubcomponenteTipo();
                    subcomponenteTipo.nombre = value.nombre;
                    subcomponenteTipo.descripcion = value.descripcion;
                    subcomponenteTipo.usuarioCreo = User.Identity.Name;
                    subcomponenteTipo.fechaCreacion = DateTime.Now;
                    subcomponenteTipo.estado = 1;

                    bool guardado = false;
                    guardado = SubComponenteTipoDAO.guardarSubComponenteTipo(subcomponenteTipo);

                    if (guardado)
                    {
                        string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                        String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;

                        if (idsPropiedades != null && idsPropiedades.Length > 0)
                        {
                            foreach (String idPropiedad in idsPropiedades)
                            {
                                SctipoPropiedad sctipoPropiedad = new SctipoPropiedad();
                                sctipoPropiedad.subcomponenteTipoid = subcomponenteTipo.id;
                                sctipoPropiedad.subcomponentePropiedadid = Convert.ToInt32(idPropiedad);
                                sctipoPropiedad.fechaCreacion = DateTime.Now;
                                sctipoPropiedad.usuarioCreo = User.Identity.Name;

                                guardado = guardado & SctipoPropiedadDAO.guardarSctipoPropiedad(sctipoPropiedad);
                            }
                        }

                        return Ok(new
                        {
                            success = guardado,
                            id = subcomponenteTipo.id,
                            usuarioCreo = subcomponenteTipo.usuarioCreo,
                            fechaCreacion = subcomponenteTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                            usuarioActualizo = subcomponenteTipo.usuarioActualizo,
                            fechaActualizacion = subcomponenteTipo.fechaActualizacion != null ? subcomponenteTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
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
                CLogger.write("3", "SubcomponenteTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Subcomponentes Tipos - Editar")]
        public IActionResult SubComponenteTipo(int id, [FromBody]dynamic value)
        {
            try
            {
                SubcomponenteTipoValidator validator = new SubcomponenteTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    SubcomponenteTipo subcomponenteTipo = SubComponenteTipoDAO.getSubComponenteTipoPorId(id);
                    subcomponenteTipo.nombre = value.nombre;
                    subcomponenteTipo.descripcion = value.descripcion;
                    subcomponenteTipo.usuarioActualizo = User.Identity.Name;
                    subcomponenteTipo.fechaActualizacion = DateTime.Now;

                    bool guardado = false;
                    guardado = SubComponenteTipoDAO.guardarSubComponenteTipo(subcomponenteTipo);

                    if (guardado)
                    {
                        List<SctipoPropiedad> propiedades_temp = SctipoPropiedadDAO.getSctipoPropiedades(subcomponenteTipo.id);

                        if (propiedades_temp != null)
                        {
                            foreach (SctipoPropiedad sctipoPropiedad in propiedades_temp)
                            {
                                guardado = guardado & SctipoPropiedadDAO.eliminarTotalSctipoPropiedad(sctipoPropiedad);
                            }
                        }

                        if (guardado)
                        {
                            string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                            String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;

                            if (idsPropiedades != null && idsPropiedades.Length > 0)
                            {
                                foreach (String idPropiedad in idsPropiedades)
                                {
                                    SctipoPropiedad sctipoPropiedad = new SctipoPropiedad();
                                    sctipoPropiedad.subcomponenteTipoid = subcomponenteTipo.id;
                                    sctipoPropiedad.subcomponentePropiedadid = Convert.ToInt32(idPropiedad);
                                    sctipoPropiedad.fechaCreacion = DateTime.Now;
                                    sctipoPropiedad.usuarioCreo = User.Identity.Name;

                                    guardado = guardado & SctipoPropiedadDAO.guardarSctipoPropiedad(sctipoPropiedad);
                                }
                            }

                            return Ok(new
                            {
                                success = guardado,
                                id = subcomponenteTipo.id,
                                usuarioCreo = subcomponenteTipo.usuarioCreo,
                                fechaCreacion = subcomponenteTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                                usuarioActualizo = subcomponenteTipo.usuarioActualizo,
                                fechaActualizacion = subcomponenteTipo.fechaActualizacion != null ? subcomponenteTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                            });
                        }
                        else
                            return Ok(new { success = false });
                    }
                    else
                        return Ok(new { success = false });
                }
                else
                    return Ok(new { success = false });
                   
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubcomponenteTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Subcomponentes Tipos - Eliminar")]
        public IActionResult SubComponenteTipo(int id)
        {
            try
            {
                SubcomponenteTipo subcomponenteTipo = SubComponenteTipoDAO.getSubComponenteTipoPorId(id);
                subcomponenteTipo.usuarioActualizo = User.Identity.Name;
                bool eliminado = SubComponenteTipoDAO.eliminarSubComponenteTipo(subcomponenteTipo);

                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubcomponenteTipoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
