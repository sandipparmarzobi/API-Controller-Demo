using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs
{
    public class MovieDto
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Duration { get; set; }

        [Required]
        public string Director { get; set; }

        [Required]
        public string PosterURL { get; set; }

        [Required]
        public string TrailerURL { get; set; }
    }
}