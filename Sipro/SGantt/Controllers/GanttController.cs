using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net.sf.mpxj;
using net.sf.mpxj.mpp;
using Utilities;
using java.io;

namespace SGantt.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class GanttController : Controller
    {
        [HttpGet("{proyectoId}")]
        [Authorize("Gantt - Visualizar")]
        public IActionResult Proyecto(int proyectoId)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("1", "GanttController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost("{multiproyecto}/{marcarCargado}/{proyecto_id}/{prestamoId}")]
        [Authorize("Gantt - Crear")]
        public async Task<IActionResult> Importar([FromForm]IFormFile file, int multiproyecto, int marcarCargado, int proyecto_id, int prestamoId)
        {
            try
            {
                String directorioTemporal = @"\SIPRO\archivos\temporales\";

                if (!Directory.Exists(directorioTemporal))
                    Directory.CreateDirectory(directorioTemporal);

                String fullPath = directorioTemporal + "temp_" + DateTime.Now.Ticks;
                FileStream documento = new FileStream(fullPath, FileMode.OpenOrCreate);

                if (documento.Length == 0)
                {
                    using (var stream = documento)
                    {
                        await file.CopyToAsync(stream);
                        documento.Close();
                    }
                }
                
                return Ok(new { success = true });
            }
            catch (Exception e)
            {
                CLogger.write("2", "GanttController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{proyectoId}")]
        [Authorize("Gantt - Exportar")]
        public IActionResult Exportar(int proyectoId)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("3", "GanttController.class", e);
                return BadRequest(500);
            }
        }
    }
}
