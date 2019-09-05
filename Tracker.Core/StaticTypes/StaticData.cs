using System.Collections.Generic;

namespace Tracker.Core.StaticTypes
{
    public static class StaticData
    {
        public static List<string> Recommendations = new List<string>()
        {
            string.Empty,
            "Sensitive for prehistoric, protohistoric, and/or historic cultural resources",
            "Contact appropriate local Native American representatives for information regarding traditional cultural properties",
            "Survey/Contact prof Archaeologist to survey entire project area prior to impact and ground disturbing activities, contact Nat Amer Reps re trad cult props, consult GLO map",
            "The project archaeologist should also contact the appropriate local Native American representatives for information regarding traditional properties that may be located within project boundries"
        };

        public static List<string> RecordSearchTypes = new List<string>()
        {
            "Standard",
            "Inhouse",
            "Copies/Information",
            "Project Review",
            "Record Search - Confidential",
            "Record Search - Non-Confidential",
            "Priority Response",
            "Emergency",
            "Ownership Wide/5 Year Update",
            "Subscription"
        };

        public static List<string> DisclosureLevels = new List<string>()
        {
            string.Empty,
            "Confidential Information Qualified",
            "Registered Forester",
            "Public Information Only"
        };
    }
}
