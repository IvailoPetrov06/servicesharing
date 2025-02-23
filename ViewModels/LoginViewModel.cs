using System.ComponentModel.DataAnnotations;

namespace servicesharing.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Имейл е задължителен.")]
        [EmailAddress(ErrorMessage = "Моля, въведете валиден имейл адрес.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомни ме")]
        public bool RememberMe { get; set; }
    }
}