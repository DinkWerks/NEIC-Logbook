using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mOrganizationList.ViewModels
{
    public class NewOrganizationDialogViewModel : BindableBase, IDialogAware
    {
        private string _organizationName;
        private string _isDistinctWarningVisible = "Hidden";
        private bool _canExit;
        private IEFService _ef;

        public string Title => "New Entry";

        public string OrganizationName
        {
            get { return _organizationName; }
            set { SetProperty(ref _organizationName, value); }
        }

        public string IsDistinctWarningVisible
        {
            get { return _isDistinctWarningVisible; }
            set { SetProperty(ref _isDistinctWarningVisible, value); }
        }

        public DelegateCommand<string> CloseCommand { get; set; }
        public event Action<IDialogResult> RequestClose;

        //Constructor
        public NewOrganizationDialogViewModel(IEFService efService)
        {
            _ef = efService;

            CloseCommand = new DelegateCommand<string>(CloseDialog);
        }

        //Methods
        protected virtual void CloseDialog(string source)
        {
            ButtonResult result = ButtonResult.None;

            if (source == "OK")
            {
                if (!string.IsNullOrWhiteSpace(OrganizationName) && ConfirmDistinct())
                {
                    result = ButtonResult.OK;
                    _canExit = true;
                }
                else
                {
                    _canExit = false;
                    IsDistinctWarningVisible = "Visible";
                }
            }
            else
            {
                _canExit = true;
                result = ButtonResult.Cancel;
                OrganizationName = "";
            }

            if (_canExit)
                RaiseRequestClose(new DialogResult(result,
                    new DialogParameters($"OrgName={OrganizationName}"))
                    );
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        private bool ConfirmDistinct()
        {
            using (var context = new EFService())
            {
                Organization match = context.Organizations.Where(o => o.OrganizationName == OrganizationName).FirstOrDefault();
                if (match == null)
                    return true;
            }
            return false;
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
