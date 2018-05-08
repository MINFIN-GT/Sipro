using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sipro.Dao;
using Sipro.Utilities.Identity;
using SiproModel.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
    [Authorize]
    [Route("/api/[controller]/[action]")]
    [Produces("application/json")]
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Rol> _roleManager;


        public LoginController(
                    RoleManager<Rol> roleManager,
                    UserManager<User> userManager,
                    SignInManager<User> signInManager)
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
            String susuario = data.username;
            String password = data.password;
            User usuario = await _userManager.FindByIdAsync(susuario);
            var result = await _signInManager.PasswordSignInAsync(susuario, password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok(new { success=true });
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
        }

        [HttpGet]
        [Authorize(Roles = "General")]
        public IActionResult Out()
        {
            _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
