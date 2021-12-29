using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.Interfaces.Identity
{
    public interface IRoleManager
    {
        string GetRoleNameById(Guid roleId);
    }
}
