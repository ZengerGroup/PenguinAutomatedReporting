using DocumentFormat.OpenXml.Drawing;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PenguinAutomatedReporting
{
    internal class Row
    {
        public string Imprint;
        public string ISBN;
        public string Title;
        public string Author;
        public string CustomerPrintNumber;
        public string TypeOfFormat;
        public string Buyer;
        public string OrderQty;
        public string PO;
        public string PriceUS;
        public string PriceCan;
        public string ShipTo;
        public string RequestedDeliveryDate;
        public string RevisedDate;
        public string DateReceived;
        public string Color;
        public string Stock;
        public string Finish1;
        public string Finish2;
        public string Status;
        public string ShipOnDate;
        public string CustRefNumber;
        public string ZengerJob;
        public string CSR;
        public Row(IList<object> reportingData)
        {
            if (reportingData.Count < 15) reportingData = AddBlanks(reportingData, 14);
            PO = reportingData[0].ToString() ?? "";
            Title = reportingData[1].ToString() ?? "";
            Author = reportingData[2].ToString() ?? "";
            ISBN = reportingData[3].ToString() ?? "";
            Imprint = reportingData[4].ToString() ?? "";
            CustomerPrintNumber = reportingData[5].ToString() ?? "";
            OrderQty = reportingData[7].ToString() ?? "";
            PriceUS = reportingData[8].ToString() ?? "";
            PriceCan = reportingData[9].ToString() ?? "";
            ShipTo = reportingData[10].ToString() ?? "";
            CSR = reportingData[11].ToString() ?? "";
            CustRefNumber = reportingData[12].ToString() ?? "";
            TypeOfFormat = reportingData[14].ToString() ?? "";
        }
        public void IngestJobData(IList<object> jobData)
        {
            if (jobData.Count < 16) jobData = AddBlanks(jobData, 16);
            ZengerJob = jobData[2].ToString() ?? "";
            DateReceived = jobData[0].ToString() ?? "";
            Buyer = jobData[3].ToString() ?? "";
            Color = jobData[7].ToString() ?? "";
            Stock = jobData[8].ToString() ?? "";
            Finish1 = jobData[9].ToString() ?? "";
            Finish2 = jobData[10].ToString() ?? "";
            Status = jobData[12].ToString() ?? "";
            RequestedDeliveryDate = jobData[13].ToString() ?? "";
            RevisedDate = jobData[14].ToString() ?? "";
            ShipOnDate = jobData[15].ToString() ?? "";
        }
        public void IngestShippedData(IList<object> shippedData)
        {
            if (shippedData.Count < 16) shippedData = AddBlanks(shippedData, 16);
            ZengerJob = shippedData[4].ToString() ?? "";
            DateReceived = shippedData[0].ToString() ?? "";
            Buyer = shippedData[2].ToString() ?? "";
            Color = shippedData[8].ToString() ?? "";
            Stock = shippedData[9].ToString() ?? "";
            Finish1 = shippedData[10].ToString() ?? "";
            Finish2 = shippedData[11].ToString() ?? "";
            Status = shippedData[13].ToString() ?? "";
            RequestedDeliveryDate = shippedData[14].ToString() ?? "";
            RevisedDate = "";
            ShipOnDate = shippedData[15].ToString() ?? "";
        }
        private IList<object> AddBlanks(IList<object> data, int desiredCount)
        {
            int missing = desiredCount - data.Count;
            for (int i = 0; i < desiredCount; i++) data.Add("");
            return data;
        }
    }
}
