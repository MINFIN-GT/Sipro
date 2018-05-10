using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiproModelCore.Models;
using Sipro.Dao;
using Sipro.Utilities;

namespace Sipro.Controllers
{
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class UnidadEjecutoraController : Controller
    {
        // POST api/values
        [HttpPost]
        public IActionResult getUnidadEjecutora([FromBody]dynamic value)
        {
            UnidadEjecutora unidadEjecutora = UnidadEjecutoraDAO.getUnidadEjecutora((int)value.ejercicio, (int)value.entidad, (int)value.unidadEjecutora);
            return Ok(JsonConvert.SerializeObject(unidadEjecutora));
        }

        // POST api/values
        [HttpPost]
        public IActionResult guardarUnidadEjecutora([FromBody]dynamic value)
        {
            UnidadEjecutora unidadEjecutora =  UnidadEjecutoraDAO.getUnidadEjecutora((int)value.ejercicio, (int)value.entidad, (int)value.unidadEjecutora);
            bool guardado = UnidadEjecutoraDAO.guardarUnidadEjecutora(unidadEjecutora);
            return Ok(JsonConvert.SerializeObject(guardado));
        }

        // POST api/values
        [HttpPost]
        public IActionResult guardar([FromBody]dynamic value)
        {
            bool guardado = UnidadEjecutoraDAO.guardar((int)value.entidad, (int)value.ejercicio, (int)value.id, (string)value.nombre);            
            return Ok(JsonConvert.SerializeObject(guardado));
        }

        // POST api/values
        [HttpPost]
        public IActionResult actualizar([FromBody]dynamic value)
        {
            bool actualizado = UnidadEjecutoraDAO.actualizar((int)value.entidad, (int)value.ejercicio, (int)value.id, (string)value.nombre);
            return Ok(JsonConvert.SerializeObject(actualizado));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPagina([FromBody]dynamic value)
        {
            List<UnidadEjecutora> lstunidadejecutora = UnidadEjecutoraDAO.getPagina((int)value.pagina, (int)value.registros, (int)value.ejercicio, (int)value.entidad);
            return Ok(JsonConvert.SerializeObject(lstunidadejecutora));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPaginaPorEntidad([FromBody]dynamic value)
        {
            List<UnidadEjecutora> lstunidadejecutora = UnidadEjecutoraDAO.getPaginaPorEntidad((int)value.pagina, (int)value.registros, (int)value.entidad, (int)value.ejercicio);
            return Ok(JsonConvert.SerializeObject(lstunidadejecutora));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getJson([FromBody]dynamic value)
        {
            string strtunidadejecutora = UnidadEjecutoraDAO.getJson((int)value.pagina, (int)value.registros, (int)value.ejercicio, (int)value.entidad);
            return Ok(JsonConvert.SerializeObject(strtunidadejecutora));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getJsonPorEntidad([FromBody]dynamic value)
        {
            string strtunidadejecutora = UnidadEjecutoraDAO.getJsonPorEntidad((int)value.pagina, (int)value.registros, (int)value.entidad, (int)value.ejercicio);
            return Ok(JsonConvert.SerializeObject(strtunidadejecutora));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getTotal([FromBody]dynamic value)
        {
            long total = UnidadEjecutoraDAO.getTotal((int)value.ejercicio, (int)value.entidad);
            return Ok(JsonConvert.SerializeObject(total));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getUnidadEjecutoras([FromBody]dynamic value)
        {
            List<UnidadEjecutora> lstunidadesejecutoras = UnidadEjecutoraDAO.getUnidadEjecutoras((int)value.ejercicio, (int)value.entidad);
            return Ok(JsonConvert.SerializeObject(lstunidadesejecutoras));
        }
    }
}