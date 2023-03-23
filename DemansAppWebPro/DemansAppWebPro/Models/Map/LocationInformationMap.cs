using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class LocationInformationMap : IEntityTypeConfiguration<LocationInformation>
    {
        public void Configure(EntityTypeBuilder<LocationInformation> builder)
        {
            builder.ToTable("LocationInformation");
            builder.HasKey(e => e.Id);


            builder.Property(p => p.Lat).HasColumnType("string").IsRequired();
            builder.Property(p => p.Lng).HasColumnType("string").IsRequired();
            builder.Property(p => p.UserId).HasColumnType("int").IsRequired();


        }

    }
}
