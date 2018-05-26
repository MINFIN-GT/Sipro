using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utilities;

namespace Identity
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
			if (user != null)
			{
				string hash = SHA256Hasher.ComputeHash(password, user.Salt);
				return Task.FromResult(hash.Equals(user.PasswordHash));
			}
			return Task.FromResult(false);
        }



	}
}
