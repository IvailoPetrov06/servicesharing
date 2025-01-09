using System.ComponentModel.DataAnnotations;

namespace servicesharing.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Името е задължително.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Имейл е задължителен.")]
        [EmailAddress(ErrorMessage = "Моля, въведете валиден имейл адрес.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Паролата трябва да е поне {2} и не повече от {1} символа.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Паролите не съвпадат.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Потвърдете паролата.")]
        [DataType(DataType.Password)]
        [Display(Name = "Потвърдете паролата")]
        public string ConfirmPassword { get; set; }
    }
}