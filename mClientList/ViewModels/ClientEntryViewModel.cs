using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using Tracker.Core;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;


namespace mClientList.ViewModels
{
    public class ClientEntryViewModel : BindableBase
    {
        private string _peid;
        private string _clientName;
        private string _officeName;
        private string _standing;

        public string PEID
        {
            get { return _peid; }
            set { SetProperty(ref _peid, value); }
        }

        public string ClientName
        {
            get { return _clientName; }
            set { SetProperty(ref _clientName, value); }
        }

        public string OfficeName
        {
            get { return _officeName; }
            set { SetProperty(ref _officeName, value); }
        }

        public string Standing
        {
            get { return _standing; }
            set { SetProperty(ref _standing, value); }
        }

        public ClientEntryViewModel(IEventAggregator eventAggregator)
        {
            
        }

        public void GenerateClient()
        {
            List<string> namePart1 = new List<string> { "Natural Resources", "Cultual Resources", "Historic", "CRM", "Western", "Shasta", "Butte", "California", "Archaeological" };
            List<string> namePart2 = new List<string> { "Incorperated", "LLC", "Research", "Pacific", "Institute", "Research Program" };
            List<string> office = new List<string> { "Sacramento", "Chico", "San Francisco", "Redding", "Siskiyous", "Butte County", "Sierra" };
            Random ran = RandomProvider.GetThreadRandom();

            PEID = ran.Next(10000).ToString("000000");

            ClientName = namePart1[ran.Next(namePart1.Count)] + " " + namePart2[ran.Next(namePart2.Count)];

            int officeRoll = ran.Next(10);
            if (officeRoll >= 7)
            {
                OfficeName = office[ran.Next(office.Count)];
            }

            Standing = ClientStanding.GoodStanding;
            int standingRoll = ran.Next(10);
            if (standingRoll >= 9)
            {
                Standing = ClientStanding.OnHold;
            }
            else if (standingRoll == 8)
            {
                Standing = ClientStanding.Warning;
            }
        }
    }
}
