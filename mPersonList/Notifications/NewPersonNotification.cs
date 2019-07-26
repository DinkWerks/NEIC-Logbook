using Prism.Interactivity.InteractionRequest;

namespace mPersonList.Notifications
{
    public class NewPersonNotification : Confirmation, INewPersonNotification
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
