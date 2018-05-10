using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SiproModelCore.Models;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Sipro.Utilities.Identity
{
    public class RoleStore : IRoleStore<Rol>
    {
        public Task<IdentityResult> CreateAsync(Rol role, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
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

        public Task<Rol> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            if(normalizedRoleName.ToLower().Equals("general")){
                return Task.FromResult(new Rol() { nombre = "General", descripcion = "General" });
            }
            return null;
        }

        public Task<string> GetNormalizedRoleNameAsync(Rol role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(Rol role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.id.ToString());
        }

        public Task<string> GetRoleNameAsync(Rol role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.nombre);
        }

        public Task SetNormalizedRoleNameAsync(Rol role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Rol role, string roleName, CancellationToken cancellationToken)
        {
            role.nombre = roleName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(Rol role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
