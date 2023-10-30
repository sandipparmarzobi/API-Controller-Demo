using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Data
{
    public class MyDemoDBContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public MyDemoDBContext(DbContextOptions options): base(options)
        {
        }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Theater> Theater { get; set; }
        public DbSet<ShowTime> ShowTime { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Seats> Seats { get; set; }
        public DbSet<SeatBooking> SeatBooking { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
    }
}
