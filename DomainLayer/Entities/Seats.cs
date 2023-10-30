using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities
{
    public class Seats
    {
        [Key]
        public Guid Id { get; set; }
        public int SeatNumber { get; set; }
        public bool IsReserved { get; set; }
        public Guid? TheaterId { get; set; }
        public Theater? Theater { get; set; }
        public Guid? ShowTimeId { get; set; }
        public ShowTime? ShowTime { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<SeatBooking>? SeatBooking { get; set; }
    }
}
