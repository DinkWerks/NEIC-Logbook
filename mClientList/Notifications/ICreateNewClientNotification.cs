using Prism.Interactivity.InteractionRequest;

namespace mClientList.Notifications
{
    public interface ICreateNewClientNotification : IConfirmation
    {
        string ClientName { get; set; }
        string OfficeName { get; set; }
    }
}
