using System;
using Assignment_1.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<CarRental> CarRentals { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed some initial data if needed
            modelBuilder.Entity<Flight>().HasData(
                new Flight { Id = 1, FlightNumber = "ABC123", Origin = "City A", Destination = "City B", DepartureTime = DateTime.Now, ArrivalTime = DateTime.Now.AddHours(2), Price = 200 },
                new Flight { Id = 2, FlightNumber = "DEF456", Origin = "City B", Destination = "City C", DepartureTime = DateTime.Now.AddHours(3), ArrivalTime = DateTime.Now.AddHours(5), Price = 250 }
            );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel { Id = 1, Name = "Hotel ABC", Location = "City X", Rating = 4, PricePerNight = 150, IsAvailable = true },
                new Hotel { Id = 2, Name = "Hotel XYZ", Location = "City Y", Rating = 3, PricePerNight = 120, IsAvailable = true }
            );

            modelBuilder.Entity<CarRental>().HasData(
                new CarRental { Id = 1, Model = "Toyota Camry", RentalAgency = "ABC Rentals", PricePerDay = 504, IsAvailable = true },
                new CarRental { Id = 2, Model = "Honda Civic", RentalAgency = "XYZ Rentals", PricePerDay = 45, IsAvailable = false },
                new CarRental { Id = 3, Model = "Ford Mustang", RentalAgency = "123 Rentals", PricePerDay = 70, IsAvailable = true }
                // Add more sample CarRental data as needed
            );

        }

    }
}

