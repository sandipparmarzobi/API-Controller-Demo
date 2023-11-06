using System.ComponentModel.DataAnnotations;
using URF.Core.EF.Trackable;

namespace DomainLayer.Entities
{
    public class SeatBooking : Entity
    {
        [Key]
        public Guid Id { get; set; }
        public int SeatNumber { get; set; }
        public Guid? BookingId { get; set; }
        public Booking? Booking { get; set; }
        public Guid? SeatId { get; set; }
        public Seats? Seats { get; set; }
    }
}
