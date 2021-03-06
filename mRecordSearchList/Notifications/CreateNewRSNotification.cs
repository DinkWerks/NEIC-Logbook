﻿using Prism.Interactivity.InteractionRequest;

namespace mRecordSearchList.Notifications
{
    public class CreateNewRSNotification : Confirmation, ICreateNewRSNotification
    {
        public string Prefix { get; set; }
        public string Year { get; set; }
        public int Enumeration { get; set; }
        public string Suffix { get; set; }
        public string ProjectName { get; set; }
    }
}
