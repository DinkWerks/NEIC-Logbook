using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Tracker.Core.StaticTypes
{
    public static class ProjectNumbers
    {
        private static List<ProjectNumber> _projectNumbers = new List<ProjectNumber>();
        private static List<ProjectNumber> _activeProjectNumbers = new List<ProjectNumber>();

        public static List<ProjectNumber> AllProjectNumbers
        {
            get { return _projectNumbers; }
            set { _projectNumbers = value; }
        }

        public static List<ProjectNumber> ActiveProjectNumbers
        {
            get { return _activeProjectNumbers; }
            set { _activeProjectNumbers = value; }
        }

        //Static Constructor
        static ProjectNumbers()
        {
            //Read XML HERE
            XElement xmlFile = XElement.Load(@"Resources\ProjectNumbers.xml");

            IEnumerable<ProjectNumber> output = from item in xmlFile.Descendants("Number")
                                                select new ProjectNumber(
                                                    (string)item.Element("Name"),
                                                    (string)item.Element("Description"),
                                                    (bool)item.Attribute("IsActive")
                                                );
            AllProjectNumbers = output.ToList();
            ActiveProjectNumbers = AllProjectNumbers.Where(r => r.IsActive).ToList();
        }


    }

    public class ProjectNumber
    {
        public string ProjectID { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ProjectNumber(string projectID, string description, bool isActive)
        {
            ProjectID = projectID;
            Description = description;
            IsActive = isActive;
        }

        public override string ToString()
        {
            return ProjectID;
        }
    }
}
