using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SProyectoImpacto.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ProyectoImpactoController : Controller
    {
        private class stimpacto
        {
            public int entidadId;
            public String entidadNombre;
            public String impacto;
            public int estado;
            public String fechaCreacion;
            public String usuarioCreo;
            public String fechaactualizacion;
            public String usuarioactualizo;
        }

        [HttpGet("{proyectoId}")]
        [Authorize("Préstamos o Proyectos - Visualizar")]
        public IActionResult ImpactosPorProyecto(int proyectoId)
        {
            try
            {
                List<ProyectoImpacto> proyectoImpactos = ProyectoImpactoDAO.getProyectoImpactoPorProyecto(proyectoId);
                List<stimpacto> impactos = new List<stimpacto>();
                if (proyectoImpactos != null)
                {
                    foreach (ProyectoImpacto pi in proyectoImpactos)
                    {
                        stimpacto temp = new stimpacto();
                        pi.entidads = EntidadDAO.getEntidad(pi.entidadentidad, pi.ejercicio);
                        temp.entidadId = pi.entidads != null ? pi.entidads.entidad : default(int);
                        temp.entidadNombre = pi.entidads != null ? pi.entidads.nombre : default(string);
                        temp.impacto = pi.impacto;
                        temp.estado = pi.estado;
                        impactos.Add(temp);
                    }
                }

                return Ok(new { success = true, impactos = impactos });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProyectoImpactoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
