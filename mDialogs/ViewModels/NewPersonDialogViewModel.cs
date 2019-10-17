using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mDialogs.ViewModels
{
    public class NewPersonDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IDialogService _ds;
        private string _firstName;
        private string _lastName;

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        public string Title => "New Person";
        public DelegateCommand<string> CloseCommand { get; set; }
        public event Action<IDialogResult> RequestClose;

        //Constructor
        public NewPersonDialogViewModel(IDialogService dialogService)
        {
            _ds = dialogService;
            CloseCommand = new DelegateCommand<string>(CloseDialog);
        }

        //Methods
        protected virtual void CloseDialog(string source)
        {
            ButtonResult result = ButtonResult.None;
            bool exit = false;

            if (source == "OK")
            {
                if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(FirstName))
                {
                    if (ConfirmDistinct())
                    {
                        result = ButtonResult.OK;
                        exit = true;
                    }
                    else
                    {
                        if (ConfirmDuplicate())
                        {
                            result = ButtonResult.OK;
                            exit = true;
                        }
                        else
                            exit = false;
                    }
                }
            }
            else
            {
                result = ButtonResult.Cancel;
                exit = true;
            }

            if (exit)
            {
                var returnValue = new DialogParameters();
                returnValue.Add("fname", FirstName);
                returnValue.Add("lname", LastName);
                RaiseRequestClose(new DialogResult(result, returnValue));
            }
                
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        private bool ConfirmDistinct()
        {
            using (var context = new EFService())
            {
                Person match = context.People.Where(p => p.FirstName == FirstName && p.LastName == LastName).FirstOrDefault();
                if (match == null)
                    return true;
            }

            return false;
        }

        private bool ConfirmDuplicate()
        {
            bool confirmDupe = false;
            _ds.Show("ConfirmationDialog",
                new DialogParameters("message=There is already a person of that name recorded.\n\n " +
                    "Are you sure you would like to create a new entry with that name?"),
                    r =>
                    {
                        if (r.Result == ButtonResult.OK)
                            confirmDupe = true;
                    });
            return confirmDupe;
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
