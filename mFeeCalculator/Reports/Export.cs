using System;
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
        private readonly Fee _fee;

        public Export(Fee fee, string icFileNum)
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

                AddHeader(icFileNum);
                AddTable();
                Word.Paragraph footer = document.Content.Paragraphs.Add(ref missing);
                footer.Range.Text = "\nAn invoice will follow from the Chico State Enterprises for billing purposes.";
                footer.Range.Font.Size = 11;
                footer.Range.Font.Bold = 0;
                footer.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void AddHeader(string icFileNum)
        {
            Word.Paragraph headerPart1 = document.Content.Paragraphs.Add(ref missing);
            headerPart1.Range.Font.Bold = 1;
            headerPart1.Range.Font.Size = 14;
            headerPart1.Range.Text = "Record Search Charge for I.C. File # " + icFileNum; //TODO This should read from the current record search
            headerPart1.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            headerPart1.Range.InsertParagraphAfter();

            Word.Paragraph headerPart2 = document.Content.Paragraphs.Add(ref missing);
            string totalCost = "$" + _fee.TotalProjectCost.ToString();
            headerPart2.Range.Text = "The charge for this record search is " + totalCost +
                ". Please see the table below for an itemization.";
            object startRange = headerPart2.Range.End - (totalCost.Length + 50);
            object endRange = headerPart2.Range.End - 50;
            Word.Range toBold = document.Range(startRange, endRange);
            toBold.Bold = 1;
            headerPart2.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            headerPart2.Range.InsertParagraphAfter();

            Word.Paragraph headerSpace = document.Content.Paragraphs.Add(ref missing);
            headerSpace.Range.Text = "\n";
            headerSpace.Range.InsertParagraphAfter();
        }

        private void AddTable()
        {
            Word.Paragraph bodyParagraph = document.Content.Paragraphs.Add(ref missing);
            Word.Table table = document.Tables.Add(bodyParagraph.Range, 3, 3, ref missing, ref missing);

            table.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            table.Columns[1].PreferredWidth = 30;

            table.Columns[2].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            table.Columns[2].PreferredWidth = 40;

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
            table.Rows[1].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            #endregion

            //Header Line 2
            #region Header Line 2
            string[] columnHeaders = new string[] { "Factor", "Charge", "Your Charge"};
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

            //Totals and Surcharges
            #region Totals & Surcharges
            //Generated in reverse order, because row insertions occur above row referenced in the Add function.
            //TotalCharge
            table.Rows[3].Cells[1].Range.Text = "Total Charge";
            table.Rows[3].Cells[1].Range.Font.Bold = 1;
            table.Rows[3].Cells[1].Range.Font.Size = 14;
            table.Rows[3].Cells[3].Range.Text = _fee.TotalProjectCost.ToString("C");
            table.Rows[3].Cells[3].Range.Font.Bold = 1;
            table.Rows[3].Cells[3].Range.Font.Size = 14;
            FormatCells(table.Rows[3]);

            //Charge Expediency Modifiers
            if (_fee.IsEmergency)
            {
                Word.Row emergencyRow = table.Rows.Add(table.Rows[3]);
                emergencyRow.Cells[1].Range.Text = "Emergency Rate";
                emergencyRow.Cells[1].Range.Font.Bold = 1;
                emergencyRow.Cells[1].Range.Font.Bold = 11;
                emergencyRow.Cells[2].Range.Text = "100% Surcharge";
                emergencyRow.Cells[3].Range.Text = _fee.Subtotal.ToString("C");
                emergencyRow.Cells[3].Range.Font.Bold = 1;
                emergencyRow.Cells[3].Range.Font.Bold = 11;
                FormatCells(emergencyRow);
            }
            if (_fee.IsPriority)
            {
                Word.Row priorityRow = table.Rows.Add(table.Rows[3]);
                priorityRow.Cells[1].Range.Text = "Priority Rate";
                priorityRow.Cells[1].Range.Font.Bold = 1;
                priorityRow.Cells[1].Range.Font.Size = 11;
                priorityRow.Cells[2].Range.Text = "50% Surcharge";
                priorityRow.Cells[3].Range.Text = (_fee.TotalProjectCost - _fee.Subtotal).ToString("C");
                priorityRow.Cells[3].Range.Font.Bold = 1;
                priorityRow.Cells[3].Range.Font.Size = 11;
                FormatCells(priorityRow);
            }
            if (_fee.IsRapidResponse)
            {
                Word.Row rapidRow = table.Rows.Add(table.Rows[3]);
                rapidRow.Cells[1].Range.Text = "Rapid Rate";
                rapidRow.Cells[1].Range.Font.Bold = 1;
                rapidRow.Cells[1].Range.Font.Size = 11;
                rapidRow.Cells[2].Range.Text = "50% Surcharge on Staff Hours";
                rapidRow.Cells[3].Range.Text = (_fee.TotalProjectCost - _fee.Subtotal).ToString("C");
                rapidRow.Cells[3].Range.Font.Bold = 1;
                rapidRow.Cells[3].Range.Font.Size = 11;
                FormatCells(rapidRow);
            }
            
            //Subtotal
            Word.Row subtotalRow = table.Rows.Add(table.Rows[3]);
            subtotalRow.Cells[1].Range.Text = "Subtotal";
            subtotalRow.Cells[1].Range.Font.Bold = 1;
            subtotalRow.Cells[1].Range.Font.Size = 11;
            subtotalRow.Cells[3].Range.Text = _fee.Subtotal.ToString("C");
            subtotalRow.Cells[3].Range.Bold = 1;
            subtotalRow.Cells[3].Range.Font.Size = 11;
            FormatCells(subtotalRow);
            #endregion

            //Charges (Loop Reversed because of row insertion behavior.)
            for (int i = _fee.Charges.Count - 1; i >= 0; i--)
            {
                ICharge charge = _fee.Charges[i];
                if (charge.Type == "boolean" && charge.TotalCost > 0)
                {
                    BooleanCharge bCharge = (BooleanCharge)charge;
                    Word.Row chargeRow = table.Rows.Add(table.Rows[3]);

                    chargeRow.Cells[1].Range.Text = bCharge.Name;
                    chargeRow.Cells[1].Range.Font.Bold = 0;
                    chargeRow.Cells[1].Range.Font.Size = 10;
                    chargeRow.Cells[3].Range.Text = bCharge.TotalCost.ToString("C");
                    chargeRow.Cells[3].Range.Font.Bold = 0;
                    chargeRow.Cells[3].Range.Font.Size = 10;

                    FormatCells(chargeRow);
                }
                else if (charge.Type == "variable" && charge.TotalCost > 0)
                {
                    VariableCharge vCharge = (VariableCharge)charge;
                    Word.Row chargeRow = table.Rows.Add(table.Rows[3]);

                    chargeRow.Cells[1].Range.Text = vCharge.Name;
                    chargeRow.Cells[1].Range.Font.Bold = 0;
                    chargeRow.Cells[1].Range.Font.Size = 10;
                    chargeRow.Cells[2].Range.Text = vCharge.Cost.ToString("C") + "/" + vCharge.UnitName;
                    chargeRow.Cells[2].Range.Font.Bold = 0;
                    chargeRow.Cells[2].Range.Font.Size = 10;
                    chargeRow.Cells[3].Range.Text = $"{vCharge.TotalCost.ToString("C")} ({vCharge.Count} {vCharge.UnitNamePlural})";
                    chargeRow.Cells[3].Range.Font.Bold = 0;
                    chargeRow.Cells[3].Range.Font.Size = 10;

                    FormatCells(chargeRow);
                }
                else if (charge.Type == "categorical" && charge.TotalCost > 0)
                {
                    CategoricalCharge cCharge = (CategoricalCharge)charge;
                    Word.Row chargeRow = table.Rows.Add(table.Rows[3]);

                    chargeRow.Cells[1].Range.Text = cCharge.Name;
                    chargeRow.Cells[1].Range.Font.Bold = 0;
                    chargeRow.Cells[1].Range.Font.Size = 10;
                    chargeRow.Cells[3].Range.Text = $"{cCharge.TotalCost.ToString("C")} ({cCharge.Count} {cCharge.UnitNamePlural})";
                    chargeRow.Cells[3].Range.Font.Bold = 0;
                    chargeRow.Cells[3].Range.Font.Size = 10;

                    FormatCells(chargeRow);
                }
            }
        }
        
        private void FormatCells(Word.Row row)
        {
            for (int i = 1; i < 4; i++)
            {
                row.Cells[i].Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                row.Cells[i].Borders[Word.WdBorderType.wdBorderBottom].LineWidth = Word.WdLineWidth.wdLineWidth100pt;
                row.Cells[i].Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                row.Cells[i].Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
            }

            row.Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            row.Cells[3].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        }
    }
}
