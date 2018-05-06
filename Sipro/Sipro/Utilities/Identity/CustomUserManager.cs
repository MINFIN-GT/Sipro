using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SiproModel.Models;

namespace Sipro.Utilities.Identity
{
    public class CustomUserManager : UserManager<Usuario>
    {
        public CustomUserManager(IUserStore<Usuario> store, 
                                 IOptions<IdentityOptions> optionsAccessor, 
                                 IPasswordHasher<Usuario> passwordHasher, 
                                 IEnumerable<IUserValidator<Usuario>> userValidators, 
                                 IEnumerable<IPasswordValidator<Usuario>> passwordValidators, 
                                 ILookupNormalizer keyNormalizer, 
                                 IdentityErrorDescriber errors, 
                                 IServiceProvider services, 
                                 ILogger<UserManager<Usuario>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override Task<bool> CheckPasswordAsync(Usuario user, string password)
        {
            string hash = SHA256Hasher.ComputeHash(password,user.salt);
            return Task.FromResult<bool>(hash.Equals(user.password));
        }
    }
}
