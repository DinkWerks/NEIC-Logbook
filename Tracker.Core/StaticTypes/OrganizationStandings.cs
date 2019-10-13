using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tracker.Core.Models;

namespace Tracker.Core.StaticTypes
{
    public class OrganizationStandings
    {
        public static readonly OrganizationStanding GoodStanding = new OrganizationStanding("Good Standing", "bullet_green.png", 0);
        public static readonly OrganizationStanding Warning = new OrganizationStanding("Warning", "bullet_orange.png", 1);
        public static readonly OrganizationStanding Denial = new OrganizationStanding("Denied Service", "bullet_red.png", 2);

        public static IEnumerable<OrganizationStanding> Values
        {
            get
            {
                yield return GoodStanding;
                yield return Warning;
                yield return Denial;
            }
        }
    }

    public class OrganizationStanding : BindableBase
    {
        [Key]
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Severity { get; set; }
        public ICollection<Organization> Organizations { get; set; }

        public OrganizationStanding(string name, string icon, int severity)
        {
            Name = name;
            Icon = icon;
            Severity = severity;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
