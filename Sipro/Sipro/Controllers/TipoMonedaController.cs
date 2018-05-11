using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiproModelCore.Models;
using Sipro.Dao;
using Utilities;

namespace Sipro.Controllers
{
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class TipoMonedaController : Controller
    {
        // POST api/values
        [HttpPost]
        public IActionResult getTotalAuotirzacionTipo([FromBody]dynamic value)
        {
            long total = TipoMonedaDAO.getTotalAuotirzacionTipo();
            return Ok(JsonConvert.SerializeObject(total));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getAutorizacionTiposPagina([FromBody]dynamic value)
        {
            List<TipoMoneda> lsttipomoneda = TipoMonedaDAO.getAutorizacionTiposPagina((int)value.pagina, (int)value.numeroTipoMoneda);
            return Ok(JsonConvert.SerializeObject(lsttipomoneda));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getTiposMoneda([FromBody]dynamic value)
        {
            List<TipoMoneda> lsttipomoneda = TipoMonedaDAO.getTiposMoneda();
            return Ok(JsonConvert.SerializeObject(lsttipomoneda));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getTipoMonedaPorSimbolo([FromBody]dynamic value)
        {
            TipoMoneda moneda = TipoMonedaDAO.getTipoMonedaPorSimbolo((string)value.simbolo);
            return Ok(JsonConvert.SerializeObject(moneda));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getTipoMonedaPorId([FromBody]dynamic value)
        {
            TipoMoneda moneda = TipoMonedaDAO.getTipoMonedaPorId((int)value.id);
            return Ok(JsonConvert.SerializeObject(moneda));
        }
    }
}
