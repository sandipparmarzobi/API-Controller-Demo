using Microsoft.AspNetCore.Identity;

namespace DomainLayer.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;   
        public DateTime? RegistrationDate { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Reviews>? Reviews { get; set; }
    }
}