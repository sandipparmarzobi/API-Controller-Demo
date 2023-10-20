using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Data
{
    public class MyDemoDBContext : DbContext
    {
        public MyDemoDBContext(DbContextOptions<MyDemoDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }

    }
}
