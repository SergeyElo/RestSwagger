using Domain.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Contracts.Interfaces.Identity
{
    public interface IUserManager
    {
        Task<Guid> CreateUserAsync(UserEntity user, string role, CancellationToken cancellationToken = default(CancellationToken));
    }
}
