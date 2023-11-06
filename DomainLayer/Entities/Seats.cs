using System.ComponentModel.DataAnnotations;
using URF.Core.EF.Trackable;

namespace DomainLayer.Entities
{
    public class Seats : Entity
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
