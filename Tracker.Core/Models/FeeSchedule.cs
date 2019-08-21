using Prism.Mvvm;
using System;

namespace Tracker.Core.Models
{
    public class FeeSchedule : BindableBase
    {
        public string FilePath { get; set; }

        public string Version { get; set; }

        public string Title { get; set; }

        public DateTime CreationDate { get; set; }

        public FeeSchedule(string filePath, string version, string title, DateTime creationDate)
        {
            FilePath = filePath;
            Version = version;
            Title = title;
            CreationDate = creationDate;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
