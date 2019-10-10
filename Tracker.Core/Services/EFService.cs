using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using Tracker.Core.Models;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Services
{
    public class EFService : DbContext, IEFService
    {

        //public DbSet<RecordSearch> tblRecordSearches { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Staff> Staffs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().OwnsOne(p => p.MailingAddress);
            modelBuilder.Entity<Project>().OwnsOne(p => p.BillingAddress);
        }
    }
}
