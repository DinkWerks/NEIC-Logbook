using mFeeCalculator.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace mFeeCalculator
{
    public class mFeeCalculatorModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Calculator>();
        }
    }
}