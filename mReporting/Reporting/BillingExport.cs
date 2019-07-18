using Tracker.Core.Services;

namespace mReporting.Reporting
{
    public class BillingExport : IReport
    {
        private IRecordSearchService _rss;

        public BillingExport(IRecordSearchService recordSearchService)
        {
            _rss = recordSearchService;
            string readthis = "https://www.c-sharpcorner.com/UploadFile/muralidharan.d/how-to-create-word-document-using-C-Sharp/";
        }
    }
}
