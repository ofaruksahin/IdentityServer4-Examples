using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions opts) : base(opts)
        {

        }

        public DbSet<CustomUser> CustomUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CustomUser>()
                .HasData(new CustomUser()
                {
                    Id = 1,
                    Email = "ofaruksahin@outlook.com.tr",
                    UserName = "ofaruksahin@outlook.com.tr",
                    Password = "123",
                    City = "İstanbul"
                },
                new CustomUser()
                {
                    Id = 2,
                    Email="harunsahin@outlook.com.tr",
                    UserName = "harunsahin@outlook.com.tr",
                    Password = "123",
                    City = "İstanbul"
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
