using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities
{
    public class Theater
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capasity { get; set; }
        public ICollection<Seats>? Seats { get; set; }
        public ICollection<ShowTime>? ShowTime { get; set; }
    }
}
