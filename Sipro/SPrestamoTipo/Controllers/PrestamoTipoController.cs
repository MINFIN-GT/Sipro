using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;
using FluentValidation.Results;

namespace SPrestamoTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class PrestamoTipoController : Controller
    {
        private class stprestamotipo
        {
            public int id;
            public String nombre;
            public String descripcion;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }

        // POST api/PrestamoTipo/PrestamoTipoPagina
        [HttpPost]
        [Authorize("Préstamo o Proyecto Tipos - Visualizar")]
        public IActionResult PrestamoTipoPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                int numeroprestamostipos = value.numeroprestamostipos != null ? (int)value.numeroprestamostipos : default(int);
                string filtro_busqueda = value.filtro_busqueda != null ? value.filtro_busqueda : default(string);
                string columnaOrdenada = value.columnaOrdenada != null ? (string)value.columnaOrdenada : default(string);
                string ordenDireccion = value.ordenDireccion != null ? (string)value.ordenDireccion : default(string);
                string excluir = value.excluir != null ? (string)value.excluir : default(string);

                List <PrestamoTipo> lstprestamotipo = PrestamoTipoDAO.getPrestamosTipoPagina(pagina, numeroprestamostipos, filtro_busqueda, 
                    columnaOrdenada, ordenDireccion, excluir);

                List<stprestamotipo> stprestamostipo = new List<stprestamotipo>();

                foreach (PrestamoTipo prestamotipo in lstprestamotipo)
                {
                    stprestamotipo temp = new stprestamotipo();
                    temp.id = Convert.ToInt32(prestamotipo.id);
                    temp.nombre = prestamotipo.nombre;
                    temp.descripcion = prestamotipo.descripcion;
                    temp.estado = prestamotipo.estado;
                    temp.fechaActualizacion = prestamotipo.fechaActualizacion != null ? prestamotipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null ;
                    temp.fechaCreacion = prestamotipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = prestamotipo.usuarioActualizo;
                    temp.usuarioCreo = prestamotipo.usuarioCreo;
                    stprestamostipo.Add(temp);
                }

                return Ok(new { success = true, prestamostipos = stprestamostipo });
            }
            catch (Exception e)
            {
                CLogger.write("1", "PrestamoTipoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/PrestamoTipo/numeroPrestamoTipos
        [HttpPost]
        [Authorize("Préstamo o Proyecto Tipos - Visualizar")]
        public IActionResult numeroPrestamoTipos([FromBody]dynamic value)
        {
            try
            {
                string filtro_busqueda = value.filtro_busqueda != null ? value.filtro_busqueda : default(string);

                long total = PrestamoTipoDAO.getTotalPrestamosTipos(filtro_busqueda);
                return Ok(new { success = true, totalprestamotipos = total });
            }
            catch (Exception e)
            {
                CLogger.write("2", "PrestamoTipoController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/PrestamoTipo/PrestamoTipo
        [HttpPost]
        [Authorize("Préstamo o Proyecto Tipos - Crear")]
        public IActionResult PrestamoTipo([FromBody]dynamic value)
        {
            try
            {
                PrestamoTipoValidator validator = new PrestamoTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    PrestamoTipo prestamoTipo = new PrestamoTipo();
                    prestamoTipo.nombre = value.nombre != null ? (string)value.nombre : default(string);
                    prestamoTipo.descripcion = value.descripcion != null ? (string)value.descripcion : default(string);
                    prestamoTipo.estado = 1;
                    prestamoTipo.usuarioCreo = User.Identity.Name;
                    prestamoTipo.fechaCreacion = DateTime.Now;

                    bool guardado = PrestamoTipoDAO.guardarPrestamoTipo(prestamoTipo);

                    return Ok(new
                    {
                        success = guardado,
                        id = prestamoTipo.id,
                        usuarioCreo = prestamoTipo.usuarioCreo,
                        fechaCreacion = prestamoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioActualizo = prestamoTipo.usuarioActualizo,
                        fechaActualizacion = prestamoTipo.fechaActualizacion != null ? prestamoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = true });
            }
            catch (Exception e)
            {
                CLogger.write("3", "PrestamoTipoController.class", e);
                return BadRequest(500);
            }
        }

        // PUT api/PrestamoTipo/PrestamoTipo/id
        [HttpPut("{id}")]
        [Authorize("Préstamo o Proyecto Tipos - Editar")]
        public IActionResult PrestamoTipo(int id, [FromBody]dynamic value)
        {
            try
            {
                PrestamoTipoValidator validator = new PrestamoTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    PrestamoTipo prestamoTipo = PrestamoTipoDAO.getPrestamoTipoPorId(id);
                    prestamoTipo.nombre = value.nombre != null ? (string)value.nombre : default(string);
                    prestamoTipo.descripcion = value.descripcion != null ? (string)value.descripcion : default(string);
                    prestamoTipo.usuarioActualizo = User.Identity.Name;
                    prestamoTipo.fechaActualizacion = DateTime.Now;

                    bool modificado = PrestamoTipoDAO.guardarPrestamoTipo(prestamoTipo);

                    return Ok(new
                    {
                        success = modificado,
                        id = prestamoTipo.id,
                        usuarioCreo = prestamoTipo.usuarioCreo,
                        fechaCreacion = prestamoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioActualizo = prestamoTipo.usuarioActualizo,
                        fechaActualizacion = prestamoTipo.fechaActualizacion != null ? prestamoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("4", "PrestamoTipoController.class", e);
                return BadRequest(500);
            }
        }

        // DELETE api/PrestamoTipo/PrestamoTipo/id
        [HttpDelete("{id}")]
        [Authorize("Préstamo o Proyecto Tipos - Eliminar")]
        public IActionResult PrestamoTipo(int id)
        {
            try
            {
                bool eliminado = false;
                PrestamoTipo prestamoTipo = PrestamoTipoDAO.getPrestamoTipoPorId(id);
                if (prestamoTipo != null)
                    eliminado = PrestamoTipoDAO.eliminarPrestamoTipo(prestamoTipo);

                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("5", "PrestamoTipoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
