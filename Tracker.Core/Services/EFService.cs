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
        public DbSet<Staff> Staff { get; set; }
        public DbSet<FeeData> FeeData { get; set; }

        //Secondary Models
        public DbSet<OrganizationStanding> OrganizationStandings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
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
            var StringToProjectNumber = new ValueConverter<ProjectNumber, string>(
                v => v.ToString(),
                v => ParseProjectNumber(v)
                );

            //Model Builder
            modelBuilder.Entity<Project>().OwnsOne(p => p.MailingAddress);
            modelBuilder.Entity<Project>().OwnsOne(p => p.BillingAddress);
            modelBuilder.Entity<Project>().Property(p => p.MainCounty).HasConversion(StringToCounty);
            modelBuilder.Entity<Project>().Property(p => p.AdditionalCounties).HasConversion(StringToAddCounties);
            modelBuilder.Entity<Project>().Property(p => p.ProjectNumber).HasConversion(StringToProjectNumber);

            modelBuilder.Entity<Organization>().OwnsOne(o => o.Address);
            modelBuilder.Entity<Organization>()
                .HasOne<OrganizationStanding>(o => o.OrganizationStanding)
                .WithMany(os => os.Organizations)
                .HasForeignKey(o => o.OrganizationStandingId);

            modelBuilder.Entity<Person>().OwnsOne(p => p.Address);
        }

        #region ConversionFunctions
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

        private ProjectNumber ParseProjectNumber(string projectNumber)
        {
            return ProjectNumbers.AllProjectNumbers.First(p => p.ProjectID == projectNumber);
        }
        #endregion
    }
}
