using Prism.Interactivity.InteractionRequest;
using System.Collections.ObjectModel;
using Tracker.Core.StaticTypes;

namespace mRecordSearchList.Notifications
{
    class AdditionalCountyNotification : Confirmation, IAdditionalCountyNotification
    {
        public ObservableCollection<County> AdditionalCounties { get; set; }
    }
}
