using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace Tracker.Core.StaticTypes
{
    public class OrganizationStandings
    {
        public static readonly OrganizationStanding GoodStanding = new OrganizationStanding(1, "Good Standing", "bullet_green.png", 0);
        public static readonly OrganizationStanding Warning = new OrganizationStanding(2, "Warning", "bullet_orange.png", 1);
        public static readonly OrganizationStanding Denial = new OrganizationStanding(3, "Denied Service", "bullet_red.png", 2);

        public OrganizationStandings(IEFService eFService)
        {
            eFService.OrganizationStandings.ToList();
        }

        public static List<OrganizationStanding> Values { get; private set;}
        /*
        public static IEnumerable<OrganizationStanding> Values
        {
            get
            {
                yield return GoodStanding;
                yield return Warning;
                yield return Denial;
            }
        }
        */
    }

    public class OrganizationStanding : BindableBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Severity { get; set; }
        public ICollection<Organization> Organizations { get; set; }

        public OrganizationStanding(int id, string name, string icon, int severity)
        {
            Id = id;
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
