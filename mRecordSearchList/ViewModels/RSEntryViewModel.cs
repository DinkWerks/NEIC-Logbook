using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mRecordSearchList.ViewModels
{
    public class RSEntryViewModel : BindableBase
    {
        private string _rsID;
        private string _rsName;
        private string _rsRequestor;

        public string RecordSearchID
        {
            get { return _rsID; }
            set { SetProperty(ref _rsID, value); }
        }

        public string RecordSearchName
        {
            get { return _rsName; }
            set { SetProperty(ref _rsName, value); }
        }

        public string RecordSearchRequestor
        {
            get { return _rsRequestor; }
            set { SetProperty(ref _rsRequestor, value); }
        }

        public RSEntryViewModel()
        {
            GenerateTestRecordSearches();
        }

        public void GenerateTestRecordSearches()
        {
            RecordSearchID = "A-19-123";
            RecordSearchName = "Test Search";
            RecordSearchRequestor = "Doe, John";
        }
    }
}
