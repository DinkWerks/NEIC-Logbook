using System;
using System.Collections.Generic;
using System.Windows;
using Tracker.Core.Extensions;
using Tracker.Core.Models;
using Tracker.Core.Models.Fees;
using Tracker.Core.Services;
using Word = Microsoft.Office.Interop.Word;

namespace mReporting.Reporting
{
    public class BillingExport : IReport
    {
        private IRecordSearchService _rss;
        private object missing;
        private Word.Document document;

        public string Name { get; set; }
        public string Description { get; set; }
        public ReportCategories Category { get; set; }
        public ParameterTypes Parameters { get; set; }

        public BillingExport(IRecordSearchService recordSearchService)
        {
            _rss = recordSearchService;

            Name = "Research Foundation Billing Export";
            Description = "Exports a detailed list of every project to be billed by the Research Foundation. The list will open up in a word document that the user can edit, print, and save.";
            Parameters = ParameterTypes.Date_Range;
        }

        public void Execute(object[] parameters)
        {
            List<RecordSearch> recordSearches = _rss.GetRecordSearchesByCriteria("WHERE ID > 0");
            try
            {
                Word.Application wordApp = new Word.Application
                {
                    ShowAnimation = false,
                    Visible = true
                };
                missing = System.Reflection.Missing.Value;
                document = wordApp.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                document.Paragraphs.SpaceAfter = 0;
                AddHeader();
                foreach (RecordSearch record in recordSearches)
                {
                    AddEntry(record);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void AddHeader()
        {
            Word.Paragraph header = document.Content.Paragraphs.Add(ref missing);
            string money = "14,364.78";
            header.Range.Font.Bold = 1;
            header.Range.Font.Size = 14;
            header.Range.ParagraphFormat.SpaceAfter = 0;
            header.Range.Text = "Northeast Information Center - Billing Through " + DateTime.Now.Date.ToDateString() + Environment.NewLine +
                "Credit Account 808008900   $" + money;
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

        private void AddEntry(RecordSearch record)
        {
            //Date
            Word.Paragraph dateHeader = document.Content.Paragraphs.Add(ref missing);
            dateHeader.Range.Paragraphs.SpaceAfter = 0;
            dateHeader.Range.Text = record.DateOfResponse.ToDateString() + "\n";
            dateHeader.Range.InsertParagraphAfter();


            //Address, PEID, & Invoice #
            Word.Table iTable = document.Tables.Add(dateHeader.Range, 1, 3, ref missing, ref missing);
            iTable.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            iTable.Columns[1].PreferredWidth = 40;
            iTable.Columns[2].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            iTable.Columns[2].PreferredWidth = 30;

            if (string.IsNullOrWhiteSpace(record.BillingAddress.AddressLine2))
                iTable.Rows[1].Cells[1].Range.Text = String.Format("{0}\r\n{1}\r\n{2}, {3} {4}",
                   record.BillingAddress.AddressName, record.BillingAddress.AddressLine1, record.BillingAddress.City, record.BillingAddress.State, record.BillingAddress.ZIP);
            else
                iTable.Rows[1].Cells[1].Range.Text = String.Format("{0}\r\n{1}\r\n{2}\r\n{3}, {4} {5}",
                   record.BillingAddress.AddressName, record.BillingAddress.AddressLine1, record.BillingAddress.AddressLine2, record.BillingAddress.City, record.BillingAddress.State, record.BillingAddress.ZIP);

            iTable.Rows[1].Cells[2].Range.Text = string.Format("PEID # " + record.ClientModel.OldPEID.PadLeft(6, '0'));
            iTable.Rows[1].Cells[2].Range.Bold = 1;
            iTable.Rows[1].Cells[2].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;

            iTable.Rows[1].Cells[3].Range.Text = "Invoice #";
            iTable.Rows[1].Cells[3].Range.Bold = 1;
            iTable.Range.InsertParagraphAfter();


            //Person, Project Name, Requestor, IC File #
            Word.Paragraph projectInfo = document.Content.Paragraphs.Add(ref missing);

            string attentionTo;
            if (String.IsNullOrWhiteSpace(record.BillingAddress.AttentionTo))
                attentionTo = "";
            else
                attentionTo = "\r\nATTN: " + record.BillingAddress.AttentionTo;
            string fileNumber = "IC File # " + record.ICTypePrefix + record.ICYear + "-" + record.ICEnumeration + record.ICSuffix;

            if (String.IsNullOrWhiteSpace(record.Requestor.FirstName))
                projectInfo.Range.Text = String.Format("{0}\r\n>>\r\nRE: {1}; {2}\r\n>>",
                    attentionTo, record.ProjectName, fileNumber);
            else
                projectInfo.Range.Text = String.Format("{0}\r\n>>\r\nRE: {1} (Requested By: {2} {3}); {4}\r\n>>",
                    attentionTo, record.ProjectName, record.Requestor.FirstName, record.Requestor.LastName, fileNumber);

            object startRange = projectInfo.Range.End - (fileNumber.Length + 4);
            object endRange = projectInfo.Range.End - 3;
            Word.Range toBold = document.Range(ref startRange, ref endRange);
            toBold.Bold = 1;
            projectInfo.Range.InsertParagraphAfter();


            //Billing Info
            Word.Table bTable = document.Content.Tables.Add(projectInfo.Range, 1, 2, ref missing);
            bTable.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            bTable.Columns[1].PreferredWidth = 25;

            bTable.Rows[1].Cells[1].Range.Text = "Amount Due: $" + record.Fee.TotalProjectCost;

            string chargeInformation = "";
            foreach (ICharge charge in record.Fee.Charges)
            {
                if (charge.TotalCost <= 0)
                    continue;
                switch (charge.Type)
                {
                    case "variable":
                        VariableCharge vCharge = (VariableCharge)charge;
                        chargeInformation += string.Format("  {0} - {1} {2} @ ${3} per {4}\n", 
                            vCharge.Name, vCharge.Count, vCharge.UnitNamePlural, vCharge.Cost, vCharge.UnitName);
                        break;
                    case "boolean":
                        BooleanCharge bCharge = (BooleanCharge)charge;
                        chargeInformation += string.Format("  {0} - ${1}\n", bCharge.Name, bCharge.TotalCost);
                        break;
                    case "categorical":
                        CategoricalCharge cCharge = (CategoricalCharge)charge;
                        chargeInformation += string.Format("  {0} - {1} {2} - ${3}\n", cCharge.Name, cCharge.Count, cCharge.UnitNamePlural, cCharge.TotalCost);
                        break;
                    default:
                        break;
                }
            }
            bTable.Rows[1].Cells[2].Range.Text = "Information\n" + chargeInformation + "Please include the invoice number on your remittance";
            bTable.Range.InsertParagraphAfter();

            //Finish with line
            InsertLine();
        }
    }
}
