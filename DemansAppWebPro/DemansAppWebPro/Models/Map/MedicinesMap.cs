using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Models.Map
{
    public class MedicinesMap : IEntityTypeConfiguration<Medicines>
    {
        public void Configure(EntityTypeBuilder<Medicines> builder)
        {
            builder.ToTable("Medicines");
            builder.HasKey(e => e.Id);


            builder.Property(p => p.Name).HasColumnType("string").IsRequired();
            builder.Property(p => p.UsageDuration).HasColumnType("string").IsRequired();
            builder.Property(p => p.UsagePurpose).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.StartDate).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.EndDate).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.Moon).HasColumnType("bit").IsRequired();
            builder.Property(p => p.Afternoon).HasColumnType("bit").IsRequired();
            builder.Property(p => p.Evening).HasColumnType("bit").IsRequired();
            builder.Property(p => p.Night).HasColumnType("bit").IsRequired();
            builder.Property(p => p.MoonTime).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.AfternoonTime).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.EveningTime).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.NightTime).HasColumnType("string").IsRequired(false);
            builder.Property(p => p.UserId).HasColumnType("int").IsRequired(false);

    }

    }
}
