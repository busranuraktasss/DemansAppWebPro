using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class CompanionsMap : IEntityTypeConfiguration<Companions>
    {
        public void Configure(EntityTypeBuilder<Companions> builder)
        {
            builder.ToTable("Companions");
            builder.HasKey(e => e.Id);


            builder.Property(p => p.Email).HasColumnType("string").IsRequired(true);
            builder.Property(p => p.Adress).HasColumnType("string").IsRequired(true);
            builder.Property(p => p.Name).HasColumnType("string").IsRequired(true);
            builder.Property(p => p.Surname).HasColumnType("string").IsRequired(true);
            builder.Property(p => p.Phone).HasColumnType("string").IsRequired(true);
            builder.Property(p => p.Sex).HasColumnType("boolean").IsRequired(false);
            builder.Property(p => p.UserId).HasColumnType("int").IsRequired(false);


        }

    }
}
