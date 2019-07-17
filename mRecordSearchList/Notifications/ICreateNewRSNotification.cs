using Prism.Interactivity.InteractionRequest;

namespace mRecordSearchList.Notifications
{
    public interface ICreateNewRSNotification : IConfirmation
    {
        string Prefix { get; set; }
        string Year { get; set; }
        int Enumeration { get; set; }
        string Suffix { get; set; }
        string ProjectName { get; set; }
    }
}
