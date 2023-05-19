using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class UsersMap : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(e => e.Id);


            builder.Property(p => p.Email).HasColumnType("string").IsRequired();
            builder.Property(p => p.EmergencyPhone).HasColumnType("string").IsRequired();
            builder.Property(p => p.UserName).HasColumnType("string").IsRequired();
            builder.Property(p => p.Surname).HasColumnType("string").IsRequired();
            builder.Property(p => p.Phone).HasColumnType("string").IsRequired();
            builder.Property(p => p.Sex).HasColumnType("boolean").IsRequired();
            builder.Property(p => p.Status).HasColumnType("int").IsRequired();


        }

    }
}
