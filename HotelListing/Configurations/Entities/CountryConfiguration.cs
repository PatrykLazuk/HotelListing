using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Configurations.Entities
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
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
        }
    }
}
