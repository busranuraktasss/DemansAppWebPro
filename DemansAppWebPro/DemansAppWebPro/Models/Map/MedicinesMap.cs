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


            builder.Property(p => p.Day).HasColumnType("string").IsRequired();
            builder.Property(p => p.Name).HasColumnType("string").IsRequired();
            builder.Property(p => p.Time).HasColumnType("date").IsRequired();
            builder.Property(p => p.UsageDuration).HasColumnType("string").IsRequired();
            builder.Property(p => p.UsagePurpose).HasColumnType("string").IsRequired();
            builder.Property(p => p.UserId).HasColumnType("int").IsRequired();
            builder.Property(p => p.CompanionId).HasColumnType("int").IsRequired();


        }

    }
}
