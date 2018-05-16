using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;

namespace SUnidadEjecutora.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    public class UnidadEjecutoraController : Controller
    {
        private class EstructuraEntidad
        {
            public int ejercicio;
            public int entidad;
            public String nombre;
            public String abreviatura;
        }

        // POST api/values
        [HttpPost]
        [Authorize("Unidades Ejecutoras - Visualizar")]
        public IActionResult UnidadEjecutoras([FromBody]dynamic value)
        {
            try
            {
                List<UnidadEjecutora> lstunidadejecutora = UnidadEjecutoraDAO.getPagina((int)value.pagina, (int)value.registros, (int)value.ejercicio, (int)value.entidad);

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
        public IActionResult UnidadEjecutoraC([FromBody]dynamic value)
        {
            try
            {
                bool guardado = UnidadEjecutoraDAO.guardar((int)value.entidad, (int)value.ejercicio, (int)value.id, (string)value.nombre);
                return Ok(new { success = guardado });
            }
            catch (Exception e)
            {
                CLogger.write("2", "UnidadEjecutoraController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/values
        [HttpPut]
        [Authorize("Unidades Ejecutoras - Editar")]
        public IActionResult UnidadEjecutoraA([FromBody]dynamic value)
        {
            try
            {
                bool guardado = UnidadEjecutoraDAO.actualizar((int)value.entidad, (int)value.ejercicio, (int)value.id, (string)value.nombre);
                return Ok(new { success = guardado });
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
        public IActionResult Total([FromBody]dynamic value)
        {
            try
            {
                long total = UnidadEjecutoraDAO.getTotal((int)value.ejercicio, (int)value.entidad);
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