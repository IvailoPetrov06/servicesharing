using System.ComponentModel.DataAnnotations;

namespace servicesharing.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на услугата е задължително.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Цената е задължителна.")]
        [Range(0, double.MaxValue, ErrorMessage = "Цената трябва да е положително число.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Времето за изпълнение е задължително.")]
        public int EstimatedTime { get; set; } // Време за изпълнение в минути
    }
}
