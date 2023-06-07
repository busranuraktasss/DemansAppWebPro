using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class AdminsMap : IEntityTypeConfiguration<Admins>
    {
        public void Configure(EntityTypeBuilder<Admins> builder)
        {
            builder.ToTable("Admins");
            builder.HasKey(e => e.Id);

            builder.Property(p => p.Username).HasColumnType("string").IsRequired();
            builder.Property(p => p.Password).HasColumnType("string").IsRequired();
            


        }

    }
}
