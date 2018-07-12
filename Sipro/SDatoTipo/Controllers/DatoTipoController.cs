using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SDatoTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class DatoTipoController : Controller
    {
        private class stdatotipo
        {
            public int id;
            public String nombre;
            public String descripcion;
        }

        // GET api/DatoTipo/Listar
        [HttpGet]
        [Authorize("Dato Tipo - Visualizar")]
        public IActionResult Listar()
        {
            try
            {
                List<DatoTipo> datoTipos = DatoTipoDAO.getDatoTipos();
                List<stdatotipo> lstdatotipo = new List<stdatotipo>();
                foreach (DatoTipo datoTipo in datoTipos)
                {
                    stdatotipo temp = new stdatotipo();
                    temp.id = datoTipo.id;
                    temp.nombre = datoTipo.nombre;
                    temp.descripcion = datoTipo.descripcion;
                    lstdatotipo.Add(temp);
                }

                return Ok(new { success = true, datoTipos = lstdatotipo });
            }
            catch (Exception e)
            {
                CLogger.write("1", "DatoTipo.class", e);
                return BadRequest(500);
            }
        }

        // GET api/DatoTipo/DatoTipoPorId/1
        [HttpGet("{id}")]
        [Authorize("Dato Tipo - Visualizar")]
        public IActionResult DatoTipoPorId(int id)
        {
            try
            {
                DatoTipo datoTipo = DatoTipoDAO.getDatoTipo(id);
                return Ok(new {
                    success = datoTipo != null ? true : false,
                    id = datoTipo.id,
                    nombre = datoTipo.nombre
                });
            }
            catch (Exception e)
            {
                CLogger.write("2", "DatoTipo.class", e);
                return BadRequest(500);
            }
        }
    }
}
