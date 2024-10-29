using System.ComponentModel.DataAnnotations;

namespace servicesharing.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Изберете механик.")]
        public int MechanicId { get; set; }

        [Required(ErrorMessage = "Оценката е задължителна.")]
        [Range(1, 5, ErrorMessage = "Оценката трябва да бъде между 1 и 5.")]
        public int Rating { get; set; }

        [MaxLength(500, ErrorMessage = "Коментарът не може да е по-дълъг от 500 знака.")]
        public string Comment { get; set; }
    }
}
