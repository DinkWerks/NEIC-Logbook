using Prism.Mvvm;
using Prism.Regions;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System.Windows;
using Tracker.Core.CompositeCommands;
using Prism.Events;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;

namespace Tracker.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "NEIC Logbook";
        private string _version = "Version 0.5.5.2";
        private IRegionManager _rm;
        private IApplicationCommands applicationCommands;
        private StatusPayload _status;
        private double _windowHeight;
        private double _windowWidth;

        public double WindowHeight
        {
            get { return _windowHeight; }
            set { SetProperty(ref _windowHeight, value); }
        }

        public double WindowWidth
        {
            get { return _windowWidth; }
            set { SetProperty(ref _windowWidth, value); }
        }

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

        public IApplicationCommands ApplicationCommands
        {
            get { return applicationCommands; }
            set { SetProperty(ref applicationCommands, value); }
        }

        public StatusPayload Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        //Commands
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand GoToGithubCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }

        //Constructor
        public MainWindowViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IRegionNavigationJournal journal, IApplicationCommands applicationCommands)
        {
            WindowWidth = SystemParameters.PrimaryScreenHeight;

            _rm = regionManager;
            
            ApplicationCommands = applicationCommands;

            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoToGithubCommand = new DelegateCommand(GoToGithub);
            ExitCommand = new DelegateCommand(Exit);

            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            eventAggregator.GetEvent<StatusEvent>().Subscribe(ChangeStatusText);
        }

        //Methods
        private void ChangeStatusText(StatusPayload statusPayload)
        {
            Status = null;
            Status = statusPayload;
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
                Content = "Are you sure you would like to exit? Unsaved work will be lost."
            },
                r =>
                {
                    if (r.Confirmed)
                        Application.Current.Shutdown();
                }
            );
        }
    }
}
