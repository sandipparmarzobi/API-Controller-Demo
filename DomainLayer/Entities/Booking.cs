using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities
{
    public class Booking
    {
        [Key]
        public Guid Id { get; set; }
        public int TotalTicket { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Guid? MovieId { get; set; }
        public Movie? Movie { get; set; }
        public Guid? ShowTimeId { get; set; }
        public ShowTime? ShowTime { get; set; }
        public ICollection<SeatBooking>? SeatBooking { get; set; }
    }
}
