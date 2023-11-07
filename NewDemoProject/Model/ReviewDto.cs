using DomainLayer.Entities;
using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace NewDemoProject.Model
{
    public class ReviewDto
    {
        [Required]
        public int Ratting { get; set; }

        [Required]
        public string Comments { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string MovieId { get; set; }
    }
}