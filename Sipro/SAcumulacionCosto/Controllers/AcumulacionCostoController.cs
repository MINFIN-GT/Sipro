using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;

namespace SAcumulacionCosto.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class AcumulacionCostoController : Controller
    {
        private class stAcumulacionCosto
        {
            public int id;
            public string nombre;
            public string usuarioCreo;
            public string usuarioActualizo;
            public string fechaCreacion;
            public string fechaActualizacion;
            int estado;
        }

        // GET api/AcumulacionCosto/AcumulacionesCosto
        [HttpGet]
        [Authorize("Acumulación Costo - Visualizar")]
        public IActionResult AcumulacionesCosto()
        {
            try
            {
                List<AcumulacionCosto> acumulacionCostos = AcumulacionCostoDAO.getAcumulacionesCosto();

                List<stAcumulacionCosto> stacumulacioncosto = new List<stAcumulacionCosto>();
                foreach (AcumulacionCosto acumulacionCosto in acumulacionCostos)
                {
                    stAcumulacionCosto temp = new stAcumulacionCosto();
                    temp.id = Convert.ToInt32(acumulacionCosto.id);
                    temp.nombre = acumulacionCosto.nombre;
                    stacumulacioncosto.Add(temp);
                }

                return Ok(new { success = true, acumulacionesTipos = stacumulacioncosto });
            }
            catch (Exception e)
            {
                CLogger.write("1", "AcumulacionCostoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
