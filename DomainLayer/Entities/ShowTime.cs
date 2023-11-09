using System.ComponentModel.DataAnnotations;
using URF.Core.EF.Trackable;

namespace DomainLayer.Entities
{
    public class ShowTime : Entity
    {
        [Key]
        public Guid Id { get; set; }
        public char Screen { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid? MovieId { get; set; }
        public Movie? Movie { get; set; }
        public Guid? TheaterId { get; set; }
        public Theater? Theater { get; set; }
        public decimal TicketPrice { get; set; }
        public bool HideShowTime { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Seats>? Seats { get; set; }
    }
}
