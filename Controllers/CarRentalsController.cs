using System;
using Assignment_1.Data;
using Assignment_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment_1.Controllers
{
    public class CarRentalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarRentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action method for listing car rentals
        public IActionResult Index()
        {
            var carRentals = _context.CarRentals.ToList();
            return View(carRentals);
        }

        // Action method for displaying car rental details
        public IActionResult Details(int id)
        {
            var carRental = _context.CarRentals.Find(id);
            if (carRental == null)
            {
                return NotFound();
            }
            return View(carRental);
        }

        [HttpPost]
        public async Task<IActionResult> Book(int id)
        {
            var carRental = await _context.CarRentals.FindAsync(id);
            if (carRental == null)
            {
                return NotFound();
            }

            // Add booking logic here using the ApplicationDbContext
            var booking = new Booking
            {
                ServiceId = carRental.Id,
                ServiceType = "CarRental",
                BookingDate = DateTime.Now,
                TotalPrice = carRental.PricePerDay // Adjust as needed
                                                   // Add other booking properties as needed
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Set success message to display on the Book view
            TempData["SuccessMessage"] = "Booking successfully made.";

            return RedirectToAction("ConfirmBook", new { bookingId = booking.Id });
        }


        public IActionResult ConfirmBook(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            // Retrieve car rental details
            var carRental = _context.CarRentals.FirstOrDefault(c => c.Id == booking.ServiceId);

            // Pass car rental details and booking details to the view
            ViewData["CarRental"] = carRental;
            ViewData["BookingId"] = booking.Id;

            return View();
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var carrentalQuery = from cr in _context.CarRentals
                             select cr;
            bool searchPerformed = !String.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
                carrentalQuery = carrentalQuery.Where(cr => cr.Model.Contains(searchString)
                                               || cr.RentalAgency.Contains(searchString)
                                                );
            }
            var car = await carrentalQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", car);
        }

        // Action method for creating a new car rental
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CarRental carRental)
        {
            if (ModelState.IsValid)
            {
                _context.CarRentals.Add(carRental);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(carRental);
        }

        // Action method for editing car rental details
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var carRental = _context.CarRentals.Find(id);
            if (carRental == null)
            {
                return NotFound();
            }
            return View(carRental);
        }

        [HttpPost]
        public IActionResult Edit(int id, CarRental carRental)
        {
            if (id != carRental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(carRental).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(carRental);
        }

        // Action method for deleting a car rental
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var carRental = _context.CarRentals.Find(id);
            if (carRental == null)
            {
                return NotFound();
            }
            return View(carRental);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var carRental = _context.CarRentals.Find(id);
            _context.CarRentals.Remove(carRental);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}


