
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using DemansAppWebPro.Models.Map;
using DemansAppWebPro.Models;

namespace DemansAppWebPro.Models
{
    public class EntitiesContext : DbContext
    {
        public EntitiesContext(DbContextOptions<EntitiesContext> options) : base(options)
        {
            //ChangeTracker.AutoDetectChangesEnabled = true;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AudioBooksMap());
            modelBuilder.ApplyConfiguration(new CommandsMap());
            modelBuilder.ApplyConfiguration(new CompanionsMap());
            modelBuilder.ApplyConfiguration(new LocationInformationMap());
            modelBuilder.ApplyConfiguration(new MedicinesMap());
            modelBuilder.ApplyConfiguration(new MotivationSentencesMap());
            modelBuilder.ApplyConfiguration(new PicturesMap());
            modelBuilder.ApplyConfiguration(new TraceOfLovesMap());
            modelBuilder.ApplyConfiguration(new UsersMap());
            modelBuilder.ApplyConfiguration(new AdminsMap());

            base.OnModelCreating(modelBuilder);
        }

        //....
        public DbSet<AudioBooks> AudioBooks { get; set; }
        public DbSet<Commands> Commands { get; set; }
        public DbSet<Companions> Companions { get; set; }
        public DbSet<LocationInformation> LocationInformation { get; set; }
        public DbSet<Medicines> Medicines { get; set; }
        public DbSet<MotivationSentences> MotivationSentences { get; set; }
        public DbSet<Pictures> Pictures { get; set; }
        public DbSet<TraceOfLoves> TraceOfLoves{ get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Admins> Admins { get; set; }
       
    }
}
