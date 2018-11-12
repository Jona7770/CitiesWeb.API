using CitiesWeb.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWeb.API.Data
{
    public class CitiesDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<City>().HasData(new City
            {
                Id = 1,
                Name = "København",
                Description = "Danmarks Hovedstad",
                PointOfInterests = new List<PointOfInterest>() { new PointOfInterest {
                    Id = 1,
                    Name = "Tivoli",
                    Description = "Forlystelsespark"
                }, new PointOfInterest
                {
                    Id = 2,
                    Name = "Rundetårn",
                    Description = "Rundt"
                }
            }
            });
        }
    }
}
