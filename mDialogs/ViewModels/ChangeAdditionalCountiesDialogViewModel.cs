using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mDialogs.ViewModels
{
    public class ChangeAdditionalCountiesDialogViewModel : BindableBase, IDialogAware
    {

        public string Title => throw new NotImplementedException();

        public event Action<IDialogResult> RequestClose;

        public ChangeAdditionalCountiesDialogViewModel()
        {

        }

        public bool CanCloseDialog()
        {
            return false;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
