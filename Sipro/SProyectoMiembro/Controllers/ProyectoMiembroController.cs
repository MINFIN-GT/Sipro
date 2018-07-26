using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SProyectoMiembro.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ProyectoMiembroController : Controller
    {
        private class stmiembro
        {
            public int proyectoId;
            public int id;
            public String nombre;
            public int estado;
            public String fechaCreacion;
            public String usuarioCreo;
            public String fechaactualizacion;
            public String usuarioactualizo;
        }

        // GET api/ProyectoMiembro/MiembrosPorProyecto/1
        [HttpGet("{proyectoId}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult MiembrosPorProyecto(int proyectoId)
        {
            try
            {
                List<ProyectoMiembro> ProyectoMiembros = ProyectoMiembroDAO.getProyectoMiembroPorProyecto(proyectoId);
                List<stmiembro> miembros = new List<stmiembro>();
                if (ProyectoMiembros != null)
                {
                    foreach (ProyectoMiembro pi in ProyectoMiembros)
                    {
                        stmiembro temp = new stmiembro();
                        temp.proyectoId = pi.proyectoid;
                        temp.id = pi.colaboradorid;

                        pi.colaboradors = ColaboradorDAO.getColaborador(pi.colaboradorid);

                        temp.nombre = pi.colaboradors != null ? (pi.colaboradors.pnombre
                                + " " + (pi.colaboradors.snombre != null ? pi.colaboradors.snombre : "")
                                + " " + pi.colaboradors.papellido
                                + " " + (pi.colaboradors.sapellido != null ? pi.colaboradors.sapellido : "")
                                ) : "";
                        temp.estado = pi.estado;
                        miembros.Add(temp);
                    }
                }

                return Ok(new { success = true, miembros = miembros });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProyectoMiembroController.class", e);
                return BadRequest(500);
            }
        }
    }
}
