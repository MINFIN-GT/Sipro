using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;

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
                List<Entidad> lstentidades = EntidadDAO.getEntidadesPagina((int)value.pagina, (int)value.registros, (string)value.filtro_entidad, (string)value.filtro_nombre,
                    (string)value.filtro_abreviatura, (string)value.columna_ordenada, (string)value.orden_direccion);

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
                bool creado = EntidadDAO.guardarEntidad((int)value.entidad, (int)value.ejercicio, (string)value.nombre, (string)value.abreviatura);

                if (creado)
                {
                    List<Entidad> lstentidades = EntidadDAO.getEntidadesPagina((int)value.pagina, (int)value.registros, (string)value.filtro_entidad, (string)value.filtro_nombre,
                    (string)value.filtro_abreviatura, (string)value.columna_ordenada, (string)value.orden_direccion);

                    return Ok(new { success = true, entidades = lstentidades });
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
                bool actualizado = EntidadDAO.guardarEntidad((int)value.entidad, (int)value.ejercicio, null,(string)value.abreviatura);

                if (actualizado)
                {
                    List<Entidad> lstentidades = EntidadDAO.getEntidadesPagina((int)value.pagina, (int)value.registros, (string)value.filtro_entidad, (string)value.filtro_nombre,
                    (string)value.filtro_abreviatura, (string)value.columna_ordenada, (string)value.orden_direccion);

                    return Ok(new { success = true, entidades = lstentidades });
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
                long total = EntidadDAO.getTotalEntidades((string)value.filtro_entidad, (string)value.filtro_nombre, (string)value.filtro_abreviatura);

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
