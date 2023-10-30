using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public MovieGenre Genre { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Director { get; set; }
        public string PosterURL { get; set; }
        public string TrailerURL { get; set; }
    }
}
