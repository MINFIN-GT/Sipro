using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;

namespace SEjecucionEstado.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class EjecucionEstadoController : Controller
    {
        private class stprograma
        {
            public int id;
            public String nombre;
            public String descripcion;
            public int programatipoid;
            public String programatipo;
            public String fechaCreacion;
            public String usuarioCreo;
            public String fechaactualizacion;
            public String usuarioactualizo;
        };

        // POST api/EjecucionEstado/EjecucionEstadoPagina
        [HttpPost]
        [Authorize("Ejecución Estado - Visualizar")]
        public IActionResult EjecucionEstadoPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                int numeroEjecucionEstado = value.numeroEjecucionEstado != null ? (int)value.numeroEjecucionEstado : default(int);

                List <EjecucionEstado> ejecucionEstados = EjecucionEstadoDAO.getEjecucionEstadosPagina(pagina, numeroEjecucionEstado);

                List<stprograma> sttipomoneda = new List<stprograma>();
                foreach (EjecucionEstado tipoMoneda in ejecucionEstados)
                {
                    stprograma temp = new stprograma();
                    temp.id = Convert.ToInt32(tipoMoneda.id);
                    temp.nombre = tipoMoneda.nombre;;
                    sttipomoneda.Add(temp);
                }

                return Ok(new { success = true, ejecucionEstados = sttipomoneda });
            }
            catch (Exception e)
            {
                CLogger.write("1", "EjecucionEstadoController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/EjecucionEstado/EjecucionEstadoPagina
        [HttpGet]
        [Authorize("Ejecución Estado - Visualizar")]
        public IActionResult numeroEjecucionEstado()
        {
            try
            {
                long total = EjecucionEstadoDAO.getTotalEjecucionEstado();
                return Ok(new { success = true, totalactividadtipos = total });
            }
            catch (Exception e)
            {
                CLogger.write("2", "EjecucionEstadoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
