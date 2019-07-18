using System;
using System.Windows;
using Tracker.Core.Extensions;
using Tracker.Core.Services;
using Word = Microsoft.Office.Interop.Word;

namespace mReporting.Reporting
{
    public class BillingExport : IReport
    {
        private IRecordSearchService _rss;
        private object missing;
        private readonly Word.Document document;

        public BillingExport(IRecordSearchService recordSearchService)
        {
            _rss = recordSearchService;
            //_rss.GetPartialRecordSearchesByCriteria();
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
                //foreach
                AddEntry();
                AddEntry();
                AddEntry();
                AddEntry();
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

        private void AddEntry()
        {
            //Date
            Word.Paragraph dateHeader = document.Content.Paragraphs.Add(ref missing);
            dateHeader.Range.Paragraphs.SpaceAfter = 0;
            dateHeader.Range.Text = DateTime.Now.ToDateString() + Environment.NewLine + Environment.NewLine;
            dateHeader.Range.InsertParagraphAfter();

            //Address, PEID, & Invoice #
            Word.Table iTable = document.Tables.Add(dateHeader.Range, 1, 3, ref missing, ref missing);
            iTable.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            iTable.Columns[1].PreferredWidth = 40;
            iTable.Columns[2].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            iTable.Columns[2].PreferredWidth = 30;

            iTable.Rows[1].Cells[1].Range.Text = "Northern California Resource Center\r\nP.O. Box 342\r\nFort Jones, CA 96032";

            iTable.Rows[1].Cells[2].Range.Text = "PEID # 005223";
            iTable.Rows[1].Cells[2].Range.Bold = 1;
            iTable.Rows[1].Cells[2].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;

            iTable.Rows[1].Cells[3].Range.Text = "Invoice #";
            iTable.Rows[1].Cells[3].Range.Bold = 1;
            iTable.Range.InsertParagraphAfter();

            //Person, Project Name, Requestor, IC File #
            Word.Paragraph projectInfo = document.Content.Paragraphs.Add(ref missing);
            string fileNumber = "IC File # D19-65";
            projectInfo.Range.Text = "\r\nATTN: Accounts Payable\r\n>>\r\nRE: City of Yreka, Hazardous Fuels Reduction; " + fileNumber + "\r\n>>";
            object startRange = projectInfo.Range.End - (fileNumber.Length + 4);
            object endRange = projectInfo.Range.End - 3;
            Word.Range toBold = document.Range(ref startRange, ref endRange);
            toBold.Bold = 1;
            projectInfo.Range.InsertParagraphAfter();

            //Billing Info
            Word.Table bTable = document.Content.Tables.Add(projectInfo.Range, 1, 2, ref missing);
            bTable.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            bTable.Columns[1].PreferredWidth = 25;

            bTable.Rows[1].Cells[1].Range.Text = "Amount Due: $919.65";

            bTable.Rows[1].Cells[2].Range.Text = "Information\n Information Center Time - 4 hours @ $150 per hour\n 49 Digitized Features\n" +
                " Photocopy Charge - 131 Copies @ $0.15 per copy\n Please include the invoice number on your remittance";
            bTable.Range.InsertParagraphAfter();
            //Finish with line
            InsertLine();
        }
    }
}
