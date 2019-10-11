using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Models
{
    class Countyzzz
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(5)]
        public string Abbreviation { get; set; }
        [MaxLength(5)]
        public string ReportAbbr { get; set; }
        [MaxLength(10)]
        public string ICCurator { get; set; }

        public Countyzzz(string name, string abbreviation, string reportAbbr, int number, string icCurator)
        {
            Name = name;
            Abbreviation = abbreviation;
            ReportAbbr = reportAbbr;
            Id = number;
            ICCurator = icCurator;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
