using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class TraceOfLovesMap : IEntityTypeConfiguration<TraceOfLoves>
    {
        public void Configure(EntityTypeBuilder<TraceOfLoves> builder)
        {
            builder.ToTable("TraceOfLoves");
            builder.HasKey(e => e.Id);


            builder.Property(p => p.Email).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.Lat).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.Lng).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.Phone).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.PlaceName).HasColumnType("string").IsRequired(false);


        }

    }
}
