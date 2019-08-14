using Prism.Interactivity.InteractionRequest;

namespace mRecordSearchList.Notifications
{
    public class ChangeICFileNumberNotification : Confirmation, IChangeICFileNumberNotification
    {
        public string Prefix { get; set; }
        public string Year { get; set; }
        public int Enumeration { get; set; }
        public string Suffix { get; set; }
    }
}
