using NyimboProject.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NyimboProject.Models.Authentication
{
    public class Login
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Поле логин не может быть пустым")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле пароль не может быть пустым")]
        public string Password { get; set; }
    }

    [ValidCompare("Password", "ConfirmPassword", ErrorMessage = "Не верное подтверждение пароля")]
    public class Registration
    {
        [Required(ErrorMessage = "Поле имени не может быть пустым")]
        public string NickName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Поле логин не может быть пустым")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Минимальная длинна пароля 8 символов")]
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Минимальная длинна пароля 8 символов")]
        [Required(ErrorMessage = "Подтвердите пароль")]
        public string ConfirmPassword { get; set; }

    }
}