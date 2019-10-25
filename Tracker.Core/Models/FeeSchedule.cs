using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Tracker.Core.Models
{
    public class FeeStructures
    {
        public static Dictionary<string, FeeStructure> Structures { get; set; } = new Dictionary<string, FeeStructure>();

        static FeeStructures()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, @"Resources\FeeStructures\");
            var feeStructures = Directory.GetFiles(filePath);

            foreach (string path in feeStructures)
            {
                XElement xmlFile = XElement.Load(path);
                FeeStructure newItem = (from fs in xmlFile.Descendants("Meta")
                                        select new FeeStructure(
                                            path,
                                            (string)fs.Element("Version"),
                                            (string)fs.Element("Name"),
                                            DateTime.Parse((string)fs.Element("Date"))
                                       ))
                                       .Single();
                Structures.Add(newItem.Version, newItem);
            }
        }
    }

    public class FeeStructure
    {
        public string FilePath { get; set; }

        public string Version { get; set; }

        public string Title { get; set; }

        public DateTime CreationDate { get; set; }

        public FeeStructure(string filePath, string version, string title, DateTime creationDate)
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
