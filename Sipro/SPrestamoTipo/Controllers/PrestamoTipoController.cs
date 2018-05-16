using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;

namespace SPrestamoTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
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
                List<PrestamoTipo> lstprestamotipo = PrestamoTipoDAO.getPrestamosTipoPagina((int)value.pagina, (int)value.numeroproyectotipos, (string)value.filtroNombre,
                    (string)value.filtroUsuarioCreo, (string)value.filtroFechaCreacion, (string)value.columnaOrdenada, (string)value.ordenDireccion, (string)value.excluir);

                List<stprestamotipo> stcooperantes = new List<stprestamotipo>();

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
                    stcooperantes.Add(temp);
                }

                return Ok(new { success = true, poryectotipos = stcooperantes });
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
                long total = PrestamoTipoDAO.getTotalPrestamosTipos((string)value.filtoNombre, (string)value.filtroUsuarioCreo, (string)value.filtroFechaCreacion);
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
                PrestamoTipo prestamoTipo = new PrestamoTipo();
                prestamoTipo.nombre = (string)value.nombre;
                prestamoTipo.descripcion = (string)value.descripcion;
                prestamoTipo.estado = 1;
                prestamoTipo.usuarioCreo = User.Identity.Name;
                prestamoTipo.fechaCreacion = DateTime.Now;

                bool guardado = PrestamoTipoDAO.guardarPrestamoTipo(prestamoTipo);

                return Ok(new {
                    success = guardado,
                    id = prestamoTipo.id,
                    usuarioCreo = prestamoTipo.usuarioCreo,
                    fechaCreacion = prestamoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                    usuarioactualizo = prestamoTipo.usuarioActualizo,
                    fechaactualizacion = prestamoTipo.fechaActualizacion != null ? prestamoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                });
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
        public IActionResult PrestamoTipoA(int id, [FromBody]dynamic value)
        {
            try
            {
                PrestamoTipo prestamoTipo = PrestamoTipoDAO.getPrestamoTipoPorId(id);
                prestamoTipo.nombre = (string)value.nombre;
                prestamoTipo.descripcion = (string)value.descripcion;
                prestamoTipo.usuarioActualizo = User.Identity.Name;
                prestamoTipo.fechaActualizacion = DateTime.Now;

                bool modificado = PrestamoTipoDAO.guardarPrestamoTipo(prestamoTipo);

                return Ok(new
                {
                    success = modificado,
                    id = prestamoTipo.id,
                    usuarioCreo = prestamoTipo.usuarioCreo,
                    fechaCreacion = prestamoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                    usuarioactualizo = prestamoTipo.usuarioActualizo,
                    fechaactualizacion = prestamoTipo.fechaActualizacion != null ? prestamoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                });
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
        public IActionResult borrarPrestamoTipo(int id)
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
