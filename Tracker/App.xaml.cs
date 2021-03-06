﻿using Tracker.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using Tracker.Core.Services;
using mReporting.Reporting;
using Tracker.Core.CompositeCommands;

namespace Tracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<HomeScreen>();
            containerRegistry.RegisterForNavigation<SettingsScreen>();

            //Composite Commands
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();

            //Services
            containerRegistry.RegisterSingleton<IRecordSearchService, RecordSearchService>();
            containerRegistry.RegisterSingleton<IClientService, ClientService>();
            containerRegistry.RegisterSingleton<IPersonService, PersonService>();
            containerRegistry.RegisterSingleton<IAddressService, AddressService>();
            containerRegistry.RegisterSingleton<IFeeService, FeeService>();
            containerRegistry.RegisterSingleton<IStaffService, StaffService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<mClientList.mClientListModule>();
            moduleCatalog.AddModule<mPersonList.mPersonListModule>();
            moduleCatalog.AddModule<mRecordSearchList.mRecordSearchListModule>();
            moduleCatalog.AddModule<mFeeCalculator.mFeeCalculatorModule>();
            moduleCatalog.AddModule<mReporting.mReportingModule>();
            moduleCatalog.AddModule<mStaffList.mStaffListModule>();
        }
    }
}
