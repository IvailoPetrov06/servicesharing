using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace servicesharing.Models
{
    public class Mechanic
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Experience { get; set; }

        public string Certifications { get; set; }

        public double Rating { get; set; }

        public List<Service> Services { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
