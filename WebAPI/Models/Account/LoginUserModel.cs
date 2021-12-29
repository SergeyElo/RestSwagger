using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Account
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "Поле «E-mail» не заполнено")]
        [EmailAddress(ErrorMessage = "Убедитесь в правильности введенной почты")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле «Пароль» не заполнено")]
        public string Password { get; set; }

        //public bool RememberMe { get; set; }
    }
}
