using Microsoft.AspNetCore.Mvc;
using Assignment_1.Models;  
using Assignment_1.Data; // Make sure to import the data namespace
using Microsoft.EntityFrameworkCore;

namespace Assignment_1.Controllers
{
    public class FlightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        // Action method for listing flights
        public IActionResult Index()
        {
            var flights = _context.Flights.ToList();
            return View(flights);
        }

        public IActionResult ConfirmBook(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            // Retrieve flight details
            var flight = _context.Flights.FirstOrDefault(f => f.Id == booking.ServiceId);

            // Pass flight details and booking details to the view
            ViewData["BookingId"] = booking.Id;
            ViewData["Flight"] = flight;

            return View();
        }





        // Action method for displaying flight details
        public IActionResult Details(int id)
        {
            var flight = _context.Flights.Find(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Flight/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightNumber,Origin,Destination,DepartureTime,ArrivalTime,Price")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        //public IActionResult Search(string origin, string destination, DateTime departureDate)
        //{
        // Retrieve flights based on search parameters
        //var flights = _context.Flights
        //.Where(f => f.Origin.Contains(origin) && f.Destination.Contains(destination) && f.DepartureTime.Date == departureDate.Date)
        //.ToList();

        // Pass the search results to the Search view
        //return View(flights);
        //}

        public async Task<IActionResult> Search1(string searchString)
        {
            var flightQuery = from f in _context.Flights
                              select f;
            bool searchPerformed = !String.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
                flightQuery = flightQuery.Where(f => f.FlightNumber.Contains(searchString)
                                                || f.Origin.Contains(searchString) ||
                                                f.Destination.Contains(searchString)
                                                );
            }
            var flights = await flightQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", flights);

            //  var searchItem = await _context.flights.Include(f=>f.Origin).Include(f=>f.Airline).Include(f=>f.Destination).Where(f => f.Airline.Contains(searchString) 
            //                                        || f.Origin.Contains(searchString) ||
            //                                        f.Destination.Contains(searchString)
            //                                        ).ToListAsync();
            //    return View(searchItem);
            //}
        }
        public async Task<IActionResult> Search2(decimal price)
        {
            var flightQuery = from f in _context.Flights
                              select f;
            bool decimalInitialized = price != 0;


            if (decimalInitialized)
            {
                flightQuery = flightQuery.Where(f => f.Price.Equals(price));
            }
            var flights = await flightQuery.ToListAsync();
            ViewData["SearchPerformed"] = decimalInitialized;
            ViewData["SearchString"] = price;
            return View("Index", flights);

        }

        [HttpPost]
        public async Task<IActionResult> Book(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            // Add your booking logic here using the ApplicationDbContext
            var booking = new Booking
            {
                ServiceId = flight.Id,
                ServiceType = "Flight",
                BookingDate = DateTime.Now,
                TotalPrice = flight.Price
                // Set other properties as needed
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Set success message in TempData
            TempData["SuccessMessage"] = "Booking successfully made.";

            return RedirectToAction("ConfirmBook", new { bookingId = booking.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flight/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
