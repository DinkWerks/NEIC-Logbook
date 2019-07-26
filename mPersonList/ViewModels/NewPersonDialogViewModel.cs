using System;
using mPersonList.Notifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Tracker.Core.Services;

namespace mPersonList.ViewModels
{
    public class NewPersonDialogViewModel : BindableBase, IInteractionRequestAware
    {
        INewPersonNotification _notification;
        private string _firstName;
        private string _lastName;
        private IPersonService _ps;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                SetProperty(ref _firstName, value);
                AcceptCommand.RaiseCanExecuteChanged();
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                SetProperty(ref _lastName, value);
                AcceptCommand.RaiseCanExecuteChanged();
            }
        }

        public INotification Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, (INewPersonNotification)value); }
        }

        public DelegateCommand AcceptCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public Action FinishInteraction { get; set; }
        public InteractionRequest<IConfirmation> ConfirmDuplicateRequest { get; set; }

        public NewPersonDialogViewModel(IPersonService personService)
        {
            _ps = personService;

            AcceptCommand = new DelegateCommand(Accept, CanAccept);
            CancelCommand = new DelegateCommand(Cancel);
            ConfirmDuplicateRequest = new InteractionRequest<IConfirmation>();
        }

        private void Accept()
        {
            if (_ps.ConfirmDistinct(FirstName, LastName))
            {
                _notification.FirstName = FirstName;
                _notification.LastName = LastName;
                _notification.Confirmed = true;
                FinishInteraction?.Invoke();
            }
            else
            {
                if (ConfirmDuplicate())
                {
                    _notification.FirstName = FirstName;
                    _notification.LastName = LastName;
                    _notification.Confirmed = true;
                    FinishInteraction?.Invoke();
                }
                else
                {
                    FirstName = "";
                    LastName = "";
                }
            }
        }

        private bool CanAccept()
        {
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                return true;
            else
                return false;
        }

        private void Cancel()
        {
            _notification.Confirmed = false;
            FinishInteraction?.Invoke();
        }

        private bool ConfirmDuplicate()
        {
            bool returnValue = false;
            ConfirmDuplicateRequest.Raise(new Confirmation
            {
                Title = "Confirmation",
                Content = "A person with that name has already been entered.\n" +
                "Are you sure that you would like to add another person by that name?"
            },
                r =>
                 {
                     if (r.Confirmed)
                         returnValue = true;
                 }
            );

            return returnValue;
        }
    }
}
