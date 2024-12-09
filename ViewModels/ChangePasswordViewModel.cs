using System.ComponentModel.DataAnnotations;

namespace servicesharing.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Имейл е задължителен.")]
        [EmailAddress(ErrorMessage = "Моля, въведете валиден имейл адрес.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Паролата трябва да бъде поне {2} и не повече от {1} символа.")]
        [DataType(DataType.Password)]
        [Display(Name = "Нова парола")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Паролите не съвпадат.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Потвърдете новата парола.")]
        [DataType(DataType.Password)]
        [Display(Name = "Потвърдете новата парола")]
        public string ConfirmNewPassword { get; set; }
    }
}