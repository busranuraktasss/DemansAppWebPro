using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class PicturesMap : IEntityTypeConfiguration<Pictures>
    {
        public void Configure(EntityTypeBuilder<Pictures> builder)
        {
            builder.ToTable("Pictures");
            builder.HasKey(e => e.Id);


            builder.Property(p => p.Url).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.Text).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.UserId).HasColumnType("int").IsRequired(false);
            builder.Property(p => p.Status).HasColumnType("bit").IsRequired(true);


        }

    }
}
