using System;
using Assignment_1.Data;
using Assignment_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment_1.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action method for listing hotels
        public IActionResult Index()
        {
            var hotels = _context.Hotels.ToList();
            return View(hotels);
        }

        [HttpPost]
        public async Task<IActionResult> Book(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            // Add booking logic here using the ApplicationDbContext
            var booking = new Booking
            {
                ServiceId = hotel.Id,
                ServiceType = "Hotel",
                BookingDate = DateTime.Now,
                TotalPrice = hotel.PricePerNight // Adjust as needed
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

            // Retrieve hotel details
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == booking.ServiceId);

            // Pass hotel details and booking details to the view
            ViewData["Hotel"] = hotel;
            ViewData["BookingId"] = booking.Id;

            return View();
        }

        // Action method for displaying hotel details
        public IActionResult Details(int id)
        {
            var hotel = _context.Hotels.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        //public IActionResult SearchByName(string hotelName)
        //{
        //    var hotels = _context.Hotels.Where(h => h.Name.Contains(hotelName)).ToList();
        //    return View("Index", hotels);
        //}

        //// Action method for searching hotels by location
        //public IActionResult SearchByLocation(string location)
        //{
        //    var hotels = _context.Hotels.Where(h => h.Location.Contains(location)).ToList();
        //    return View("Index", hotels);
        //}

        // Action method for searching hotels
        //public IActionResult Search(string searchTerm)
        //{
        //    var hotels = _context.Hotels.Where(h => h.Name.Contains(searchTerm) || h.Location.Contains(searchTerm)).ToList();
        //    return View(hotels);
        //}

        public async Task<IActionResult> Search(string searchString)
        {
            var hotelQuery = from h in _context.Hotels
                             select h;
            bool searchPerformed = !String.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
                hotelQuery = hotelQuery.Where(h => h.Name.Contains(searchString)
                                               || h.Location.Contains(searchString)
                                                );
            }
            var hotels = await hotelQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", hotels);
        }

        //  var searchItem = await _context.flights.Include(f=>f.Origin).Include(f=>f.Airline).Include(f=>f.Destination).Where(f => f.Airline.Contains(searchString) 
        //                                        || f.Origin.Contains(searchString) ||
        //                                        f.Destination.Contains(searchString)
        //                                        ).ToListAsync();
        //    return View(searchItem);
        //}
        //}
        //public async Task<IActionResult> SearchByLocation(string searchString)
        //{
        //    var hotelQuery = from h in _context.Hotels
        //                     select h;
        //    bool searchPerformed = !String.IsNullOrEmpty(searchString);
        //    if (searchPerformed)
        //    {
        //        hotelQuery = hotelQuery.Where(h => h.Location.Contains(searchString)
        //                                        );
        //    }
        //    var hotels = await hotelQuery.ToListAsync();
        //    ViewData["SearchPerformed"] = searchPerformed;
        //    ViewData["SearchString"] = searchString;
        //    return View("Index", hotels);
        //}



        // Action method for creating a new hotel
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Hotels.Add(hotel);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // Action method for editing hotel details
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var hotel = _context.Hotels.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        public IActionResult Edit(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(hotel).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // Action method for deleting a hotel
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}


