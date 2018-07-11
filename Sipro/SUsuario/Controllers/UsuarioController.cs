using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class UsuarioController : Controller
    {
        private class stetiqueta
        {
            public int id;
            public String claseNombre;
            public String proyecto;
            public String colorPrincipal;
        }

        [HttpGet]
        [Authorize("Usuarios - Visualizar")]
        public IActionResult Usuario()
        {
            try
            {
                Usuario user = UsuarioDAO.getUsuario(User.Identity.Name);
                return Ok(new { success = true, usuario = user });
            }
            catch (Exception e)
            {
                CLogger.write("1", "UsuarioController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        public IActionResult userLoginHistory([FromBody]dynamic data)
        {
            UsuarioDAO.userLoginHistory((string)data.usuario);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult tienePermiso([FromBody]dynamic data)
        {
            bool tienePermiso = UsuarioDAO.tienePermiso((string)data.usuario, (string)data.permisoNombre);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult registroUsuario([FromBody]dynamic data)
        {
            bool tienePermiso = UsuarioDAO.registroUsuario((string)data.cadenausuario, (string)data.email, (string)data.passwordTextoPlano, (string)data.usuarioCreo, (Int32)data.sistemaUsuario);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult cambiarPassword([FromBody]dynamic data)
        {
            bool passwordCambio = UsuarioDAO.cambiarPassword((string)data.usuario, (string)data.password, (string)data.usuarioActualiza);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult asignarPermisosUsuario([FromBody]dynamic data)
        {
            string strpermisos = (string)data.permisos;
            List<int> permisos = new List<int>(strpermisos.Split(',').Select(int.Parse).ToList());
            bool passwordCambio = UsuarioDAO.asignarPermisosUsuario((string)data.usuario, permisos, (string)data.usuarioCreo);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult existeUsuario([FromBody]dynamic data)
        {
            bool passwordCambio = UsuarioDAO.existeUsuario((string)data.usuario);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult desactivarUsuario([FromBody]dynamic data)
        {
            bool passwordCambio = UsuarioDAO.desactivarUsuario((string)data.usuario, (string)data.usuarioActualiza);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult editarUsuario([FromBody]dynamic data)
        {
            Usuario usuario = UsuarioDAO.getUsuario((string)data.usuario);
            usuario.email = (string)data.email;
            bool passwordCambio = UsuarioDAO.editarUsuario(usuario, (string)data.usuarioActualiza);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult getPermisosActivosUsuario([FromBody]dynamic data)
        {
            List< Permiso> permisosActivos = UsuarioDAO.getPermisosActivosUsuario((string)data.usuario);
            return Ok(JsonConvert.SerializeObject(permisosActivos));
        }

        [HttpPost]
        public IActionResult getPermisosDisponibles([FromBody]dynamic data)
        {
            List<Permiso> permisosActivos = UsuarioDAO.getPermisosDisponibles((string)data.usuario);
            return Ok(JsonConvert.SerializeObject(permisosActivos));
        }

        [HttpPost]
        public IActionResult getUsuarios([FromBody]dynamic data)
        {
            List<Usuario> usuarios = UsuarioDAO.getUsuarios((int)data.pagina, (int)data.numeroUsuarios, (string)data.usuario, (string)data.email, (string)data.filtroUsuarioCreo, (string)data.filtroFechaCreacion);
            return Ok(JsonConvert.SerializeObject(usuarios));
        }

        [HttpPost]
        public IActionResult getTotalUsuarios([FromBody]dynamic data)
        {
            long cantidadUsuarios = UsuarioDAO.getTotalUsuarios((string)data.usuario, (string)data.email, (string)data.filtroUsuarioCreo, (string)data.filtroFechaCreacion);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult getUsuariosDisponibles([FromBody]dynamic data)
        {
            UsuarioDAO.getUsuariosDisponibles();
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult desasignarPermisos([FromBody]dynamic data)
        {
            UsuarioDAO.desasignarPermisos((string)data.usuario);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult setNuevoPassword([FromBody]dynamic data)
        {
            Usuario usuario = UsuarioDAO.getUsuario((string)data.usuario);
            Usuario usuarioP = UsuarioDAO.setNuevoPassword(usuario, (string)data.password);
            return Ok(JsonConvert.SerializeObject(usuarioP));
        }

        [HttpGet]
        [Authorize("Usuarios - Visualizar")]
        public IActionResult SistemasUsuario()
        {
            try
            {
                List<Etiqueta> etiquetasUsuario = EtiquetaDAO.getEtiquetas();
                List<stetiqueta> etiquetas = new List<stetiqueta>();
                foreach (Etiqueta etiqueta in etiquetasUsuario)
                {
                    stetiqueta temp = new stetiqueta();
                    temp.id = etiqueta.id;
                    temp.claseNombre = etiqueta.nombre;
                    temp.proyecto = etiqueta.proyecto;
                    temp.colorPrincipal = etiqueta.colorPrincipal;
                    etiquetas.Add(temp);
                }
                return Ok(new { success = true, etiquetas = etiquetas });
            }
            catch (Exception e)
            {
                CLogger.write("17", "UsuarioController.class", e);
                return BadRequest(500);
            }
            
        }

        [HttpGet("{id}")]
        [Authorize("Usuarios - Visualizar")]
        public IActionResult EtiquetasSistemaUsuario(int id)
        {
            try
            {
                Etiqueta etiqueta = EtiquetaDAO.getEtiquetaPorId(id);
                stetiqueta temp = new stetiqueta();
                temp.claseNombre = etiqueta.nombre;
                temp.id = etiqueta.id;
                temp.proyecto = etiqueta.proyecto;
                temp.colorPrincipal = etiqueta.colorPrincipal;
                return Ok(new { success = true, etiquetas = temp });
            }
            catch (Exception e)
            {
                CLogger.write("18", "UsuarioController.class", e);
                return BadRequest(500);
            }
        }
    }
}
