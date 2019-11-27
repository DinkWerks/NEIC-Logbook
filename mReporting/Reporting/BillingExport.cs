using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Tracker.Core.Extensions;
using Tracker.Core.Models;
using Tracker.Core.Models.Fees;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;
using Word = Microsoft.Office.Interop.Word;

namespace mReporting.Reporting
{
    public class BillingExport : IReport
    {
        private IProjectService _ps;
        private List<Project> _projects;

        private object missing;
        private Word.Document document;
        private ObservableCollection<object> _parameterPayload = new ObservableCollection<object>();
        private int _errorCount;

        //Properties
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
        public BillingExport(IProjectService projectService)
        {
            _ps = projectService;

            Name = "Research Foundation Billing Export";
            Description = "Exports a detailed list of every project to be billed by the Research Foundation. The list will open up in a word document that the user can edit, print, and save.";
        }

        //Methods
        public void Execute(ObservableCollection<object> parameters)
        {
            ParameterPayload = parameters;
            DateTime StartDate = (DateTime)ParameterPayload[0];
            DateTime EndDate = (DateTime)ParameterPayload[1];

            if (VerifyParameters())
            {
                _projects = _ps.GetProjectsDateRange(StartDate, EndDate, tracking: false);
                _projects = _projects.Where(r => r.Status == "Awaiting Billing").ToList();

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


                    foreach (ProjectNumber account in ProjectNumbers.ActiveProjectNumbers)
                    {
                        GenerateAccountReport(account.ProjectID);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }

            if (_errorCount > 0)
                MessageBox.Show(_errorCount + " error(s) found in last export");
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

        private void GenerateAccountReport(string account)
        {
            List<Project> projectSelection = _projects.Where(r => r.ProjectNumber != null && r.ProjectNumber.ProjectID == account).ToList();

            //Skip if no entries with that 
            if (projectSelection.Count <= 0)
                return;

            decimal total = 0;
            foreach (Project project in projectSelection)
                total += project.TotalFee;

            AddHeader(account, total);
            foreach (Project project in projectSelection)
            {
                project.GenerateFee();
                AddEntry(project);
            }
            document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
        }

        private void AddHeader(string account, decimal total)
        {
            Word.Paragraph header = document.Content.Paragraphs.Add(ref missing);
            header.Range.Font.Bold = 1;
            header.Range.Font.Size = 14;
            header.Range.ParagraphFormat.SpaceAfter = 0;
            header.Range.Text = "Northeast Information Center - Billing Through " + DateTime.Now.Date.ToDateString() + Environment.NewLine +
                "Credit Account " + account + "   $" + total;
            header.Range.InsertParagraphAfter();
            InsertLine();
        }

        private void InsertLine()
        {
            Word.InlineShape line = document.Paragraphs.Last.Range.InlineShapes.AddHorizontalLineStandard(ref missing);
            line.Height = 2;
            line.Fill.Solid();
            line.HorizontalLineFormat.NoShade = true;
            line.Fill.ForeColor.RGB = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            line.HorizontalLineFormat.PercentWidth = 100;
            line.HorizontalLineFormat.Alignment = Word.WdHorizontalLineAlignment.wdHorizontalLineAlignCenter;
        }

        private void AddEntry(Project project)
        {
            if (!project.ValidateCompleteness())
            {
                Word.Paragraph errorMessage = document.Content.Paragraphs.Add(ref missing);
                errorMessage.Range.Text = project.GetFileNumberFormatted() + " lacks a requestor or a client and was subsequently skipped. Please correct before exporting again.";
                _errorCount++;
                InsertLine();
                return;
            }
               
            //Date
            Word.Paragraph dateHeader = document.Content.Paragraphs.Add(ref missing);
            dateHeader.Range.Paragraphs.SpaceAfter = 0;
            try
            {
                dateHeader.Range.Text = project.DateOfResponse.ToDateString() + "\n";
            }
            catch
            {
                dateHeader.Range.Text = "Missing date of response.";
                dateHeader.Range.Font.Bold = 1;
                dateHeader.Range.Font.Color = Word.WdColor.wdColorDarkRed;
                _errorCount++;
            }
            dateHeader.Range.InsertParagraphAfter();


            //Address, PEID, & Invoice #
            Word.Table iTable = document.Tables.Add(dateHeader.Range, 1, 3, ref missing, ref missing);
            iTable.AllowAutoFit = true;
            iTable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);
            iTable.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            iTable.Columns[1].PreferredWidth = 40;
            iTable.Columns[2].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            iTable.Columns[2].PreferredWidth = 30;

            //--Address
            if (project.BillingAddress.ValidateMinimalCompleteness())
            {
                if (string.IsNullOrWhiteSpace(project.BillingAddress.AddressLine2))
                    iTable.Rows[1].Cells[1].Range.Text = string.Format("{0}\r\n{1}\r\n{2}, {3} {4}",
                       project.BillingAddress.AddressName, project.BillingAddress.AddressLine1, project.BillingAddress.City, project.BillingAddress.State, project.BillingAddress.ZIP);
                else
                    iTable.Rows[1].Cells[1].Range.Text = string.Format("{0}\r\n{1}\r\n{2}\r\n{3}, {4} {5}",
                       project.BillingAddress.AddressName, project.BillingAddress.AddressLine1, project.BillingAddress.AddressLine2, project.BillingAddress.City, project.BillingAddress.State, project.BillingAddress.ZIP);
            }
            else
            {
                iTable.Rows[1].Cells[1].Range.Text = "Missing billing address information.";
                iTable.Rows[1].Cells[1].Range.Font.Bold = 1;
                iTable.Rows[1].Cells[1].Range.Font.Color = Word.WdColor.wdColorDarkRed;
                _errorCount++;
            }

            //--PEID
            if (!string.IsNullOrWhiteSpace(project.Client.NewPEID))
                iTable.Rows[1].Cells[2].Range.Text = string.Format("PEID # " + project.Client.NewPEID);
            else if (!string.IsNullOrWhiteSpace(project.Client.OldPEID))
                iTable.Rows[1].Cells[2].Range.Text = string.Format("PEID # " + project.Client.OldPEID);
            else
            {
                iTable.Rows[1].Cells[2].Range.Text = "Missing PEID.";
                iTable.Rows[1].Cells[2].Range.Font.Bold = 1;
                iTable.Rows[1].Cells[2].Range.Font.Color = Word.WdColor.wdColorDarkRed;
                _errorCount++;
            }
            iTable.Rows[1].Cells[2].Range.Bold = 1;
            iTable.Rows[1].Cells[2].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;


            //--Invoice #
            iTable.Rows[1].Cells[3].Range.Text = "Invoice #";
            iTable.Rows[1].Cells[3].Range.Bold = 1;
            iTable.Range.InsertParagraphAfter();


            //Person, Project Name, Requestor, IC File #
            Word.Paragraph projectInfo = document.Content.Paragraphs.Add(ref missing);

            string attentionTo;
            if (string.IsNullOrWhiteSpace(project.BillingAddress.AttentionTo))
                attentionTo = "";
            else
                attentionTo = "\r\nATTN: " + project.BillingAddress.AttentionTo;
            string fileNumber = "IC File # " + project.GetFileNumberFormatted();

            if (string.IsNullOrWhiteSpace(project.Requestor.FirstName))
                projectInfo.Range.Text = string.Format("{0}\r\nRE: {1}; {2}\r\n",
                    attentionTo, project.ProjectName, fileNumber);
            else
                projectInfo.Range.Text = string.Format("{0}\r\nRE: {1} (Requested By: {2} {3}); {4}\r\n",
                    attentionTo, project.ProjectName, project.Requestor.FirstName, project.Requestor.LastName, fileNumber);

            object startRange = projectInfo.Range.End - (fileNumber.Length + 4);
            object endRange = projectInfo.Range.End - 2;
            Word.Range toBold = document.Range(ref startRange, ref endRange);
            toBold.Bold = 1;
            projectInfo.Range.InsertParagraphAfter();


            //Billing Info
            Word.Table bTable = document.Content.Tables.Add(projectInfo.Range, 1, 2, ref missing);
            bTable.AllowAutoFit = true;
            bTable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);
            bTable.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            bTable.Columns[1].PreferredWidth = 25;

            //--Total
            try
            {
                bTable.Rows[1].Cells[1].Range.Text = "Amount Due: " + project.Fee.TotalProjectCost.ToString("C");
            }
            catch
            {
                bTable.Rows[1].Cells[1].Range.Text = "Missing Total Project Cost.";
                bTable.Rows[1].Cells[1].Range.Font.Bold = 1;
                bTable.Rows[1].Cells[1].Range.Font.Color = Word.WdColor.wdColorDarkRed;
                _errorCount++;
            }

            //--Fees & Surcharge
            string chargeInformation = "";
            decimal runningTotal = 0;
            foreach (ICharge charge in project.Fee.Charges)
            {
                if (charge.TotalCost <= 0)
                    continue;
                switch (charge.Type)
                {
                    case "variable":
                        VariableCharge vCharge = (VariableCharge)charge;
                        if (project.Fee.FeeData.IsRapidResponse && charge.DBField == "StaffTime")
                        {
                            chargeInformation += string.Format("  {0} - {1} {2} @ ${3} per {4}\n",
                            vCharge.Name, vCharge.Count.ToString("G29"), vCharge.UnitNamePlural, vCharge.Cost * 1.5m, vCharge.UnitName);
                            runningTotal += vCharge.TotalCost;
                        }
                        else
                        {
                            chargeInformation += string.Format("  {0} - {1} {2} @ ${3} per {4}\n",
                            vCharge.Name, vCharge.Count.ToString("G29"), vCharge.UnitNamePlural, vCharge.Cost, vCharge.UnitName);
                            runningTotal += vCharge.TotalCost;
                        }
                        break;
                    case "boolean":
                        BooleanCharge bCharge = (BooleanCharge)charge;
                        chargeInformation += string.Format("  {0} - ${1}\n", bCharge.Name, bCharge.TotalCost);
                        runningTotal += bCharge.TotalCost;
                        break;
                    case "categorical":
                        CategoricalCharge cCharge = (CategoricalCharge)charge;
                        chargeInformation += string.Format("  {0} - {1} {2} - ${3}\n", cCharge.Name, cCharge.Count.ToString("G29"), cCharge.UnitNamePlural, cCharge.TotalCost);
                        runningTotal += cCharge.TotalCost;
                        break;
                    default:
                        break;
                }
            }

            string surcharge = "";
            if (project.FeeData.IsPriority)
                surcharge += "  Subtotal = " + project.Fee.Subtotal + "\n  --------- +\n  Priority Surcharge Fee: $" + (project.Fee.TotalProjectCost - runningTotal) + "\n";
            if (project.FeeData.IsEmergency)
                surcharge += "  Subtotal = " + project.Fee.Subtotal + "\n  --------- +\n  Emergency Surcharge Fee: $" + project.Fee.TotalProjectCost + "\n";

            bTable.Rows[1].Cells[2].Range.Text = "Information\n" + chargeInformation + surcharge + "Please include the invoice number on your remittance";
            bTable.Range.InsertParagraphAfter();

            //Finish with line
            InsertLine();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
