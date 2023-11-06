using System.ComponentModel.DataAnnotations;
using URF.Core.EF.Trackable;

namespace DomainLayer.Entities
{
    public class Reviews : Entity
    {
        [Key]
        public Guid Id { get; set; }
        public int Ratting { get; set; }
        public string Comments { get; set; }
        public DateTime ReviewDate { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Guid? MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
