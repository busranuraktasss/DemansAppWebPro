using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class AudioBooksMap : IEntityTypeConfiguration<AudioBooks>
    {
        public void Configure(EntityTypeBuilder<AudioBooks> builder)
        {
            builder.ToTable("AudioBooks");
            builder.HasKey(e => e.Id);


            builder.Property(p => p.Name).HasColumnType("string").IsRequired();
            builder.Property(p => p.Subject).HasColumnType("string").IsRequired();
            builder.Property(p => p.Text).HasColumnType("string").IsRequired();
            builder.Property(p => p.UserId).HasColumnType("int").IsRequired();


        }

    }
}
