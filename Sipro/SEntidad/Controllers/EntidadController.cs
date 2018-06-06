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
                string filtro_entidad = value.filtro_entidad != null ? (string)value.filtro_entidad : default(string);
                string filtro_nombre = value.filtro_nombre != null ? (string)value.filtro_nombre : default(string);
                string filtro_abreviatura = value.filtro_abreviatura != null ? (string)value.filtro_abreviatura : default(string);
                string columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : default(string);
                string orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : default(string);

                List <Entidad> lstentidades = EntidadDAO.getEntidadesPagina(pagina, registros, filtro_entidad, filtro_nombre, filtro_abreviatura, 
                    columna_ordenada, orden_direccion);

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

                    if (creado)
                    {
                        int pagina = value.pagina != null ? (int)value.pagina : default(int);
                        int registros = value.registros != null ? (int)value.registros : default(int);
                        string filtro_entidad = value.filtro_entidad != null ? (string)value.filtro_entidad : default(string);
                        string filtro_nombre = value.filtro_nombre != null ? (string)value.filtro_nombre : default(string);
                        string filtro_abreviatura = value.filtro_abreviatura != null ? (string)value.filtro_abreviatura : default(string);
                        string columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : default(string);
                        string orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : default(string);

                        List<Entidad> lstentidades = EntidadDAO.getEntidadesPagina(pagina, registros, filtro_entidad, filtro_nombre,
                            filtro_abreviatura, columna_ordenada, orden_direccion);

                        return Ok(new { success = true, entidades = lstentidades });
                    }
                    else
                        return Ok(new { success = false });
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
        [HttpPost]
        [Authorize("Entidades - Editar")]
        public IActionResult EntidadA([FromBody]dynamic value)
        {
            try
            {
                EntidadValidator validator = new EntidadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    int entidad = value.entidad != null ? (int)value.entidad : default(int);
                    int ejercicio = value.ejercicio != null ? (int)value.ejercicio : default(int);
                    string abreviatura = value.abreviatura != null ? (string)value.abreviatura : default(string);

                    bool actualizado = EntidadDAO.guardarEntidad(entidad, ejercicio, null, abreviatura);

                    if (actualizado)
                    {
                        int pagina = value.pagina != null ? (int)value.pagina : default(int);
                        int registros = value.registros != null ? (int)value.registros : default(int);
                        string filtro_entidad = value.filtro_entidad != null ? (string)value.filtro_entidad : default(string);
                        string filtro_nombre = value.filtro_nombre != null ? (string)value.filtro_nombre : default(string);
                        string filtro_abreviatura = value.filtro_abreviatura != null ? (string)value.filtro_abreviatura : default(string);
                        string columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : default(string);
                        string orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : default(string);

                        List<Entidad> lstentidades = EntidadDAO.getEntidadesPagina(pagina, registros, filtro_entidad, filtro_nombre, filtro_abreviatura,
                            columna_ordenada, orden_direccion);

                        return Ok(new { success = true, entidades = lstentidades });
                    }
                    else
                        return Ok(new { success = false });
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
                string filtro_entidad = value.filtro_entidad != null ? (string)value.filtro_entidad : default(string);
                string filtro_nombre = value.filtro_nombre != null ? (string)value.filtro_nombre : default(string);
                string filtro_abreviatura = value.filtro_abreviatura != null ? (string)value.filtro_abreviatura : default(string);

                long total = EntidadDAO.getTotalEntidades(filtro_entidad, filtro_nombre, filtro_abreviatura);

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
