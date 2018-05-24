using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;

namespace SAutorizacionTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class AutorizacionTipoController : Controller
    {
        private class stautorizacionTipo
        {
            public int id;
            public String nombre;
            public String descripcion;
        }

        // GET api/values
        [HttpGet]
        [Authorize("Autorización Tipo - Visualizar")]
        public IActionResult numeroAutorizacionTipo()
        {
            try
            {
                long total = AutorizacionTipoDAO.getTotalAuotirzacionTipo();
                return Ok(new { success = true, totalactividades = total });
            }
            catch (Exception e)
            {
                CLogger.write("1", "AcumulacionCostoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/values
        [HttpPost]
        [Authorize("Autorización Tipo - Visualizar")]
        public IActionResult AutorizacionTipoPagina([FromBody]dynamic value)
        {
            try
            {
                List<AutorizacionTipo> autorizacionTipos = AutorizacionTipoDAO.getAutorizacionTiposPagina((int)value.pagina, (int)value.numeroAutorizacionTipo);

                List<stautorizacionTipo> stautorizaciontipos = new List<stautorizacionTipo>();
                foreach (AutorizacionTipo autorizacionTipo in autorizacionTipos)
                {
                    stautorizacionTipo temp = new stautorizacionTipo();
                    temp.id = autorizacionTipo.id;
                    temp.nombre = autorizacionTipo.nombre;
                    temp.descripcion = autorizacionTipo.descripcion;
                    stautorizaciontipos.Add(temp);
                }

                return Ok(new { success = true, autorizacionTipos = stautorizaciontipos });
            }
            catch (Exception e)
            {
                CLogger.write("2", "AcumulacionCostoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
