using DomainLayer.Entities;
using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs
{
    public class ReviewDto
    {
        [Required]
        public int Ratting { get; set; }

        [Required]
        public string Comments { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid MovieId { get; set; }
    }
}