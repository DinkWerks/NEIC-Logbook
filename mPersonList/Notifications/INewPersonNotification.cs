using Prism.Interactivity.InteractionRequest;

namespace mPersonList.Notifications
{
    public interface INewPersonNotification : IConfirmation
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
