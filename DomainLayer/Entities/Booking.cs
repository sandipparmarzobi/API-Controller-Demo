using System.ComponentModel.DataAnnotations;
using URF.Core.EF.Trackable;

namespace DomainLayer.Entities
{
    public class Booking : Entity
    {
        public Booking()
        {
            BookingDate = DateTime.UtcNow;
        }

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
