using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sipro.Dao;
using SiproModel.Models;

namespace Sipro.Utilities.Identity
{
    public class CustomUserManager : UserManager<User>
    {
        public CustomUserManager(IUserStore<User> store, 
                                 IOptions<IdentityOptions> optionsAccessor, 
                                 IPasswordHasher<User> passwordHasher, 
                                 IEnumerable<IUserValidator<User>> userValidators, 
                                 IEnumerable<IPasswordValidator<User>> passwordValidators, 
                                 ILookupNormalizer keyNormalizer, 
                                 IdentityErrorDescriber errors, 
                                 IServiceProvider services, 
                                 ILogger<UserManager<User>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override Task<bool> CheckPasswordAsync(User user, string password)
        {
            string hash = SHA256Hasher.ComputeHash(password, user.Salt);
            return Task.FromResult<bool>(hash.Equals(user.PasswordHash));
        }



	}
}
