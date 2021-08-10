using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    ShortName = "JM",
                    Name = "Jamaica"
                },
                new Country
                {
                    Id = 2,
                    ShortName = "BS",
                    Name = "Bahamas"
                },
                new Country
                {
                    Id = 3,
                    ShortName = "CI",
                    Name = "Cayman Island"
                }
                );

            builder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Sandals Resorts and Spa",
                    Address = "Negril",
                    CountryId = 1,
                    Rating = 4.5

                },
                new Hotel
                {
                    Id = 3,
                    Name = "Comfort Suites",
                    Address = "George Town",
                    CountryId = 3,
                    Rating = 3.5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Grand Paladium",
                    Address = "Nassua",
                    CountryId = 2,
                    Rating = 4
                }
                );
        }
    }
}
