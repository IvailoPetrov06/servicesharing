using System.ComponentModel.DataAnnotations;

namespace servicesharing.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public double EstimatedTime { get; set; }

        public int MechanicId { get; set; }

        public Mechanic Mechanic { get; set; }
    }
}
