using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;
using FluentValidation.Results;

namespace SUnidadEjecutora.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class UnidadEjecutoraController : Controller
    {
        private class EstructuraEntidad
        {
            public int ejercicio;
            public int entidad;
            public String nombre;
            public String abreviatura;
            public int unidadEjecutora;
        }

        // POST api/values
        [HttpPost]
        [Authorize("Unidades Ejecutoras - Visualizar")]
        public IActionResult UnidadEjecutoras([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                int registros = value.registros != null ? (int)value.registros : default(int);
                int ejercicio = value.ejercicio != null ? (int)value.ejercicio : default(int);
                int ventidad = value.entidad != null ? (int)value.entidad : default(int);
                string filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;

                List <UnidadEjecutora> lstunidadejecutora = UnidadEjecutoraDAO.getPagina(pagina, registros, ejercicio, ventidad, filtro_busqueda);

                if (lstunidadejecutora != null)
                {
                    List<EstructuraEntidad> lstEstructuraEntidad = new List<EstructuraEntidad>();
                    foreach (UnidadEjecutora unidadEjecutora in lstunidadejecutora)
                    {
                        EstructuraEntidad estructuraEntidad = new EstructuraEntidad();
                        estructuraEntidad.entidad = unidadEjecutora.entidadentidad;
                        estructuraEntidad.ejercicio = unidadEjecutora.ejercicio;
                        estructuraEntidad.nombre = unidadEjecutora.nombre;
                        Entidad entidad = EntidadDAO.getEntidad(unidadEjecutora.entidadentidad, unidadEjecutora.ejercicio);
                        estructuraEntidad.abreviatura = entidad.abreviatura;
                        estructuraEntidad.unidadEjecutora = unidadEjecutora.unidadEjecutora;
                        lstEstructuraEntidad.Add(estructuraEntidad);
                    }

                    return Ok(new { success = true, unidadesEjecutoras = lstEstructuraEntidad });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("1", "UnidadEjecutoraController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/values
        [HttpPost]
        [Authorize("Unidades Ejecutoras - Crear")]
        public IActionResult UnidadEjecutora([FromBody]dynamic value)
        {
            try
            {
                UnidadEjecutoraValidator validator = new UnidadEjecutoraValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    int ejercicio = value.ejercicio;
                    int entidad = value.entidad;
                    int id = value.unidadEjecutora;
                    string nombre = value.nombre;

                    bool guardado = UnidadEjecutoraDAO.guardar(entidad, ejercicio, id, nombre);
                    return Ok(new { success = guardado });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("2", "UnidadEjecutoraController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/values
        [HttpPut("{unidadEjecutora}")]
        [Authorize("Unidades Ejecutoras - Editar")]
        public IActionResult UnidadEjecutora(int unidadEjecutora, [FromBody]dynamic value)
        {
            try
            {
                UnidadEjecutoraValidator validator = new UnidadEjecutoraValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    int ejercicio = value.ejercicio;
                    int entidad = value.entidad;
                    int id = unidadEjecutora;
                    string nombre = value.nombre;

                    bool guardado = UnidadEjecutoraDAO.actualizar(entidad, ejercicio, id, nombre);
                    return Ok(new { success = guardado });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "UnidadEjecutoraController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/values
        [HttpPost]
        [Authorize("Unidades Ejecutoras - Visualizar")]
        public IActionResult TotalElementos([FromBody]dynamic value)
        {
            try
            {
                int ejercicio = value.ejercicio != null ? (int)value.ejercicio : default(int);
                int entidad = value.entidad != null ? (int)value.entidad : default(int);
                string filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;

                long total = UnidadEjecutoraDAO.getTotal(ejercicio, entidad, filtro_busqueda);
                return Ok(new { success = true, total = total });
            }
            catch (Exception e)
            {
                CLogger.write("4", "UnidadEjecutoraController.class", e);
                return BadRequest(500);
            }
        }        
    }
}