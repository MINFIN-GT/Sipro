using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SActividadPropiedad.Controllers
{
    [Route("api/[controller]")]
    public class ActividadPropiedadController : Controller
    {
        private class Stactividadpropiedad
        {
            public int id;
            public String nombre;
            public String descripcion;
            public int datotipoid;
            public String datotiponombre;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
        }

        [HttpGet("{idActividadTipo}")]
        public IActionResult ActividadPropiedadPaginaPorTipo(int idActividadTipo)
        {
            try
            {
                List<ActividadPropiedad> actividadpropiedades = ActividadPropiedadDAO.getActividadPropiedadesPorTipoActividadPagina(idActividadTipo);
                List<Stactividadpropiedad> stactividadpropiedad = new List<Stactividadpropiedad>();
                foreach (ActividadPropiedad actividadpropiedad in actividadpropiedades)
                {
                    Stactividadpropiedad temp = new Stactividadpropiedad();
                    temp.id = actividadpropiedad.id;
                    temp.nombre = actividadpropiedad.nombre;
                    temp.descripcion = actividadpropiedad.descripcion;

                    actividadpropiedad.datoTipos = DatoTipoDAO.getDatoTipo(actividadpropiedad.datoTipoid);

                    temp.datotipoid = actividadpropiedad.datoTipoid;
                    temp.datotiponombre = actividadpropiedad.datoTipos.nombre;

                    temp.fechaActualizacion = actividadpropiedad.fechaActualizacion != null ? actividadpropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = actividadpropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = actividadpropiedad.usuarioActualizo;
                    temp.usuarioCreo = actividadpropiedad.usuarioCreo;
                    stactividadpropiedad.Add(temp);
                }

                return Ok(new { success = true, actividadpropiedades = stactividadpropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        public IActionResult ActividadPropiedadPagina([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("2", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        public IActionResult ActividadPropiedadesTotalDisponibles([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("3", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult NumeroActividadPropiedadesDisponibles()
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("4", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        public IActionResult ActividadPropiedad([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("5", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        public IActionResult ActividadPropiedad(int id, [FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("6", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ActividadPropiedad(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("7", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{idActividad}/{idActividadTipo}")]
        public IActionResult ActividadPropiedadPorTipo(int idActividad, int idActividadTipo)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("8", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }
    }
}
