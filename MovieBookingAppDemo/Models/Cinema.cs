namespace MovieBookingAppDemo.Models
{
    public class Cinema
    {
        public int CinemaId { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }

        public string? ImageUrl { get; set; }

        // Navigation property
        public List<TicketBooking> TicketBookings { get; set; } = new List<TicketBooking>();




    }
}
