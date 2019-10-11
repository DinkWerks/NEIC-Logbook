using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using Tracker.Core.Models;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Services
{
    public class EFService : DbContext, IEFService
    {
        //Principle Models
        public DbSet<Project> Projects { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Staff> Staffs { get; set; }

        //Secondary Models
        //public DbSet<County> Counties { get; set; }
        public DbSet<ProjectNumber> ProjectNumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Conversions
            var StringToCounty = new ValueConverter<County, string>(
                v => v.ToString(),
                v => ParseCounty(v)
            );
            var StringToAddCounties = new ValueConverter<ObservableCollection<County>, string>(
                v => WriteAdditionalCounties(v),
                v => ParseAdditionalCounties(v)
            );

            //Model Builder
            modelBuilder.Entity<Project>().OwnsOne(p => p.MailingAddress);
            modelBuilder.Entity<Project>().OwnsOne(p => p.BillingAddress);
            modelBuilder.Entity<Project>().Property(p => p.MainCounty).HasConversion(StringToCounty);
            modelBuilder.Entity<Project>().Property(p => p.AdditionalCounties).HasConversion(StringToAddCounties);

            modelBuilder.Entity<Organization>().OwnsOne(o => o.Address);
            modelBuilder.Entity<Person>().OwnsOne(p => p.Address);
        }

        private ObservableCollection<County> ParseAdditionalCounties(string additionalCounties)
        {
            ObservableCollection<County> returnCollection = new ObservableCollection<County>();
            if (!string.IsNullOrWhiteSpace(additionalCounties))
            {
                foreach (string individualCounty in additionalCounties.Split(','))
                {
                    returnCollection.Add(ParseCounty(individualCounty));
                }
            }
            return returnCollection;
        }

        private County ParseCounty(string countyName)
        {
            return Counties.Values.First(c => c.Name == countyName);
        }

        private string WriteAdditionalCounties(ObservableCollection<County> additionalCounties)
        {
            string returnValue = "";
            foreach (County county in additionalCounties)
            {
                returnValue += county.Name + ",";
            }
            return returnValue.TrimEnd(',');
        }
    }
}
