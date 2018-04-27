using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sipro.Dao;
using SiproModel.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
    [Route("[controller]/[action]")]
    public class UsuarioController : Controller
    {

        [HttpPost]
        public IActionResult getUsuario([FromBody]dynamic data)
        {
            Usuario usuario = UsuarioDAO.getUsuario((string)data.usuario);
            return Ok("getUsuario");
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
            List< UsuarioPermiso> permisosActivos = UsuarioDAO.getPermisosActivosUsuario((string)data.usuario);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult getPermisosDisponibles([FromBody]dynamic data)
        {
            List<Permiso> permisosActivos = UsuarioDAO.getPermisosDisponibles((string)data.usuario);
            return Ok("userLoginHistory");
        }

        [HttpPost]
        public IActionResult getUsuarios([FromBody]dynamic data)
        {
            List<Usuario> usuarios = UsuarioDAO.getUsuarios((int)data.pagina, (int)data.numeroUsuarios, (string)data.usuario, (string)data.email, (string)data.filtroUsuarioCreo, (string)data.filtroFechaCreacion);
            return Ok("userLoginHistory");
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
            return Ok("userLoginHistory");
        }
    }
}
