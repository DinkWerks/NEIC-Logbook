using System;
using System.Collections.Generic;
using mRecordSearchList.Notifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;

namespace mRecordSearchList.ViewModels
{
    public class CreateNewRSDialogViewModel : BindableBase, IInteractionRequestAware, IRegionMemberLifetime
    {
        private IRecordSearchService _rss;
        private Prefix _prefix;
        private string _year;
        private int _enumeration;
        private string _suffix;
        private string _projectName;
        private bool _isAcceptEnabled;
        private string _isDistinctWarningVisible;

        private ICreateNewRSNotification _notification;

        public List<Prefix> PrefixChoices { get; private set; }

        public bool IsAcceptEnabled
        {
            get { return _isAcceptEnabled; }
            set { SetProperty(ref _isAcceptEnabled, value); }
        }

        public Prefix Prefix
        {
            get { return _prefix; }
            set
            {
                SetProperty(ref _prefix, value);
                GetNextEnumeration();
                AcceptCommand.RaiseCanExecuteChanged();
            }
        }

        public string Year
        {
            get { return _year; }
            set
            {
                SetProperty(ref _year, value);
                GetNextEnumeration();
                AcceptCommand.RaiseCanExecuteChanged();
            }
        }

        public int Enumeration
        {
            get { return _enumeration; }
            set
            {
                SetProperty(ref _enumeration, value);
                AcceptCommand.RaiseCanExecuteChanged();
            }
        }

        public string Suffix
        {
            get { return _suffix; }
            set { SetProperty(ref _suffix, value); }
        }
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                SetProperty(ref _projectName, value);
                AcceptCommand.RaiseCanExecuteChanged();
            }
        }

        public string IsDistinctWarningVisible
        {
            get { return _isDistinctWarningVisible; }
            set
            {
                SetProperty(ref _isDistinctWarningVisible, value);
            }
        }

        public INotification Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, (ICreateNewRSNotification)value); }
        }

        public DelegateCommand AcceptCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public Action FinishInteraction { get; set; }
        public bool KeepAlive => false;

        public CreateNewRSDialogViewModel(IRecordSearchService recordSearchService)
        {
            _rss = recordSearchService;
            PrefixChoices = new List<Prefix>(RecordSearchPrefixes.Values);

            CancelCommand = new DelegateCommand(Cancel);
            AcceptCommand = new DelegateCommand(Accept, CanAccept);
            IsDistinctWarningVisible = "Hidden";
            Year = DateTime.Today.Year.ToString().ToString().Substring(2, 2);
        }

        private void Cancel()
        {
            _notification.Confirmed = false;
            FinishInteraction?.Invoke();
        }

        private void Accept()
        {
            //Set Data Package Here
            if (ConfirmIDDistinct())
            {
                _notification.Prefix = Prefix.Code;
                _notification.Year = Year;
                _notification.Enumeration = Enumeration;
                _notification.Suffix = Suffix;
                _notification.ProjectName = ProjectName;
                _notification.Confirmed = true;
                FinishInteraction?.Invoke();
            }
            else
            {
                IsDistinctWarningVisible = "Visible";
                Enumeration = _rss.GetNextEnumeration(Prefix.ToString(), Year);
            }
        }

        private bool CanAccept()
        {
            if (Prefix != null && !string.IsNullOrEmpty(Year) && Enumeration > 0 && !string.IsNullOrEmpty(ProjectName))
                return true;
            else
                return false;
        }

        private void GetNextEnumeration()
        {
            if (!string.IsNullOrEmpty(Year) && Prefix != null)
            {
                Enumeration = _rss.GetNextEnumeration(Prefix.ToString(), Year);
            }
        }

        private bool ConfirmIDDistinct()
        {
            return _rss.ConfirmDistinct(Prefix.ToString(), Year, Enumeration, Suffix);
        }
    }
}
