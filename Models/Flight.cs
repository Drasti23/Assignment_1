using System;
namespace Assignment_1.Models
{
	public class Flight
	{
        public int Id { get;  set;}

        public string? FlightNumber { get; set; }

        public string? Origin { get; set; }

        public string? Destination { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public float Price { get; set; }
    }

}

