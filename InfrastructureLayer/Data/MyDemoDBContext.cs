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

    }
}
