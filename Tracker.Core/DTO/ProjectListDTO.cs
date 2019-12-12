using Prism.Mvvm;
using System;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.DTO
{
    public class ProjectListDTO : BindableBase
    {
        public int Id { get; set; }
        public string ICTypePrefix { get; set; }
        public string ICYear { get; set; }
        public int ICEnumeration { get; set; }
        public string ICSuffix { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public County MainCounty { get; set; }
        public string PLSS { get; set; }
        public DateTime? LastUpdated { get; set; }

        public ProjectListDTO()
        {

        }
    }
}
