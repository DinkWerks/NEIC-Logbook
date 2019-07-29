using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using Tracker.Core.Models;
using Tracker.Core.Services;
using System.Reflection;
using System.Windows;

namespace mReporting.Reporting
{
    public class CheckingExport : IReport
    {
        private IRecordSearchService _rss;
        private object missing;
        private Word.Document document;

        public string Name { get; set; }
        public string Description { get; set; }
        public ParameterTypes Parameters { get; set; }

        //Constructor
        public CheckingExport(IRecordSearchService recordSearchService)
        {
            _rss = recordSearchService;
            Name = "Checks to Deposit Export";
            Description = "This exported view will list any check recieved as payments within a specific date range.";
            Parameters = ParameterTypes.Date_Range;
        }

        //Methods
        public void Execute(object[] parameters)
        {
            List<RecordSearch> recordSearches = _rss.GetRecordSearchesByCriteria("WHERE ID > 0 AND CheckName IS NOT NULL");
            try
            {
                Word.Application wordApp = new Word.Application
                {
                    ShowAnimation = false,
                    Visible = true
                };
                missing = Missing.Value;
                document = wordApp.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                document.Paragraphs.SpaceAfter = 0;

                AddHeader();
                foreach (RecordSearch rs in recordSearches)
                {
                    AddEntry();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void AddHeader()
        {
            throw new NotImplementedException();
        }

        private void AddEntry()
        {
            throw new NotImplementedException();
        }
    }
}
