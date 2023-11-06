using DomainLayer.Entities;
using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace NewDemoProject.Model
{
    public class BookingDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string MovieId { get; set; }

        [Required]
        public string ShowTimeId { get; set; }

        // Seat number is like "1,3,4,5"
        [Required]
        public string SeatNumbers { get; set; }
    }
}