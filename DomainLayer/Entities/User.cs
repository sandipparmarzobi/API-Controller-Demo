using System.ComponentModel.DataAnnotations;
using URF.Core.EF.Trackable;

namespace DomainLayer.Entities
{
    //SP: This is a testing entity for demo it is not used for Movie Project.
    public class User : Entity
    {
        [Key]
        public Guid Id { get; set; } 
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string Email { get; set; } = string.Empty;   
    }
}