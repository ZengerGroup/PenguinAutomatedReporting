using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PenguinAutomatedReporting
{
    internal class GoogleHandler
    {
        ServiceAccountCredential serviceCredentials;
        GoogleCredential googleCredentials;
        GoogleCredential scopedCredentials;
        SheetsService Sheets;
        Spreadsheet Workbook;
        public GoogleHandler()
        {
            serviceCredentials = CredentialFactory.FromJson<ServiceAccountCredential>(File.ReadAllText(Configurator.CredentialsPath));
            googleCredentials = GoogleCredential.FromServiceAccountCredential(serviceCredentials);
            scopedCredentials = googleCredentials.CreateScoped(Google.Apis.Sheets.v4.SheetsService.Scope.Spreadsheets);
        }
        public async Task<bool> VerifyConnection()
        {
            try
            {
                string token = await scopedCredentials.UnderlyingCredential.GetAccessTokenForRequestAsync();
                Logger.WriteLog("Token received!", false);
                return !string.IsNullOrEmpty(token);
            }
            catch (Exception e)
            {
                Logger.WriteLog(e.Message, false);
                return false;
            }
        }
        public void SetupApi()
        {
            try
            {
                Sheets = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = scopedCredentials,
                    ApplicationName = "Penguin PO Tracker"
                });
                Workbook = Sheets.Spreadsheets.Get(Configurator.SheetId).Execute();
            }
            catch (Exception e)
            {
                Logger.ErrorExit([e.Message], 15);
            }
        }
        public async Task<IList<IList<object>>> GetSheetData(string sheetName)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request = Sheets.Spreadsheets.Values.Get(Configurator.SheetId, sheetName);
            ValueRange response = await request.ExecuteAsync();
            IList<IList<object>> values = response.Values;
            if (values != null & values.Count > 0) return values;
            else return null;
        }
    }
}
