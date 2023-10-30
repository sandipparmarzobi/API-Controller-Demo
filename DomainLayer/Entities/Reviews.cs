using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities
{
    public class Reviews
    {
        [Key]
        public Guid Id { get; set; }
        public int Ratting { get; set; }
        public string Comments { get; set; }
        public DateTime ReviewDate { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Guid? MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
