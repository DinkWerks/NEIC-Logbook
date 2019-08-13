using Prism.Mvvm;
using Prism.Regions;
using Prism.Commands;
using Tracker.Views;
using System;
using Prism.Interactivity.InteractionRequest;
using System.Media;
using System.Windows;

namespace Tracker.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "NEIC Logbook";
        private string _version = "Version 0.5.0";
        private IRegionManager _rm;
        private IRegionNavigationJournal _journal;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }

        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand GoToGithubCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; set; }

        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }

        //Constructor
        public MainWindowViewModel(IRegionManager regionManager, RegionNavigationService regionNavigationService)
        {
            _rm = regionManager;
            _journal = regionNavigationService.Journal;

            regionManager.RegisterViewWithRegion("ContentRegion", typeof(HomeScreen));

            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoToGithubCommand = new DelegateCommand(GoToGithub);
            ExitCommand = new DelegateCommand(Exit);
            GoBackCommand = new DelegateCommand(GoBack);

            ConfirmationRequest = new InteractionRequest<IConfirmation>();
        }
        private void Navigate(string navigationTarget)
        {
            _rm.RequestNavigate("ContentRegion", navigationTarget);
        }

        private void GoToGithub()
        {
            System.Diagnostics.Process.Start("https://github.com/DinkWerks/NEIC-Logbook");
        }

        private void Exit()
        {
            ConfirmationRequest.Raise(new Confirmation
            {
                Title = "Exit",
                Content = "Are you sure you would like to exit? Unsaved work will be lost. Return the home screen to save."
            },
                r =>
                {
                    if (r.Confirmed)
                        Application.Current.Shutdown();
                }
            );
        }

        private void GoBack()
        {
            _journal.GoBack();
        }

    }
}
