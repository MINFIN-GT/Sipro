using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace STipoMoneda.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [EnableCors("AllowAllHeaders")]
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
        [Authorize("Tipo Moneda - Visualizar")]
        public IActionResult TipoMonedaPagina([FromBody]dynamic value)
        {
            try
            {
                string filtro_busqueda = value.filtro_busqueda != null ? value.filtro_busqueda : default(string);
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                int numeroTipoMoneda = value.numeroTipoMoneda != null ? (int)value.numeroTipoMoneda : default(int);

                List <TipoMoneda> lsttipomoneda = TipoMonedaDAO.getAutorizacionTiposPagina(pagina, numeroTipoMoneda, filtro_busqueda);

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
        [Authorize("Tipo Moneda - Visualizar")]
        public IActionResult numeroTipoMonedas([FromBody]dynamic value)
        {
            try
            {
                string filtro_busqueda = value.filtro_busqueda != null ? value.filtro_busqueda : default(string);
                long total = TipoMonedaDAO.getTotalAuotirzacionTipo(filtro_busqueda);
                return Ok(new { success = true, totalTipoMonedas = total });
            }
            catch (Exception e)
            {
                CLogger.write("2", "TipoMonedaController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/TipoMoneda/TipoMonedas
        [HttpGet]
        [Authorize("Tipo Moneda - Visualizar")]
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
