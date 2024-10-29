using System.ComponentModel.DataAnnotations;

namespace servicesharing.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int MechanicId { get; set; }

        [Required]
        public string Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public Mechanic Mechanic { get; set; }
    }
}
