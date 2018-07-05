using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproModelCore.Models;
using Utilities;
using SiproDAO.Dao;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace SDocumentoAdjunto.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [EnableCors("AllowAllHeaders")]
    public class DocumentoAdjuntoController : Controller
    {
        private class datos
        {
            public int id;
            public String nombre;
            public String extension;
            public int idTipoObjto;
            public int idObjeto;
        };

        // POST api/DocumentoAdjunto/Documento
        [HttpPost("{objetoId}/{tipoObjetoId}")]
        [Authorize("Documentos Adjuntos - Crear")]
        public async Task<IActionResult> Documento([FromForm]IFormFile file, int objetoId, int tipoObjetoId)
        {
            try
            {
                bool guardado = false;
                bool existe = false;
                List<datos> datos_ = new List<datos>();
                FileStream documento;
                String directorioTemporal = @"\SIPRO\archivos\documentos\";
                if (objetoId > 0)
                {
                    directorioTemporal = directorioTemporal + tipoObjetoId + @"\";
                }
                if (tipoObjetoId >= -1)
                {
                    directorioTemporal = directorioTemporal + objetoId + @"\";
                }

                String nombreDocumento = file.FileName;
                String[] tipo = nombreDocumento.Split('.');
                String tipoContenido = tipo[tipo.Length - 1];
                Documento documentoAdjunto = new Documento();
                documentoAdjunto.nombre = nombreDocumento;
                documentoAdjunto.extension = tipoContenido;
                documentoAdjunto.idObjeto = objetoId;
                documentoAdjunto.idTipoObjeto = tipoObjetoId;
                documentoAdjunto.usuarioCreo = User.Identity.Name;
                documentoAdjunto.fechaCreacion = DateTime.Now;
                documentoAdjunto.estado = 1;

                if (!Directory.Exists(directorioTemporal))
                    Directory.CreateDirectory(directorioTemporal);

                if (nombreDocumento.LastIndexOf('/') >= 0)
                {
                    documento = new FileStream(directorioTemporal + nombreDocumento, FileMode.OpenOrCreate);
                }
                else
                {
                    documento = new FileStream(directorioTemporal + @"\" + nombreDocumento, FileMode.OpenOrCreate);
                }

                if (documento.Length == 0)
                {
                    using (var stream = documento)
                    {
                        await file.CopyToAsync(stream);

                        guardado = DocumentosAdjuntosDAO.guardarDocumentoAdjunto(documentoAdjunto);

                        if (guardado)
                        {
                            List<Documento> docs = DocumentosAdjuntosDAO.getDocumentos(objetoId, tipoObjetoId);
                            foreach (Documento doc in docs)
                            {
                                datos dato = new datos();
                                dato.id = doc.id;
                                dato.nombre = doc.nombre;
                                dato.extension = doc.extension;
                                datos_.Add(dato);
                            }

                            documento.Close();
                        }
                    }
                }
                else
                {
                    existe = true;
                    documento.Close();
                }


                return Ok(new { success = guardado, documentos = datos_, existe_archivo = existe });
            }
            catch (Exception e)
            {
                CLogger.write("1", "DocumentoAdjuntoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/DocumentoAdjunto/Documentos
        [HttpPost]
        [Authorize("Documentos Adjuntos - Visualizar")]
        public IActionResult Documentos([FromBody]dynamic value)
        {
            try
            {
                int idObjeto = value.idObjeto != null ? (int)value.idObjeto : default(int);
                int idTipoObjeto = value.idTipoObjeto != null ? (int)value.idTipoObjeto : default(int);
                List<Documento> documentos = DocumentosAdjuntosDAO.getDocumentos(idObjeto, idTipoObjeto);

                List<datos> datos_ = new List<datos>();
                foreach (Documento documento in documentos)
                {
                    datos dato = new datos();
                    dato.id = documento.id;
                    dato.nombre = documento.nombre;
                    dato.extension = documento.extension;
                    datos_.Add(dato);
                }

                return Ok(new { success = true, documentos = datos_ });
            }
            catch (Exception e)
            {
                CLogger.write("2", "DocumentoAdjuntoController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/DocumentoAdjunto/Documentos/idDocumento
        [HttpGet("{idDocumento}")]
        [Authorize("Documentos Adjuntos - Visualizar")]
        public async Task<IActionResult> Descarga(int idDocumento)
        {
            try
            {
                Documento documento = DocumentosAdjuntosDAO.getDocumentoById(idDocumento);
                String directorioTemporal = @"\SIPRO\archivos\documentos\";

                String filePath = directorioTemporal + @"\" + documento.idTipoObjeto + @"\" + documento.idObjeto + @"\" + documento.nombre;

                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
            }
            catch (Exception e)
            {
                CLogger.write("3", "DocumentoAdjuntoController.class", e);
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
                {".csv", "text/csv"}
            };
        }

        // DELETE api/DocumentoAdjunto/Documentos/idDocumento
        [HttpDelete("{idDocumento}")]
        [Authorize("Documentos Adjuntos - Eliminar")]
        public IActionResult Documento(int idDocumento)
        {
            try
            {
                Documento documento = DocumentosAdjuntosDAO.getDocumentoById(idDocumento);
                documento.usuarioActualizo = User.Identity.Name;
                bool eliminar = DocumentosAdjuntosDAO.eliminarDocumentoAdjunto(documento);
                return Ok(new { success = eliminar });
            }
            catch (Exception e)
            {
                CLogger.write("4", "DocumentoAdjuntoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
