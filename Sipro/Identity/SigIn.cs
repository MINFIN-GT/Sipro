using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity
{
    public class SignIn<TUser> : SignInManager<TUser> where TUser : class
    {
        
        
        public SignIn(UserManager<TUser> userManager, 
                      IHttpContextAccessor contextAccessor, 
                      IUserClaimsPrincipalFactory<TUser> claimsFactory, 
                      IOptions<IdentityOptions> optionsAccessor, 
                      ILogger<SignInManager<TUser>> logger, 
                      IAuthenticationSchemeProvider schemes) 
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }

        public override async Task SignInAsync(TUser user, bool isPersistent, string authenticationMethod = null)
        {
			if (user != null)
				await base.SignInAsync(user, isPersistent, authenticationMethod);
        }
    }
}
