using DocumentFormat.OpenXml.Spreadsheet;

namespace PenguinAutomatedReporting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger.WriteLog("Starting process", true);
            //Connect to sheets
            GoogleHandler GSheet = new GoogleHandler();
            if (!GSheet.VerifyConnection().Result) Logger.ErrorExit(["Unable to connect to google API."], 12);
            else GSheet.SetupApi();
            //Read entirety of reporting tab, create list of rows
            DataHandler Data = new DataHandler(GSheet.GetSheetData("TO_Reporting").Result);
            Logger.Display("Found {0} rows to process.", false, Data.Rows.Length.ToString());
            //add jobs tab details to each row
            Data.AddJobData(GSheet.GetSheetData("Jobs").Result);
            Data.AddShippedData(GSheet.GetSheetData("Shipped").Result);
            //Create Excel file
            ExcelHandler ESheets = new ExcelHandler();
            //Foreach row add it to excel
            ESheets.FillData(Data.SortRows(true), ESheets.LiveWorkbook, ESheets.LiveWorksheet, ESheets.LiveSheetPath);
            ESheets.FillData(Data.SortRows(false), ESheets.ShippedWorkbook, ESheets.ShippedWorksheet, ESheets.ShippedSheetPath);
            //Send mail with excel file attached
            Mailer MailSender = new Mailer();
            MailSender.SendMail(ESheets.LiveSheetPath, ESheets.ShippedSheetPath);
            ESheets.CleanUp();
        }
    }
}
