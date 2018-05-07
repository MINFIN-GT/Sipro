using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sipro.Dao;
using Sipro.Utilities;
using Sipro.Utilities.Identity;
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
        private readonly RoleManager<Rol> _roleManager;


        public LoginController(
                    RoleManager<Rol> roleManager,
                    UserManager<Usuario> userManager,
                    SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
                HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                { new Claim(ClaimTypes.Name, usuario) }, CookieAuthenticationDefaults.AuthenticationScheme));
                var identity = (ClaimsIdentity)HttpContext.User.Identity;
                identity.AddClaim(new Claim(ClaimTypes.Role, "General"));
                List<Permiso> permisos = UsuarioDAO.getPermisosActivosUsuario(usuario);
                foreach(Permiso permiso in permisos){
                    identity.AddClaim(new Claim(CustomClaimType.Permission, permiso.nombre));
                }
                return Ok("1");
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
            Console.WriteLine("Aqui");
            Console.WriteLine("Aqui");
            Console.WriteLine("Aqui");
            _signInManager.SignOutAsync();
            return Ok("Sign out");
        }
    }
}
