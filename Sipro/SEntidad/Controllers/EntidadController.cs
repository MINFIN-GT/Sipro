using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;
using FluentValidation.Results;

namespace SEntidad.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class EntidadController : Controller
    {
        // POST api/Entidad/Entidades
        [HttpPost]
        [Authorize("Entidades - Visualizar")]
        public IActionResult Entidades([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                int registros = value.registros != null ? (int)value.registros : default(int);
                string filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;                
                string columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : default(string);
                string orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : default(string);

                List <Entidad> lstentidades = EntidadDAO.getEntidadesPagina(pagina, registros, filtro_busqueda, columna_ordenada, orden_direccion);

                return Ok(new { success = lstentidades != null ? true : false, entidades = lstentidades });
            }
            catch (Exception e)
            {
                CLogger.write("1", "EntidadController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/Entidad/EntidadesPorEjercicio/ejercicio
        [HttpGet("{ejercicio}")]
        [Authorize("Entidades - Visualizar")]
        public IActionResult EntidadesPorEjercicio(int ejercicio)
        {
            try
            {
                List<Entidad> lstentidades = EntidadDAO.getEntidades(ejercicio);

                return Ok(new { success = lstentidades != null ? true : false, entidades = lstentidades });
            }
            catch (Exception e)
            {
                CLogger.write("2", "EntidadController.class", e);
                return BadRequest(500); 
            }
        }

        // POST api/Entidad/Entidad
        [HttpPost]
        [Authorize("Entidades - Crear")]
        public IActionResult Entidad([FromBody]dynamic value)
        {
            try
            {
                EntidadValidator validator = new EntidadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    int entidad = value.entidad != null ? (int)value.entidad : default(int);
                    int ejercicio = value.ejercicio != null ? (int)value.ejercicio : default(int);
                    string nombre = value.nombre != null ? (string)value.nombre : default(string);
                    string abreviatura = value.abreviatura != null ? (string)value.abreviatura : default(string);

                    bool creado = EntidadDAO.guardarEntidad(entidad, ejercicio, nombre, abreviatura);
                    return Ok(new { success = creado });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "EntidadController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Entidad/Entidad
        [HttpGet("{entidad}")]
        [Authorize("Entidades - Editar")]
        public IActionResult Entidad(int entidad, [FromBody]dynamic value)
        {
            try
            {
                EntidadValidator validator = new EntidadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    int ejercicio = value.ejercicio != null ? (int)value.ejercicio : default(int);
                    string abreviatura = value.abreviatura != null ? (string)value.abreviatura : default(string);

                    bool actualizado = EntidadDAO.guardarEntidad(entidad, ejercicio, null, abreviatura);
                    return Ok(new { success = actualizado });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("4", "EntidadController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Entidad/totalEntidades
        [HttpPost]
        [Authorize("Entidades - Visualizar")]
        public IActionResult totalEntidades([FromBody]dynamic value)
        {
            try
            {
                string filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : default(string);

                long total = EntidadDAO.getTotalEntidades(filtro_busqueda);

                return Ok(new { success = true, total = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "EntidadController.class", e);
                return BadRequest(500);
            }
        }
    }
}
