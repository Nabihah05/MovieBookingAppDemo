using System.ComponentModel.DataAnnotations;

namespace MovieBookingAppDemo.Models
{
    public class Cinema
    {
        public int CinemaId { get; set; }


        [Required(ErrorMessage = "Please enter a cinema name")]

        public string? Name { get; set; }


        [Required(ErrorMessage = "Please enter a cinema location.")]

        public string? Location { get; set; }


        [Required(ErrorMessage = "Please enter an image.")]

        public string? ImageUrl { get; set; }

        // Navigation property
        public List<TicketBooking> TicketBookings { get; set; } = new List<TicketBooking>();

    }
}