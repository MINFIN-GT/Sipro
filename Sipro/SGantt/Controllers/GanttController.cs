using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities;

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

        [HttpPost("{multiproyecto}/{mostrarCargando}/{proyecto_id}/{prestamoId}")]
        [DisableRequestSizeLimit]
        [Authorize("Gantt - Crear")]
        public async Task<IActionResult> Importar([FromForm]IFormFile file, int multiproyecto, int mostrarCargando, int proyecto_id, int prestamoId)
        {
            try
            {
                String directorioTemporal = @Utils.getDirectorioTemporal();

                if (!Directory.Exists(directorioTemporal))
                    Directory.CreateDirectory(directorioTemporal);

                String fullPath = directorioTemporal + "temp_" + Guid.NewGuid();
                FileStream documento = new FileStream(fullPath, FileMode.OpenOrCreate);

                if (documento.Length == 0)
                {
                    using (var stream = documento)
                    {
                        await file.CopyToAsync(stream);
                        documento.Close();
                    }
                }

                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                String arguments = "-jar \"" + @Utils.getJartImportProject() + "\" \"" + fullPath + "\" \""+ User.Identity.Name +"\" \"0\" \"" + proyecto_id + "\" \"1\" \"" + prestamoId + "\"";
                p.StartInfo.FileName = "java.exe";
                p.StartInfo.Arguments = arguments;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                Int32 proyResult;
                if (Int32.TryParse(output, out proyResult)) { }

                System.IO.File.Delete(fullPath);

                return Ok(new { success = proyResult > 0 ? true : false, proyectoId = proyResult });
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
