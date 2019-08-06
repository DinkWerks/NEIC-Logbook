using Prism.Mvvm;
using System.Collections.Generic;

namespace Tracker.Core.StaticTypes
{
    public class ClientStandings
    {
        public static readonly ClientStanding GoodStanding = new ClientStanding("Good Standing", "bullet_green.png", 0);
        public static readonly ClientStanding Warning = new ClientStanding("Warning", "bullet_orange.png", 1);
        public static readonly ClientStanding Denial = new ClientStanding("Denied Service", "bullet_red.png", 2);

        public static IEnumerable<ClientStanding> Values
        {
            get
            {
                yield return GoodStanding;
                yield return Warning;
                yield return Denial;
            }
        }
    }

    public class ClientStanding : BindableBase
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Severity { get; set; }

        public ClientStanding(string name, string icon, int severity)
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
