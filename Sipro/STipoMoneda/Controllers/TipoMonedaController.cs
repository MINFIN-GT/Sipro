using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;

namespace STipoMoneda.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class TipoMonedaController : Controller
    {
        private class stTipoMoneda
        {
            public int id;
            public String nombre;
            public String simbolo;
        };

        // POST api/TipoMoneda/TipoMonedaPagina
        [HttpPost]
        public IActionResult TipoMonedaPagina([FromBody]dynamic value)
        {
            try
            {
                List<TipoMoneda> lsttipomoneda = TipoMonedaDAO.getAutorizacionTiposPagina((int)value.pagina, (int)value.numeroTipoMoneda);

                List<stTipoMoneda> sttipomoneda = new List<stTipoMoneda>();
                foreach (TipoMoneda tipoMoneda in lsttipomoneda)
                {
                    stTipoMoneda temp = new stTipoMoneda();
                    temp.id = tipoMoneda.id;
                    temp.nombre = tipoMoneda.nombre;
                    temp.simbolo = tipoMoneda.simbolo;
                    sttipomoneda.Add(temp);
                }

                return Ok(new { success = true, tipoMonedas = sttipomoneda });
            }
            catch (Exception e)
            {
                CLogger.write("1", "TipoMonedaController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/TipoMoneda/numeroTipoMonedas
        [HttpPost]
        public IActionResult numeroTipoMonedas([FromBody]dynamic value)
        {
            try
            {
                long total = TipoMonedaDAO.getTotalAuotirzacionTipo();
                return Ok(new { success = true, totalactividadtipos = total });
            }
            catch (Exception e)
            {
                CLogger.write("2", "TipoMonedaController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/TipoMoneda/TipoMonedas
        [HttpGet]
        public IActionResult TipoMonedas()
        {
            try
            {
                List<TipoMoneda> tipoMonedas = TipoMonedaDAO.getTiposMoneda();
                List<stTipoMoneda> sttipomoneda = new List<stTipoMoneda>();
                foreach (TipoMoneda tipoMoneda in tipoMonedas)
                {
                    stTipoMoneda temp = new stTipoMoneda();
                    temp.id = tipoMoneda.id;
                    temp.nombre = tipoMoneda.nombre;
                    temp.simbolo = tipoMoneda.simbolo;
                    sttipomoneda.Add(temp);
                }

                return Ok(new { success = true, tipoMonedas = sttipomoneda });
            }
            catch (Exception e)
            {
                CLogger.write("3", "TipoMonedaController.class", e);
                return BadRequest(500);
            }
        }
    }
}
