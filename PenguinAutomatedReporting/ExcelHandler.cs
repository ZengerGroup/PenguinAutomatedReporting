using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PenguinAutomatedReporting
{
    internal class ExcelHandler
    {
        public string SheetPath;
        XLWorkbook Workbook;
        IXLWorksheet Worksheet;
        public ExcelHandler()
        {
            SheetPath = Path.Combine(Path.GetTempPath(), String.Format("{0}_PenguinReporting.xlsx", DateTime.Now.ToString("MM-dd-yyyy")));
            Workbook = new XLWorkbook();
            Worksheet = Workbook.Worksheets.Add("Reporting");
            AddHeaders();
        }
        public void FillData(Row[] rows)
        {
            for(int i = 0; i < rows.Length; i++) AddData(rows[i], (i + 2));
            Worksheet.Columns().AdjustToContents();
            Workbook.SaveAs(SheetPath);
        }
        private void AddHeaders()
        {
            Worksheet.Cell("A1").Value = "Imprint";
            Worksheet.Cell("B1").Value = "ISBN";
            Worksheet.Cell("C1").Value = "Title";
            Worksheet.Cell("D1").Value = "Author";
            Worksheet.Cell("E1").Value = "Customer Print #";
            Worksheet.Cell("F1").Value = "Format";
            Worksheet.Cell("G1").Value = "Buyer";
            Worksheet.Cell("H1").Value = "Order Qty";
            Worksheet.Cell("I1").Value = "PO";
            Worksheet.Cell("J1").Value = "Price USA";
            Worksheet.Cell("K1").Value = "Price CAN";
            Worksheet.Cell("L1").Value = "Ship To";
            Worksheet.Cell("M1").Value = "Requested Delivery Date";
            Worksheet.Cell("N1").Value = "Revised Date";
            Worksheet.Cell("O1").Value = "Ship On Date";
            Worksheet.Cell("P1").Value = "Customer Reference #";
            Worksheet.Cell("Q1").Value = "Zenger Job #";
            Worksheet.Cell("R1").Value = "CSR";
            Worksheet.Cell("S1").Value = "Color";
            Worksheet.Cell("T1").Value = "Stock";
            Worksheet.Cell("U1").Value = "Status";
        }
        private void AddData(Row row, int rowIndex)
        {
            Worksheet.Cell(String.Format("A{0}", rowIndex)).Value = row.Imprint;
            Worksheet.Cell(String.Format("B{0}", rowIndex)).Value = row.ISBN;
            Worksheet.Cell(String.Format("C{0}", rowIndex)).Value = row.Title;
            Worksheet.Cell(String.Format("D{0}", rowIndex)).Value = row.Author;
            Worksheet.Cell(String.Format("E{0}", rowIndex)).Value = row.CustomerPrintNumber;
            Worksheet.Cell(String.Format("F{0}", rowIndex)).Value = row.TypeOfFormat;
            Worksheet.Cell(String.Format("G{0}", rowIndex)).Value = row.Buyer;
            Worksheet.Cell(String.Format("H{0}", rowIndex)).Value = row.OrderQty;
            Worksheet.Cell(String.Format("I{0}", rowIndex)).Value = row.PO;
            if(Double.TryParse(row.PriceUS, out double _))
            {
                Worksheet.Cell(String.Format("J{0}", rowIndex)).Value = Double.Parse(row.PriceUS);
                Worksheet.Cell(String.Format("J{0}", rowIndex)).Style.NumberFormat.Format = "$#,##0.00";
            }
            else Worksheet.Cell(String.Format("J{0}", rowIndex)).Value = row.PriceUS;
            if (Double.TryParse(row.PriceCan, out double _))
            {
                Worksheet.Cell(String.Format("K{0}", rowIndex)).Value = Double.Parse(row.PriceCan);
                Worksheet.Cell(String.Format("K{0}", rowIndex)).Style.NumberFormat.Format = "$#,##0.00";
            }
            else Worksheet.Cell(String.Format("K{0}", rowIndex)).Value = row.PriceCan;
            Worksheet.Cell(String.Format("L{0}", rowIndex)).Value = row.ShipTo;
            Worksheet.Cell(String.Format("M{0}", rowIndex)).Value = row.RequestedDeliveryDate;
            Worksheet.Cell(String.Format("N{0}", rowIndex)).Value = row.RevisedDate;
            Worksheet.Cell(String.Format("O{0}", rowIndex)).Value = row.ShipOnDate;
            Worksheet.Cell(String.Format("P{0}", rowIndex)).Value = row.CustRefNumber;
            Worksheet.Cell(String.Format("Q{0}", rowIndex)).Value = row.ZengerJob;
            Worksheet.Cell(String.Format("R{0}", rowIndex)).Value = row.CSR;
            Worksheet.Cell(String.Format("S{0}", rowIndex)).Value = row.Color;
            Worksheet.Cell(String.Format("T{0}", rowIndex)).Value = row.Stock;
            Worksheet.Cell(String.Format("U{0}", rowIndex)).Value = row.Status;
        }
        public void CleanUp()
        {
            Workbook.Dispose();
        }
    }
}
