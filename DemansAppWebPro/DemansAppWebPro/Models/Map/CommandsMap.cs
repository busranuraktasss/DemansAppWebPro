using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class CommandsMap : IEntityTypeConfiguration<Commands>
    {
        public void Configure(EntityTypeBuilder<Commands> builder)
        {
            builder.ToTable("Commands");
            builder.HasKey(e => e.Id);


            builder.Property(p => p.ProcessName).HasColumnType("string").IsRequired();
            builder.Property(p => p.Status).HasColumnType("boolean").IsRequired();
            builder.Property(p => p.UserId).HasColumnType("int").IsRequired(false);
            builder.Property(p => p.CompanionId).HasColumnType("int").IsRequired(false);

        }

    }
}
