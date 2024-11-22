using servicesharing.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace servicesharing.ViewModels
{
    public class ReservationViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Email е задължителен.")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Дата и час на резервация е задължителна.")]
        public DateTime ReservationDate { get; set; }

        public List<Service> Services { get; set; }

    }
}
