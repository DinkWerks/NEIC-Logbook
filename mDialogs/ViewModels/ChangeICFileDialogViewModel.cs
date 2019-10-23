using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core.Models;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;

namespace mDialogs.ViewModels
{
    public class ChangeICFileDialogViewModel : BindableBase, IDialogAware
    {
        private Prefix _prefix;
        private string _year;
        private int _enumeration;
        private string _suffix;
        private string _isDistinctWarningVisible = "Hidden";
        private bool _canExit;

        public List<Prefix> PrefixChoices { get; private set; }

        public Prefix Prefix
        {
            get { return _prefix; }
            set
            {
                SetProperty(ref _prefix, value);
                GetNextEnumeration();
            }
        }

        public string Year
        {
            get { return _year; }
            set
            {
                SetProperty(ref _year, value);
                GetNextEnumeration();
            }
        }

        public int Enumeration
        {
            get { return _enumeration; }
            set { SetProperty(ref _enumeration, value); }
        }

        public string Suffix
        {
            get { return _suffix; }
            set { SetProperty(ref _suffix, value); }
        }

        public string IsDistinctWarningVisible
        {
            get { return _isDistinctWarningVisible; }
            set
            {
                SetProperty(ref _isDistinctWarningVisible, value);
            }
        }

        public string Title => "Change the IC File Number";
        public DelegateCommand<string> CloseCommand { get; set; }
        public event Action<IDialogResult> RequestClose;

        //Constructor
        public ChangeICFileDialogViewModel()
        {
            PrefixChoices = ProjectPrefixes.Values.ToList();

            CloseCommand = new DelegateCommand<string>(CloseDialog);
        }

        //Methods
        protected virtual void CloseDialog(string source)
        {
            ButtonResult result = ButtonResult.None;

            if (source == "OK")
            {
                if (ValidateID())
                {
                    result = ButtonResult.OK;
                    _canExit = true;
                }
                else
                {
                    _canExit = false;
                    IsDistinctWarningVisible = "Visible";
                    GetNextEnumeration();
                }
            }
            else
            {
                _canExit = true;
                result = ButtonResult.Cancel;
            }

            var returnValue = new DialogParameters();
            if (_canExit)
            {
                returnValue.Add("prefix", Prefix.ToString());
                returnValue.Add("year", Year);
                returnValue.Add("enumeration", Enumeration);
                returnValue.Add("suffix", Suffix);
                RaiseRequestClose(new DialogResult(result, returnValue));
            }
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        private void GetNextEnumeration()
        {
            if (!string.IsNullOrEmpty(Year) && Prefix != null)
            {
                using (var context = new EFService())
                {
                    Enumeration = context.Projects
                        .Where(p => p.ICTypePrefix == Prefix.ToString() && p.ICYear == Year)
                        .Select(p => p.ICEnumeration)
                        .DefaultIfEmpty(0)
                        .Max() + 1;
                }
            }
        }

        private bool ValidateID()
        {
            if (Prefix == null)
                return false;
            if (string.IsNullOrWhiteSpace(Year))
                return false;
            if (Enumeration <= 0)
                return false;
            if (string.IsNullOrWhiteSpace(Year))
                return false;
            if (ConfirmDistinct())
                return true;
            else
                return false;
        }

        private bool ConfirmDistinct()
        {
            using (var context = new EFService())
            {
                Project match = context.Projects
                    .Where(p => p.ICTypePrefix == Prefix.ToString()
                             && p.ICYear == Year
                             && p.ICEnumeration == Enumeration
                             && p.ICSuffix == Suffix)
                    .FirstOrDefault();
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
            Prefix = null;
            Year = "";
            Enumeration = 0;
            Suffix = "";
            IsDistinctWarningVisible = "Hidden";
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
