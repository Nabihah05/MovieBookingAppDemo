namespace MovieBookingAppDemo.Models
{
    public class TicketBooking
    {
        public int TicketBookingId { get; set; }

        // Foreign Keys
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }

        public DateTime ShowDate { get; set; }

    }
}
