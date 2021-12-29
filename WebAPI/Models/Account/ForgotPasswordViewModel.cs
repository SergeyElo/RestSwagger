using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Account
{
    /// <summary>
    /// Модель для отправки ссылки для сброса пароля
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Адрес электронной почты пользователя
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
