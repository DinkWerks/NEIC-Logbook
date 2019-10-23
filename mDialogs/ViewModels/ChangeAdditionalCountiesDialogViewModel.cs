using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tracker.Core.StaticTypes;

namespace mDialogs.ViewModels
{
    public class ChangeAdditionalCountiesDialogViewModel : BindableBase, IDialogAware
    {
        private IEnumerable<County> _selectableCounties = new List<County>();
        private ObservableCollection<County> _selectedCounties = new ObservableCollection<County>();

        public IEnumerable<County> SelectableCounties
        {
            get { return _selectableCounties; }
            set
            {
                SetProperty(ref _selectableCounties, value);
            }
        }

        public ObservableCollection<County> SelectedCounties
        {
            get { return _selectedCounties; }
            set { SetProperty(ref _selectedCounties, value); }
        }

        public string Title => "Add or Remove Additional Counties";
        public DelegateCommand<string> CloseCommand { get; set; }
        public event Action<IDialogResult> RequestClose;

        public ChangeAdditionalCountiesDialogViewModel()
        {
            SelectableCounties = from c in Counties.Values
                                 where c.ICCurator is "NEIC"
                                 select c;

            CloseCommand = new DelegateCommand<string>(CloseDialog);
        }

        //Methods
        protected virtual void CloseDialog(string source)
        {
            ButtonResult result = ButtonResult.None;
            DialogParameters returnCollection = new DialogParameters();

            if (source == "OK")
            {
                result = ButtonResult.OK;
                ObservableCollection<County> selected = new ObservableCollection<County>(
                    SelectableCounties.Where(c => c.IsChecked is true)
                    .ToList()
                    );
                returnCollection.Add("counties", selected); 
            }
            else
            {
                result = ButtonResult.Cancel;
            }

            RaiseRequestClose(new DialogResult(result,returnCollection));
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
            ObservableCollection<County> selectedCounties = parameters.GetValue<ObservableCollection<County>>("selected");

            foreach(County c in SelectableCounties)
            {
                if (selectedCounties != null && selectedCounties.Contains(c))
                    c.IsChecked = true;
                else
                    c.IsChecked = false;
            }
        }
    }
}
