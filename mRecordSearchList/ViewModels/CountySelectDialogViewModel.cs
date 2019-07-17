using mRecordSearchList.Views;
using Prism.Mvvm;
using Prism.Regions;

namespace mRecordSearchList.ViewModels
{
    public class CountySelectDialogViewModel : BindableBase
    {
        public CountySelectDialogViewModel(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("CountySelectDialog", typeof(CountySelectDialogContents));
        }
    }
}
