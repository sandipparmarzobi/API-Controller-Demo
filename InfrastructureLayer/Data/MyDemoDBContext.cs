using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Data
{
    public class MyDemoDBContext : IdentityDbContext<ApplicationUser>
    {
        public MyDemoDBContext(DbContextOptions options): base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

    }
}
