using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Identity
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class User :  IdentityUser
    {
        public String Salt { get; set; }
        public virtual ICollection<IdentityRole> Roles { get; } = new List<IdentityRole>();
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; } = new List<IdentityUserClaim<string>>();

        public User(){
            
        }



    }
}
