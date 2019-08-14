using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
namespace mRecordSearchList.Notifications
{
    public interface IChangeICFileNumberNotification : IConfirmation
    {
        string Prefix { get; set; }
        string Year { get; set; }
        int Enumeration { get; set; }
        string Suffix { get; set; }
    }
}
