using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
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
                String lineaBase = null;
                if (!Directory.Exists(directorioTemporal))
                    Directory.CreateDirectory(directorioTemporal);

                String nombreArchivo = "temp_" + Guid.NewGuid();
                String fullPath = directorioTemporal + nombreArchivo;
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
                String arguments = "-jar \"" + @Utils.getJartImportProject()+ "\" \"1\" \"" + directorioTemporal + "\" \"" + nombreArchivo + "\" \"" + User.Identity.Name +"\" \"0\" \"" + proyecto_id + "\" \"1\" \"" + prestamoId + "\" \"" + lineaBase + "\"";
                p.StartInfo.FileName = "java.exe";
                p.StartInfo.Arguments = arguments;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                Int32 proyResult;
                if (Int32.TryParse(output, out proyResult)) { }

                System.IO.File.Delete(fullPath);

                ProyectoDAO.calcularCostoyFechas(proyResult);

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
        public async Task<IActionResult> Exportar(int proyectoId)
        {
            try
            {
                String directorioTemporal = @Utils.getDirectorioTemporal();
                String lineaBase = null;

                if (!Directory.Exists(directorioTemporal))
                    Directory.CreateDirectory(directorioTemporal);

                String nombreArchivo = "temp_" + Guid.NewGuid();

                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                String arguments = "-jar \"" + @Utils.getJartImportProject() + "\" \"2\" \"" + directorioTemporal + "\" \"" + nombreArchivo + "\" \"" + User.Identity.Name + "\" \"0\" \"" + proyectoId + "\" \"1\" \"" + 0 + "\" \"" + lineaBase + "\"";
                p.StartInfo.FileName = "java.exe";
                p.StartInfo.Arguments = arguments;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                var memory = new MemoryStream();
                using (var stream = new FileStream(directorioTemporal + nombreArchivo + ".xml", FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(directorioTemporal+nombreArchivo+".xml"), Path.GetFileName(directorioTemporal + nombreArchivo + ".xml"));               
            }
            catch (Exception e)
            {
                CLogger.write("3", "GanttController.class", e);
                return BadRequest(500);
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".xml", "text/xml"}
            };
        }
    }
}
