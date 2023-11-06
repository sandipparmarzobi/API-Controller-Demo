using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities
{
    public class TheaterDto 
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int Capasity { get; set; }
    }
}
