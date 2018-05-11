using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SiproDAO.Dao;
using SiproModelCore.Models;

namespace Identity
{
    public class ApplicationClaimsIdentityFactory : UserClaimsPrincipalFactory<User>
    {
        UserManager<User> _userManager;

        public ApplicationClaimsIdentityFactory(UserManager<User> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);
            var identity = ((ClaimsIdentity)principal.Identity);
            identity.AddClaim(new Claim(ClaimTypes.Role, "General"));
            List<Permiso> permisos = UsuarioDAO.getPermisosActivosUsuario(user.UserName);
            foreach (Permiso permiso in permisos)
            {
                identity.AddClaim(new Claim(CustomClaimType.Permission, permiso.nombre));
            }
            return principal;
        }
    }
}
