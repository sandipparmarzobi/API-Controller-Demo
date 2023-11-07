using DomainLayer.Entities;
using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs
{
    public class ShowTimeDto
    {
        [Required]
        public char Screen { get; set; }

        [Required]
        public decimal TicketPrice { get; set; }

        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
        
        [Required]
        public Guid? MovieId { get; set; }

        [Required]
        public Guid? TheaterId { get; set; }
    }
}