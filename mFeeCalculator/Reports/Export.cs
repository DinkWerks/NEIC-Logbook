using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tracker.Core.Models;
using Tracker.Core.Models.Fees;
using Word = Microsoft.Office.Interop.Word;

namespace mFeeCalculator.Reports
{
    class Export
    {
        private Word.Document document;
        private object missing;
        private Fee _fee;
        private RecordSearch _recordSearch;

        public Export(Fee fee)
        {
            _fee = fee;
            try
            {
                Word.Application application = new Word.Application
                {
                    ShowAnimation = false,
                    Visible = true
                };
                missing = System.Reflection.Missing.Value;
                document = application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                document.Paragraphs.SpaceAfter = 0;

                AddHeader();
                foreach (Word.Paragraph p in document.Paragraphs)
                {
                    p.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void AddHeader()
        {
            Word.Paragraph headerPart1 = document.Content.Paragraphs.Add(ref missing);
            headerPart1.Range.Font.Bold = 1;
            headerPart1.Range.Font.Size = 14;
            headerPart1.Range.Text = "Record Search Charge for I.C. File # D19-121\n";
            headerPart1.Range.InsertParagraphAfter();

            Word.Paragraph headerPart2 = document.Content.Paragraphs.Add(ref missing);
            string totalCost = "$" + _fee.TotalProjectCost.ToString();
            headerPart2.Range.Text = "The charge for this record search is " + totalCost +
                ". Please see the table below for an itemization.\n";
            object startRange = headerPart2.Range.End - (totalCost.Length + 50);
            object endRange = headerPart2.Range.End - 50;
            Word.Range toBold = document.Range(startRange, endRange);
            toBold.Bold = 1;

            headerPart2.Range.InsertParagraphAfter();
        }

        private void AddBody()
        {
            Word.Paragraph bodyParagraph = document.Content.Paragraphs.Add(ref missing);
            Word.Table table = document.Tables.Add(bodyParagraph.Range, 5, 3, ref missing, ref missing);
        }
    }
}
