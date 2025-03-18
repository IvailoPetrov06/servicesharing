using System.ComponentModel.DataAnnotations;

namespace servicesharing.Data.Entities
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public double EstimatedTime { get; set; }

        public string Details { get; set; }

        public string PriceRange { get; set; }
    }
}