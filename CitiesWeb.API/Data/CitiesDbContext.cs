using CitiesWeb.API.Entities;
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
        public CitiesDbContext(DbContextOptions<CitiesDbContext> options) : base(options)
        {
            Database.EnsureCreated();

        }
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<CityDTO>().HasData(new CityDTO
        //    {
        //        Id = 1,
        //        Name = "København",
        //        Description = "Danmarks Hovedstad",
        //        PointOfInterests = new List<PointOfInterestDTO>() { new PointOfInterestDTO {
        //            Id = 1,
        //            Name = "Tivoli",
        //            Description = "Forlystelsespark"
        //        }, new PointOfInterestDTO
        //        {
        //            Id = 2,
        //            Name = "Rundetårn",
        //            Description = "Rundt"
        //        }
        //    }
        //    });
        //}
    }
}
