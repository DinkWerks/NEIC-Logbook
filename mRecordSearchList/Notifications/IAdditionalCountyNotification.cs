using Prism.Interactivity.InteractionRequest;
using System.Collections.ObjectModel;
using Tracker.Core.StaticTypes;

namespace mRecordSearchList.Notifications
{
    public interface IAdditionalCountyNotification : IConfirmation
    {
        ObservableCollection<County> AdditionalCounties { get; set; }
    }
}
