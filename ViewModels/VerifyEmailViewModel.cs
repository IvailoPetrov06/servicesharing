using System.ComponentModel.DataAnnotations;

namespace servicesharing.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Изисква се имейл.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}