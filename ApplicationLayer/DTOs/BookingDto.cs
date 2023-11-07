using DomainLayer.Entities;
using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs
{
    public class BookingDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid MovieId { get; set; }

        [Required]
        public Guid ShowTimeId { get; set; }

        // Seat number is like "1,3,4,5"
        [Required]
        //[RegularExpression(@"^\d+$", ErrorMessage = "SeatNumbers must contain only numeric characters.")]   
        public string SeatNumbers { get; set; }
    }
}