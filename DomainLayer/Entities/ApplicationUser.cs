using Microsoft.AspNetCore.Identity;

namespace DomainLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Reviews>? Reviews { get; set; }
    }
}