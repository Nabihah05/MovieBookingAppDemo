using Microsoft.AspNetCore.Mvc;
using MovieBookingAppDemo.Data;
using MovieBookingAppDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieBookingAppDemo.Controllers
{
    public class BookingSummaryController : Controller
    {


        //connects to the database
        private readonly DataContext _context;

        //uses dependency injection to automatically make the connection
        public BookingSummaryController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            //Load information from database
            var bookings = _context.TicketBookings
                .Include(b => b.Movie)
                .Include(b => b.Cinema)
                .AsQueryable(); //makes data filterable i.e. means you might apply filters to it later

            // Search filter (Booking ID or movie title)
            if (!string.IsNullOrEmpty(search))   //only run search if user typed something 
            {
                bookings = bookings.Where(b =>
                    b.TicketBookingId.ToString().Contains(search) ||
                    b.Movie.Title.Contains(search));
            }

            //Map to ViewModel (combining data from 3 tables into 1)
            var result = await bookings.Select(b => new BookingSummary  // for each booking, create a viewModel
            {
                BookingId = b.TicketBookingId,
                BookingDate = b.ShowDate,

                MovieTitle = b.Movie.Title,
                MovieGenre = b.Movie.Genre,

                CinemaName = b.Cinema.Name,
                CinemaLocation = b.Cinema.Location
            }).ToListAsync(); // execute query and store results in a list 

            //Send the result (ie the list) to the view
            return View(result);
        }
    }
}
