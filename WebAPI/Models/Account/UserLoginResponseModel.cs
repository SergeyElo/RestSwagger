using System;

namespace WebAPI.Models.Account
{
    public class UserLoginResponseModel
    {
        public Guid? Id { get; set; }
        public string AuthToken { get; set; }
    }
}
