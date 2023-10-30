using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities
{
    public class ShowTime
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid? MovieId { get; set; }
        public Movie? Movie { get; set; }
        public Guid? TheaterId { get; set; }
        public Theater? Theater { get; set; }
    }
}
