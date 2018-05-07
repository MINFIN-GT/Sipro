using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiproModel.Models;
using Sipro.Dao;
using Sipro.Utilities;

namespace Sipro.Controllers
{
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class CooperanteController : Controller
    {
        // POST api/values
        [HttpPost]
        public IActionResult getCooperantes([FromBody]dynamic value)
        {
            List<Cooperante> lstcooperante = CooperanteDAO.getCooperantes();
            return Ok(JsonConvert.SerializeObject(lstcooperante));
        }

        // POST api/values
        [HttpPost]
        public IActionResult guardarCooperante([FromBody]dynamic value)
        {
            Cooperante cooperante = new Cooperante();
            cooperante.codigo = (int)value.codigo;
            cooperante.descripcion = (string)value.descripcion;
            cooperante.ejercicio = (int)value.ejercicio;
            cooperante.estado = (int)value.estado;
            cooperante.fechaCreacion = DateTime.Now;
            cooperante.nombre = (string)value.nombre;
            cooperante.siglas = (string)value.siglas;
            cooperante.usuarioCreo = (string)value.usuarioCreo;

            bool guardado = CooperanteDAO.guardarCooperante(cooperante);
            return Ok(JsonConvert.SerializeObject(guardado));
        }

        // POST api/values
        [HttpPost]
        public IActionResult eliminarCooperante([FromBody]dynamic value)
        {
            Cooperante cooperante = CooperanteDAO.getCooperantePorCodigo((int)value.codigo);
            bool eliminado = CooperanteDAO.eliminarCooperante(cooperante);
            return Ok(JsonConvert.SerializeObject(eliminado));
        }

        // POST api/values
        [HttpPost]
        public IActionResult eliminarTotalCooperante([FromBody]dynamic value)
        {
            Cooperante cooperante = CooperanteDAO.getCooperantePorCodigo((int)value.codigo);
            bool eliminado = CooperanteDAO.eliminarTotalCooperante(cooperante);
            return Ok(JsonConvert.SerializeObject(eliminado));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getCooperantesPagina([FromBody]dynamic value)
        {
            List<Cooperante> lstcooperante = CooperanteDAO.getCooperantesPagina((int)value.pagina, (int)value.numerocooperantes, (string)value.filtro_codigo, (string)value.filtro_nombre,
                (string)value.filtro_usuario_creo, (string)value.filtro_fecha_creacion, (string)value.columna_ordenada, (string)value.orden_direccion);
            return Ok(JsonConvert.SerializeObject(lstcooperante));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getTotalCooperantes([FromBody]dynamic value)
        { 
            long total = CooperanteDAO.getTotalCooperantes((string)value.filtro_codigo, (string)value.filtro_nombre,
                (string)value.filtro_usuario_creo, (string)value.filtro_fecha_creacion);
            return Ok(JsonConvert.SerializeObject(total));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getCooperantePorCodigo([FromBody]dynamic value)
        {
            Cooperante cooperante = CooperanteDAO.getCooperantePorCodigo((int)value.codigo);
            return Ok(JsonConvert.SerializeObject(cooperante));
        }
    }
}