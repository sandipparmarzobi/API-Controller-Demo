using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using URF.Core.EF.Trackable;

namespace DomainLayer.Entities
{
    public class Movie : Entity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public MovieGenre Genre { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Director { get; set; }
        public string TrailerURL { get; set; }
        public byte[] Image { get; set; }

        [NotMapped]
        public string ImageBase64 { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Reviews>? Reviews { get; set; }
        public ICollection<ShowTime>? ShowTime { get; set; }
    }
}
