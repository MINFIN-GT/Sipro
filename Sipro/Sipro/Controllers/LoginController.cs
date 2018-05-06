using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sipro.Utilities;
using SiproModel.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;


        public LoginController(
                    UserManager<Usuario> userManager,
                    SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST /<controller>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> In([FromBody]dynamic data)
        {
            String ret = "";
            String usuario = data.user;
            String password = data.password;
            var result = await _signInManager.PasswordSignInAsync(usuario, password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok(true);
            }
            if (result.IsLockedOut)
            {
                ret = "Usuario bloqueado";
                return Ok(ret);
            }
            else
            {
                ret = "Login fallido";
                return Ok(ret);
            }
            //return Ok("Datos: " + data.user + ", " + data.password);
        }

        [HttpPost]
        [Authorize(Roles = "General")]
        public IActionResult Out()
        {
            return Ok("Sign out");
        }
    }
}
