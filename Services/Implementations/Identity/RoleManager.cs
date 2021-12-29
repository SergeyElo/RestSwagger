using Domain.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services.Contracts.Interfaces.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations.Identity
{
    public class RoleManager : RoleManager<RoleEntity>, IRoleManager
    {
        public RoleManager(
            IRoleStore<RoleEntity> store,
            IEnumerable<IRoleValidator<RoleEntity>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<RoleEntity>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        { }

        public IEnumerable<RoleEntity> GetRoles()
        {
            return Roles;
        }

        public string GetRoleNameById(Guid roleId)
        {
            return Roles.FirstOrDefault(r => r.Id == roleId).Name;
        }

        public IEnumerable<string> GetRolesAsView()
        {
            return Roles
                .Select(role => AsView(role))
                .OrderBy(role => role);
        }

        public string AsView(RoleEntity entity)
        {
            return entity.Name;
        }
    }
}
