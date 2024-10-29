using System.Collections.Generic;

namespace servicesharing.ViewModels
{
    public class MechanicProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Experience { get; set; }
        public string Certifications { get; set; }
        public double Rating { get; set; }
        public List<ServiceViewModel> Services { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }
    }
}
