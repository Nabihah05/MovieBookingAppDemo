namespace MovieBookingAppDemo.Models
{
    public class BookingSummary
    {
        //movie info
        public string MovieTitle { get; set; }
        public string MovieGenre { get; set; }

        //cinema info
        public string CinemaName { get; set; }
        public string CinemaLocation { get; set; }

        //booking info
        public DateTime BookingDate { get; set; }
        public int BookingId { get; set; }


    }
}
