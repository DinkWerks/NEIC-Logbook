using System.Collections.ObjectModel;

namespace mReporting.Reporting
{
    public interface IReport
    {
        string Name { get; set; }
        string Description { get; set; }
        ReportCategories Category { get; set; }
        ParameterTypes? Parameters { get; }
        ObservableCollection<object> ParameterPayload { get; set; }
        int ParameterCount { get; }
        void Execute(ObservableCollection<object> parameters);
        bool VerifyParameters();
    }
}
