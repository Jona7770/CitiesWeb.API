using CitiesWeb.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitiesWeb.API.Data
{
    public class CitiesDbContext : DbContext
    {
        public CitiesDbContext(DbContextOptions<CitiesDbContext> options) : base(options)
        {
            Database.Migrate();

        }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }
    }
}
