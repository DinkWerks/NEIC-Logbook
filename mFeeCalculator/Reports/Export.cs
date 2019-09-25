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
                AddTable();
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

        private void AddTable()
        {
            Word.Paragraph bodyParagraph = document.Content.Paragraphs.Add(ref missing);
            Word.Table table = document.Tables.Add(bodyParagraph.Range, 3, 3, ref missing, ref missing);

            //Header Line 1
            #region Header Line 1
            table.Rows[1].Cells[1].Merge(table.Rows[1].Cells[2]);
            table.Rows[1].Cells[1].Merge(table.Rows[1].Cells[2]);
            table.Rows[1].Cells[1].Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Rows[1].Cells[1].Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Rows[1].Cells[1].Borders[Word.WdBorderType.wdBorderBottom].LineWidth = Word.WdLineWidth.wdLineWidth225pt;
            table.Rows[1].Cells[1].Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Rows[1].Cells[1].Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

            table.Rows[1].Cells[1].Range.Text = "This is NOT an invoice";
            table.Rows[1].Cells[1].Range.Font.Bold = 1;
            table.Rows[1].Cells[1].Range.Font.Size = 14;
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            #endregion

            //Header Line 
            #region Header Line 2
            string[] columnHeaders = new string[] { "Factor", "Charge", "Your Change"};
            for (int i = 1; i < 4; i++)
            {
                table.Rows[2].Cells[i].Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                table.Rows[2].Cells[i].Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                table.Rows[2].Cells[i].Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                table.Rows[2].Cells[i].Borders[Word.WdBorderType.wdBorderBottom].LineWidth = Word.WdLineWidth.wdLineWidth225pt;

                table.Rows[2].Cells[i].Range.Text = columnHeaders[i-1];
                table.Rows[2].Cells[i].Range.Font.Bold = 1;
            }
            #endregion

            //Generated in reverse order, because row insertions occur above row referenced in Add function.
            //TotalCharge
            table.Rows[3].Cells[1].Range.Text = "Total Charge";
            table.Rows[3].Cells[1].Range.Font.Bold = 1;
            table.Rows[3].Cells[1].Range.Font.Size = 14;
            table.Rows[3].Cells[3].Range.Text = _fee.TotalProjectCost.ToString();
            table.Rows[3].Cells[3].Range.Font.Bold = 1;
            table.Rows[3].Cells[3].Range.Font.Size = 14;

            //Charge Expediency Modifiers
            if (_fee.IsEmergency)
            {
                Word.Row emergencyRow = table.Rows.Add(table.Rows[3]);
                emergencyRow.Cells[1].Range.Text = "Emergency Rate";
                emergencyRow.Cells[1].Range.Font.Bold = 1;
                emergencyRow.Cells[2].Range.Text = "100% Surcharge";
                emergencyRow.Cells[3].Range.Text = _fee.Subtotal.ToString();
                emergencyRow.Cells[3].Range.Font.Bold = 1;
            }
            if (_fee.IsPriority)
            {
                Word.Row priorityRow = table.Rows.Add(table.Rows[3]);
                priorityRow.Cells[1].Range.Text = "Priority Rate";
                priorityRow.Cells[1].Range.Font.Bold = 1;
                priorityRow.Cells[2].Range.Text = "50% Surcharge";
                priorityRow.Cells[3].Range.Text = (_fee.TotalProjectCost - _fee.Subtotal).ToString();
                priorityRow.Cells[3].Range.Font.Bold = 1;
            }
            if (_fee.IsRapidResponse)
            {
                Word.Row rapidRow = table.Rows.Add(table.Rows[3]);
                rapidRow.Cells[1].Range.Text = "Rapid Rate";
                rapidRow.Cells[1].Range.Font.Bold = 1;
                rapidRow.Cells[2].Range.Text = "50% Surcharge on Staff Hours";
                rapidRow.Cells[3].Range.Text = (_fee.TotalProjectCost - _fee.Subtotal).ToString();
                rapidRow.Cells[3].Range.Font.Bold = 1;
            }

            //Subtotal
            Word.Row subtotalRow = table.Rows.Add(table.Rows[3]);
            subtotalRow.Cells[1].Range.Text = "Subtotal";
            subtotalRow.Cells[1].Range.Font.Bold = 1;
            subtotalRow.Cells[1].Range.Font.Size = 12;
            subtotalRow.Cells[3].Range.Text = _fee.TotalProjectCost.ToString();
            subtotalRow.Cells[3].Range.Bold = 1;

            //Charges (Loop)
        }
    }
}
