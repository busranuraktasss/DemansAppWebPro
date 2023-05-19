using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class MotivationSentencesMap : IEntityTypeConfiguration<MotivationSentences>
    {
        public void Configure(EntityTypeBuilder<MotivationSentences> builder)
        {
            builder.ToTable("MotivationSentences");
            builder.HasKey(e => e.Id);


            builder.Property(p => p.Name).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.Text).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.UserId).HasColumnType("int").IsRequired(false);
            builder.Property(p => p.Status).HasColumnType("bit").IsRequired(true);


        }

    }
}
