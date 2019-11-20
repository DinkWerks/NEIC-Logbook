using System;
using System.Collections.Generic;
using Word = Microsoft.Office.Interop.Word;
using Tracker.Core.Models;
using Tracker.Core.Services;
using System.Reflection;
using System.Windows;
using System.Collections.ObjectModel;

namespace mReporting.Reporting
{
    public class CheckingExport : IReport
    {
        //private IRecordSearchService _rss;
        private IProjectService _ps;
        private object missing;
        private Word.Document document;
        private Word.Table table;
        private ObservableCollection<object> _parameterPayload = new ObservableCollection<object>();

        public string Name { get; set; }
        public string Description { get; set; }
        public ReportCategories Category { get; set; }
        public ParameterTypes? Parameters { get; } = ParameterTypes.Date_Range;
        public int ParameterCount { get; } = 2;
        public ObservableCollection<object> ParameterPayload
        {
            get { return _parameterPayload; }
            set { _parameterPayload = value; }
        }

        //Constructor
        public CheckingExport(IProjectService projectService)
        {
            _ps = projectService;
            Name = "Checks to Deposit Export";
            Description = "This exported view will list any check recieved as payments within a specific date range.";
        }

        //Methods
        public void Execute(ObservableCollection<object> parameters)
        {
            ParameterPayload = parameters;
            DateTime StartDate = (DateTime)ParameterPayload[0];
            DateTime EndDate = (DateTime)ParameterPayload[1];

            if (VerifyParameters())
            {
                List<Project> projects = _ps.GetProjectsDateRange(StartDate, EndDate, tracking: false);

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

                    AddTitle(StartDate, EndDate);

                    //Create table
                    Word.Paragraph tableSection = document.Content.Paragraphs.Add(ref missing);
                    table = document.Tables.Add(tableSection.Range, projects.Count + 1, 5, ref missing, ref missing);

                    table.AllowAutoFit = true;
                    table.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);

                    SetColumnWidth(1, 15);
                    SetColumnWidth(2, 15);
                    SetColumnWidth(3, 45);
                    SetColumnWidth(4, 15);
                    SetColumnWidth(5, 10);

                    //Create Header
                    table.Rows[1].Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Rows[1].Borders[Word.WdBorderType.wdBorderBottom].LineWidth = Word.WdLineWidth.wdLineWidth225pt;
                    AddHeaderText(1, "Date Paid");
                    AddHeaderText(2, "IC File #");
                    AddHeaderText(3, "Project Name");
                    AddHeaderText(4, "Amount");
                    AddHeaderText(5, "Check #");

                    //Start Index at 2 to skip header row.
                    int index = 2;
                    foreach (Project project in projects)
                    {
                        AddEntry(index, project);
                        index++;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        public bool VerifyParameters()
        {
            foreach (object param in ParameterPayload)
            {
                if (param == null)
                    return false;
            }
            if ((DateTime)ParameterPayload[0] < (DateTime)ParameterPayload[1])
                return true;
            else
                return false;
        }

        private void AddTitle(DateTime start, DateTime end)
        {
            Word.Paragraph header = document.Content.Paragraphs.Add(ref missing);
            header.Range.Font.Bold = 1;
            header.Range.Font.Size = 18;
            header.Range.Text = "Northeast Information Center Deposits"; 
            header.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            header.Range.InsertParagraphAfter();

            Word.Paragraph dateRange = document.Content.Paragraphs.Add(ref missing);
            dateRange.Range.Font.Bold = 1;
            dateRange.Range.Font.Size = 14;
            dateRange.Range.Text = start.ToShortDateString() + " - " + end.ToShortDateString();
            dateRange.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            header.Range.InsertParagraphAfter();
        }

        private void AddEntry(int index, Project project)
        {
            table.Rows[index].Cells[1].Range.Text = project.DateReceived.Value.ToShortDateString();
            table.Rows[index].Cells[2].Range.Text = project.GetFileNumberFormatted();
            table.Rows[index].Cells[3].Range.Text = project.ProjectName;
            table.Rows[index].Cells[4].Range.Text = project.TotalFee.ToString();
            table.Rows[index].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
            table.Rows[index].Cells[5].Range.Text = project.CheckNumber;
            table.Rows[index].Cells[5].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
            table.Rows[index].Cells[5].Range.ParagraphFormat.LineSpacing = 16;
        }

        private void SetColumnWidth(int column, int width)
        {
            table.Columns[column].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            table.Columns[column].PreferredWidth = width;
        }

        private void AddHeaderText(int cell, string text)
        {
            table.Rows[1].Cells[cell].Range.Text = text;
            table.Rows[1].Cells[cell].Range.Bold = 1;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
