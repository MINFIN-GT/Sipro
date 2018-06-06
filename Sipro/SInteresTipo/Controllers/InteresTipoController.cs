using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;

namespace SInteresTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class InteresTipoController : Controller
    {
        private class stinteresTipo
        {
            public int id;
            public String nombre;
            public String descripcion;
        }

        // GET api/InteresTipo/numeroInteresTipo
        [HttpGet]
        [Authorize("Interés Tipo - Visualizar")]
        public IActionResult numeroInteresTipo()
        {
            try
            {
                long total = InteresTipoDAO.getTotalInteresTipos();
                return Ok(new { success = true, totalIntereses = total });
            }
            catch (Exception e)
            {
                CLogger.write("1", "InteresTipoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/InteresTipo/AutorizacionTipoPagina
        [HttpPost]
        [Authorize("Interés Tipo - Visualizar")]
        public IActionResult AutorizacionTipoPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                int numeroInteresTipo = value.numeroInteresTipo != null ? (int)value.numeroInteresTipo : default(int);

                List <InteresTipo> autorizacionTipos = InteresTipoDAO.getInteresTiposPagina(pagina, numeroInteresTipo);

                List<stinteresTipo> stautorizaciontipos = new List<stinteresTipo>();
                foreach (InteresTipo autorizacionTipo in autorizacionTipos)
                {
                    stinteresTipo temp = new stinteresTipo();
                    temp.id = autorizacionTipo.id;
                    temp.nombre = autorizacionTipo.nombre;
                    temp.descripcion = autorizacionTipo.descripcion;
                    stautorizaciontipos.Add(temp);
                }

                return Ok(new { success = true, interesTipos = stautorizaciontipos });
            }
            catch (Exception e)
            {
                CLogger.write("2", "InteresTipoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
