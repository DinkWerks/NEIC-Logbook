using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace mDialogs.ViewModels
{
    public class ConfirmationDialogViewModel : BindableBase, IDialogAware
    {
        private string _message;
        
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public string Title => "Delete Entry";

        public DelegateCommand<string> CloseCommand { get; private set; }
        public event Action<IDialogResult> RequestClose;

        public ConfirmationDialogViewModel()
        {
            CloseCommand = new DelegateCommand<string>(CloseDialog); 
        }

        protected virtual void CloseDialog(string source)
        {
            ButtonResult result = ButtonResult.None;

            if (source == "OK")
                result = ButtonResult.OK;
            else if (source == "Cancel")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
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
            Message = parameters.GetValue<string>("message");
        }
    }
}
