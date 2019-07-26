using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClientList.Notifications
{
    public class CreateNewClientNotification : Confirmation, ICreateNewClientNotification
    {
        public string ClientName { get; set; }
        public string OfficeName { get; set; }
    }
}
