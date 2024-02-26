using System;
namespace Assignment_1.Models
{
	public class Booking
	{
        public int Id { get; set; }

        public int ServiceId { get; set; }

        public string? ServiceType { get; set; }

        public DateTime BookingDate { get; set; }

        public double TotalPrice { get; set; }

    }
}

