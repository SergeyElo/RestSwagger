using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Account
{
    public class UserRoleModel
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }
    }
}
