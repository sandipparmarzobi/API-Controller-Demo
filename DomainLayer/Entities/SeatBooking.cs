using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities
{
    public class SeatBooking
    {
        [Key]
        public Guid Id { get; set; }
        public int SeatNumber { get; set; }
        public bool IsReserved { get; set; }
        public Guid? BookingId { get; set; }
        public Booking? Booking { get; set; }
        public Guid? SeatId { get; set; }
        public Seats? Seats { get; set; }
    }
}
