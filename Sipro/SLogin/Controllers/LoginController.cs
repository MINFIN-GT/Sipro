using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Identity;
using SiproModelCore.Models;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using SiproDAO.Dao;
using Microsoft.AspNetCore.Authentication;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
	[Authorize]
	[Route("/api/[controller]/[action]")]
	[Produces("application/json")]
	[EnableCors("AllowAllHeaders")]
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

		private string generateJWT(string username)
		{
			HMACSHA256 hmac = new HMACSHA256();
			string secret = Convert.ToBase64String(hmac.Key);
			byte[] key = Convert.FromBase64String(secret);
			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
				new Claim(ClaimTypes.Name, username)});
			UsuarioDAO.getPermisosActivosUsuario(username).ForEach(
				permiso =>
				{
					claimsIdentity.AddClaim(new Claim(CustomClaimType.Permission, permiso.nombre));
				}
			);
			SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
			{
				Subject = claimsIdentity,
				NotBefore = DateTime.UtcNow,
				Expires = DateTime.UtcNow.AddMinutes(60),
				SigningCredentials = new SigningCredentials(securityKey,
				SecurityAlgorithms.HmacSha256Signature)
			};

			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
			return handler.WriteToken(token);
		}

		// POST /<controller>
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> In([FromBody]dynamic data)
		{
			String susuario = data.username;
			String password = data.password;
			try
			{
				var result = await _signInManager.PasswordSignInAsync(susuario, password, false, lockoutOnFailure: false);
				if (result.Succeeded)
				{
                    
					var identity = new ClaimsIdentity("Identity.Application");
                    identity.AddClaim(new Claim(ClaimTypes.Name, susuario));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "General"));
					List<Permiso> permisos = UsuarioDAO.getPermisosActivosUsuario(susuario);
                    foreach (Permiso permiso in permisos)
                    {
                        identity.AddClaim(new Claim(CustomClaimType.Permission, permiso.nombre));
                    }
					var _User = new ClaimsPrincipal(identity);
                    
                    await this.HttpContext.SignInAsync(
						"Identity.Application",
						_User,
						new AuthenticationProperties()
						{
							ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
							IsPersistent = false,
							AllowRefresh = true
						}
					);
					return Ok(new { success = true, jwt = generateJWT(susuario) });
				}
				if (result.IsLockedOut)
				{
					return Ok(new { success = false, mensaje = "Usuario bloqueado" });
				}
				else
				{
					return Ok(new { success = false, mensaje = "Login fallido" });
				}
			}
			catch (Exception e)
			{
				return Ok(new { success = false, mensaje = "Login fallido" });
			}
		}

		[HttpGet]
		[Authorize(Roles = "General")]
		public IActionResult Out()
		{
			try
			{
				_signInManager.SignOutAsync();
				HttpContext.SignOutAsync();
				return Ok(new { success = true });
			}
			catch (Exception e)
			{
				return Ok(new { sucess = false });
			}
		}
	}
}
