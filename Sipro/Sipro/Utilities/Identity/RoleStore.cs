using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SiproModel.Models;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Sipro.Utilities.Identity
{
    public class RoleStore : IRoleStore<Rol>
    {
        public async Task<IdentityResult> CreateAsync(Rol role, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(Rol role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<Rol> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Rol> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            if(normalizedRoleName.ToLower().Equals("general")){
                return new Rol() { nombre = "General", descripcion = "General" };
            }
            return null;
        }

        public Task<string> GetNormalizedRoleNameAsync(Rol role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetRoleIdAsync(Rol role, CancellationToken cancellationToken)
        {
            return role.id.ToString();
        }

        public async Task<string> GetRoleNameAsync(Rol role, CancellationToken cancellationToken)
        {
            return role.nombre;
        }

        public async Task SetNormalizedRoleNameAsync(Rol role, string normalizedName, CancellationToken cancellationToken)
        {
            
        }

        public async Task SetRoleNameAsync(Rol role, string roleName, CancellationToken cancellationToken)
        {
            role.nombre = roleName;
        }

        public Task<IdentityResult> UpdateAsync(Rol role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
