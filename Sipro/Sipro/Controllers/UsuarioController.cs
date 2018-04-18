using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sipro.Dao;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
    [Route("[controller]/[action]")]
    public class UsuarioController : Controller
    {

        [HttpPost]
        public IActionResult CrearUsuario([FromBody]dynamic data)
        {
            new UsuarioDAO();
            UsuarioDAO.registroUsuario((string)data.nuevousuario, (string)data.nuevomail, (string)data.nuevopassword, (string)"admin",(int)data.sistemaUsuario);
            return Ok("Return de Crear Usuario");
        }

        [HttpPost]
        public IActionResult BorrarUsuario([FromBody]dynamic data)
        {
            return Ok("Return de Borrar Usuario");
        }
    }
}
