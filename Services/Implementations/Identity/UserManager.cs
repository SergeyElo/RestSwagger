using Common.Identity;
using Domain.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Contracts.Interfaces.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Implementations.Identity
{
    public class UserManager : UserManager<UserEntity>, IUserManager
    {
        public UserManager(
            IUserStore<UserEntity> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<UserEntity> passwordHasher,
            IEnumerable<IUserValidator<UserEntity>> userValidators,
            IEnumerable<IPasswordValidator<UserEntity>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<UserEntity>> logger)
                : base(store,
                      optionsAccessor,
                      passwordHasher,
                      userValidators,
                      passwordValidators,
                      keyNormalizer,
                      errors,
                      services,
                      logger)
        { }

        public async Task<Guid> CreateUserAsync(UserEntity user, string role, CancellationToken cancellationToken = default)
        {
            IdentityResult createUserResult = await CreateAsync(user);
            if (!createUserResult.Succeeded)
                //TODO
                throw new Exception(createUserResult.Errors.First().Description);

            //add role to user
            IdentityResult addRoleToUser = await AddToRoleAsync(user, role);
            if (!addRoleToUser.Succeeded)
                //TODO
                throw new Exception(createUserResult.Errors.First().Description);

            return user.Id;
        }
    }

}
