using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using URF.Core.EF.Trackable;

namespace DomainLayer.Entities
{
    //Testin
    public class User : Entity
    {
        [Key]
        public Guid Id { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Email { get; set; }

    }
    
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Password { get; set; }
    }
}