using System;
using System.ComponentModel.DataAnnotations;

namespace servicesharing.Data.Entities
{
    public class Promotion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително.")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Моля въведете валидна дата.")]
        [DataType(DataType.Date)]
        public DateTime ValidUntil { get; set; }

        [Required(ErrorMessage = "Моля изберете услуга.")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
