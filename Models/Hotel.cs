using System;
namespace Assignment_1.Models
{
	public class Hotel
	{
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public int Rating { get; set; }

        public float PricePerNight { get; set; }

        public bool IsAvailable { get; set; }
    }
}

