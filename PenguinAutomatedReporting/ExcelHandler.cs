using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PenguinAutomatedReporting
{
    internal class ExcelHandler
    {
        public string LiveSheetPath;
        public string ShippedSheetPath;
        public XLWorkbook LiveWorkbook;
        public XLWorkbook ShippedWorkbook;
        public IXLWorksheet LiveWorksheet;
        public IXLWorksheet ShippedWorksheet;
        public ExcelHandler()
        {
            LiveSheetPath = Path.Combine(Path.GetTempPath(), String.Format("{0}_Penguin_Live.xlsx", DateTime.Now.ToString("MM-dd-yyyy")));
            ShippedSheetPath = Path.Combine(Path.GetTempPath(), String.Format("{0}_Penguin_Shipped.xlsx", DateTime.Now.ToString("MM-dd-yyyy")));
            LiveWorkbook = new XLWorkbook();
            ShippedWorkbook = new XLWorkbook();
            LiveWorksheet = LiveWorkbook.Worksheets.Add("Reporting");
            ShippedWorksheet = ShippedWorkbook.Worksheets.Add("Reporting");
            AddHeaders(LiveWorksheet);
            AddHeaders(ShippedWorksheet);
        }
        public void FillData(Row[] rows, XLWorkbook book, IXLWorksheet sheet, string path)
        {
            for(int i = 0; i < rows.Length; i++) AddData(rows[i], (i + 2), sheet);
            sheet.Columns().AdjustToContents();
            book.SaveAs(path);
        }
        private void AddHeaders(IXLWorksheet sheet)
        {
            sheet.Cell("A1").Value = "Imprint";
            sheet.Cell("B1").Value = "ISBN";
            sheet.Cell("C1").Value = "Title";
            sheet.Cell("D1").Value = "Author";
            sheet.Cell("E1").Value = "Customer Print #";
            sheet.Cell("F1").Value = "Format";
            sheet.Cell("G1").Value = "Buyer";
            sheet.Cell("H1").Value = "Order Qty";
            sheet.Cell("I1").Value = "PO";
            sheet.Cell("J1").Value = "Price USA";
            sheet.Cell("K1").Value = "Price CAN";
            sheet.Cell("L1").Value = "Ship To";
            sheet.Cell("M1").Value = "Requested Delivery Date";
            sheet.Cell("N1").Value = "Revised Date";
            sheet.Cell("O1").Value = "Ship On Date";
            sheet.Cell("P1").Value = "Customer Reference #";
            sheet.Cell("Q1").Value = "Zenger Job #";
            sheet.Cell("R1").Value = "CSR";
            sheet.Cell("S1").Value = "Color";
            sheet.Cell("T1").Value = "Stock";
            sheet.Cell("U1").Value = "Status";
        }
        private void AddData(Row row, int rowIndex, IXLWorksheet sheet)
        {
            sheet.Cell(String.Format("A{0}", rowIndex)).Value = row.Imprint;
            sheet.Cell(String.Format("B{0}", rowIndex)).Value = row.ISBN;
            sheet.Cell(String.Format("C{0}", rowIndex)).Value = row.Title;
            sheet.Cell(String.Format("D{0}", rowIndex)).Value = row.Author;
            sheet.Cell(String.Format("E{0}", rowIndex)).Value = row.CustomerPrintNumber;
            sheet.Cell(String.Format("F{0}", rowIndex)).Value = row.TypeOfFormat;
            sheet.Cell(String.Format("G{0}", rowIndex)).Value = row.Buyer;
            sheet.Cell(String.Format("H{0}", rowIndex)).Value = row.OrderQty;
            sheet.Cell(String.Format("I{0}", rowIndex)).Value = row.PO;
            if(Double.TryParse(row.PriceUS, out double _))
            {
                sheet.Cell(String.Format("J{0}", rowIndex)).Value = Double.Parse(row.PriceUS);
                sheet.Cell(String.Format("J{0}", rowIndex)).Style.NumberFormat.Format = "$#,##0.00";
            }
            else sheet.Cell(String.Format("J{0}", rowIndex)).Value = row.PriceUS;
            if (Double.TryParse(row.PriceCan, out double _))
            {
                sheet.Cell(String.Format("K{0}", rowIndex)).Value = Double.Parse(row.PriceCan);
                sheet.Cell(String.Format("K{0}", rowIndex)).Style.NumberFormat.Format = "$#,##0.00";
            }
            else sheet.Cell(String.Format("K{0}", rowIndex)).Value = row.PriceCan;
            sheet.Cell(String.Format("L{0}", rowIndex)).Value = row.ShipTo;
            sheet.Cell(String.Format("M{0}", rowIndex)).Value = row.RequestedDeliveryDate;
            sheet.Cell(String.Format("N{0}", rowIndex)).Value = row.RevisedDate;
            sheet.Cell(String.Format("O{0}", rowIndex)).Value = row.ShipOnDate;
            sheet.Cell(String.Format("P{0}", rowIndex)).Value = row.CustRefNumber;
            sheet.Cell(String.Format("Q{0}", rowIndex)).Value = row.ZengerJob;
            sheet.Cell(String.Format("R{0}", rowIndex)).Value = row.CSR;
            sheet.Cell(String.Format("S{0}", rowIndex)).Value = row.Color;
            sheet.Cell(String.Format("T{0}", rowIndex)).Value = row.Stock;
            sheet.Cell(String.Format("U{0}", rowIndex)).Value = row.Status;
        }
        public void CleanUp()
        {
            LiveWorkbook.Dispose();
            ShippedWorkbook.Dispose();
        }
    }
}
