using System;
using mClientList.Notifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Tracker.Core.Services;

namespace mClientList.ViewModels
{
    public class NewClientDialogViewModel : BindableBase, IInteractionRequestAware
    {
        private ICreateNewClientNotification _notification;
        private IClientService _cs;
        private string _clientName;
        private string _offficeName;
        private string _isDistinctWarningVisible = "Hidden";
        private bool _canSubmit;

        public string ClientName
        {
            get { return _clientName; }
            set {
                SetProperty(ref _clientName, value);
                AcceptCommand.RaiseCanExecuteChanged();
            }
        }

        public string OfficeName
        {
            get { return _offficeName; }
            set { SetProperty(ref _offficeName, value); }
        }

        public bool CanSubmit
        {
            get { return _canSubmit; }
            set { SetProperty(ref _canSubmit, value); }
        }

        public string IsDistinctWarningVisible
        {
            get { return _isDistinctWarningVisible; }
            set{ SetProperty(ref _isDistinctWarningVisible, value); }
        }

        public INotification Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, (ICreateNewClientNotification)value); }
        }

        public DelegateCommand AcceptCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public Action FinishInteraction { get; set; }
        public NewClientDialogViewModel(IClientService clientService)
        {
            _cs = clientService;

            AcceptCommand = new DelegateCommand(Accept, CanAccept);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            _notification.Confirmed = false;
            FinishInteraction?.Invoke();
        }

        private void Accept()
        {
            if (ConfirmDistinct())
            {
                _notification.ClientName = ClientName;
                _notification.OfficeName = OfficeName;
                _notification.Confirmed = true;
                FinishInteraction?.Invoke();
            }
            else
                IsDistinctWarningVisible = "Visible";
        }

        private bool CanAccept()
        {
            if (!string.IsNullOrEmpty(ClientName))
                return true;
            else
                return false;
        }

        private bool ConfirmDistinct()
        {
            return _cs.ConfirmDistinct(ClientName, OfficeName);
        }
    }
}
