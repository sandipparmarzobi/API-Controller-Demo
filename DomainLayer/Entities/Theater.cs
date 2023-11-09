using System.ComponentModel.DataAnnotations;
using URF.Core.EF.Trackable;

namespace DomainLayer.Entities
{
    public class Theater : Entity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public ICollection<Seats>? Seats { get; set; }
        public ICollection<ShowTime>? ShowTime { get; set; }
    }
}
