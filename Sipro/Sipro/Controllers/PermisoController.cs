using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiproModel.Models;
using Sipro.Dao;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class PermisoController : Controller
    {
        

        // POST api/values
        [HttpPost]
        public IActionResult getPermisos([FromBody]dynamic value)
        {
            List<Permiso> permisos = PermisoDAO.getPermisos();
            return Ok(JsonConvert.SerializeObject(permisos));
        }

        // POST api/values
        [HttpPost]
        public IActionResult guardarPermiso([FromBody]dynamic value)
        {
            Permiso permiso = new Permiso();
            permiso.id = 88888;
            permiso.nombre = "Prueba";
            permiso.descripcion = "Prueba";
            permiso.fechaCreacion = DateTime.Now;
            permiso.estado = 1;
            permiso.usuarioCreo = "admin";

            PermisoDAO.guardarPermiso(permiso);
            return Ok(JsonConvert.SerializeObject(permiso));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPermiso([FromBody]dynamic value)
        {
            Permiso permiso = PermisoDAO.getPermiso((string)value.nombrepermiso);
            return Ok(JsonConvert.SerializeObject(permiso));
        }

        // POST api/values
        [HttpPost]
        public IActionResult eliminarPermiso([FromBody]dynamic value)
        {
            Permiso permiso = PermisoDAO.getPermiso((string)value.nombrepermiso);
            bool eliminado = PermisoDAO.eliminarPermiso(permiso);
            return Ok(JsonConvert.SerializeObject(permiso));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPermisoById([FromBody]dynamic value)
        {
            Permiso permiso = PermisoDAO.getPermisoById((int)value.id);
            return Ok(JsonConvert.SerializeObject(permiso));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getPermisosPagina([FromBody]dynamic value)
        {
            List<Permiso> lstpermisos = PermisoDAO.getPermisosPagina((int)value.pagina, (int)value.numeroPermisos, (string)value.filtroId, (string)value.filtroNombre, (string)value.filtroUsuarioCreo, (string)value.filtroFechaCreacion);
            return Ok(JsonConvert.SerializeObject(lstpermisos));
        }

        // POST api/values
        [HttpPost]
        public IActionResult getTotalPermisos([FromBody]dynamic value)
        {
            long totalpermisos = PermisoDAO.getTotalPermisos((string)value.filtroId, (string)value.filtroNombre, (string)value.filtroUsuarioCreo, (string)value.filtroFechaCreacion);
            return Ok(JsonConvert.SerializeObject(totalpermisos));
        }
    }
}
