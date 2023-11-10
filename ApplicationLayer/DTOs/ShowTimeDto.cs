using DomainLayer.Entities;
using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs
{
    public class ShowTimeDto
    {
        [Required(ErrorMessage = "Screen is required.")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Screen must be a single character.")]
        public string Screen { get; set; }

        [Required(ErrorMessage = "Ticket Price is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid ticket price. Enter a valid number with up to two decimal places.")]
        public string TicketPrice { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]  
        public string EndTime { get; set; }

        [Required(ErrorMessage = "Movie is required.")]
        public string Movie { get; set; }

        [Required(ErrorMessage = "Theater is required.")]
        public string Theater { get; set; }

        public bool? HideShowTime { get; set; }
    }
}