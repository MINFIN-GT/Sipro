using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sipro.Models;
using Sipro.Dao;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
    [Route("api/[controller]")]
    public class ActividadPropiedadController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            new ActividadPropiedadDAO();
            //List<actividad_propiedad> lstActividadPropiedads = ActividadPropiedadDAO.getActividadPropiedadPaginaTotalDisponibles(0,20,"5,3,4");
            //String[] resultList = new String[lstActividadPropiedads.Count];

            //for (int i = 0; i < lstActividadPropiedads.Count; i++)
            //{
            //    actividad_propiedad temp = lstActividadPropiedads[i];
            //    resultList[i] = String.Join(" - ", temp.id.ToString(), temp.nombre, temp.fecha_creacion.ToString(), temp.fecha_actualizacion.ToString());
            //}

            actividad_propiedad temp = ActividadPropiedadDAO.getActividadPropiedadPorId(1);
            ActividadPropiedadDAO.eliminarActividadPropiedad(temp);
            return new String[] { ActividadPropiedadDAO.getTotalActividadPropiedades().ToString() };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
