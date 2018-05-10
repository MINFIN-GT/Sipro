using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiproModelCore.Models;
using Sipro.Dao;

namespace Sipro.Controllers
{
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class PrestamoTipoController : Controller
    {
        // POST api/values
        [HttpPost]
        public IActionResult getPrestamoTipos([FromBody]dynamic value)
        {
            List<PrestamoTipo> lstprestamotipos = PrestamoTipoDAO.getPrestamoTipos();
            return Ok(JsonConvert.SerializeObject(lstprestamotipos));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPrestamoTipoPorId([FromBody]dynamic value)
        {
            PrestamoTipo prestamotipo = PrestamoTipoDAO.getPrestamoTipoPorId((int)value.id);
            return Ok(JsonConvert.SerializeObject(prestamotipo));
        }

        // POST api/values
        [HttpPost]
        public IActionResult guardarPrestamoTipo([FromBody]dynamic value)
        {
            PrestamoTipo prestamotipo = PrestamoTipoDAO.getPrestamoTipoPorId((int)value.id);
            bool guardado = PrestamoTipoDAO.guardarPrestamoTipo(prestamotipo);
            return Ok(JsonConvert.SerializeObject(guardado));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPrestamosTipoPagina([FromBody]dynamic value)
        {
            List<PrestamoTipo> lstprestamotipo = PrestamoTipoDAO.getPrestamosTipoPagina((int)value.pagina, (int)value.numeroproyectotipos, (string)value.filtroNombre,
                (string)value.filtroUsuarioCreo, (string)value.filtroFechaCreacion, (string)value.columnaOrdenada, (string)value.ordenDireccion, (string)value.excluir);
            return Ok(JsonConvert.SerializeObject(lstprestamotipo));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getTotalPrestamosTipos([FromBody]dynamic value)
        {
           long total = PrestamoTipoDAO.getTotalPrestamosTipos((string)value.filtoNombre, (string)value.filtroUsuarioCreo, (string)value.filtroFechaCreacion);
            return Ok(JsonConvert.SerializeObject(total));
        }

        // POST api/values
        [HttpPost]
        public IActionResult eliminarPrestamoTipo([FromBody]dynamic value)
        {
            PrestamoTipo prestamotipo = PrestamoTipoDAO.getPrestamoTipoPorId((int)value.id);
            bool eliminado = PrestamoTipoDAO.eliminarPrestamoTipo(prestamotipo);
            return Ok(JsonConvert.SerializeObject(eliminado));
        }

        // POST api/values
        [HttpPost]
        public IActionResult desasignarTiposAPrestamo([FromBody]dynamic value)
        {            
            bool desasignado = PrestamoTipoDAO.desasignarTiposAPrestamo((int)value.prestamoId);
            return Ok(JsonConvert.SerializeObject(desasignado));
        }

        // POST api/values
        [HttpPost]
        public IActionResult asignarTiposAPrestamo([FromBody]dynamic value)
        {
            string strtipos = (string)value.prestamoTipos;
            List<int> tipos = new List<int>(strtipos.Split(',').Select(int.Parse).ToList());
            Prestamo prestamo = PrestamoDAO.getPrestamoById((int)value.idPrestamo);
            bool asignado = PrestamoTipoDAO.asignarTiposAPrestamo(tipos, prestamo, (string)value.usuario);
            return Ok(JsonConvert.SerializeObject(false));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPrestamoTiposPrestamo([FromBody]dynamic value)
        {
            List<PrestamoTipoPrestamo> lstprestamotipoprestamo = PrestamoTipoDAO.getPrestamoTiposPrestamo((int)value.idPrestamo);
            return Ok(JsonConvert.SerializeObject(lstprestamotipoprestamo));
        }
    }
}