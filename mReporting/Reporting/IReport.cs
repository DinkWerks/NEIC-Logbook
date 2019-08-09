namespace mReporting.Reporting
{
    public interface IReport
    {
        string Name { get; set; }
        string Description { get; set; }
        ReportCategories Category { get; set; }
        ParameterTypes? Parameters { get; set; }
        void Execute(object[] parameters);
    }
}
