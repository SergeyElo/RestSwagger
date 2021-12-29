using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Account
{
    public class CreateUserModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Поле «E-mail» не заполнено")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле «Пароль» не заполнено")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Required(ErrorMessage = "Поле «Подтверждение пароля» не заполнено")]
        public string PasswordConfirm { get; set; }

        //[Required(ErrorMessage = "Поле «Имя пользователя» не заполнено")]
        //public string UserName { get; set; }
    }
}
