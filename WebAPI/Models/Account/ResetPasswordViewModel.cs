using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Account
{
    /// <summary>
    /// Модель для сброса пароля
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Пароль должен содержать как минимум 4 символа", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Подтверждение пароля
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Токен для смены пароля
        /// </summary>
        public string Code { get; set; }
    }
}
