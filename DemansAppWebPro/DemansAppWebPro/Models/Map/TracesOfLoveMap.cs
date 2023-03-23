using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class TracesOfLoveMap : IEntityTypeConfiguration<TracesOfLove>
    {
        public void Configure(EntityTypeBuilder<TracesOfLove> builder)
        {
            builder.ToTable("TracesOfLove");
            builder.HasKey(e => e.Id);


            builder.Property(p => p.Email).HasColumnType("string").IsRequired();
            builder.Property(p => p.Lat).HasColumnType("string").IsRequired();
            builder.Property(p => p.Lng).HasColumnType("string").IsRequired();
            builder.Property(p => p.Phone).HasColumnType("string").IsRequired();
            builder.Property(p => p.PlaceName).HasColumnType("string").IsRequired();


        }

    }
}
