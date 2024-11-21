﻿using System;
using System.ComponentModel.DataAnnotations;

namespace servicesharing.Models
{
    public class Reservation
    {
       
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime ReservationDate { get; set; }
       
    }
}