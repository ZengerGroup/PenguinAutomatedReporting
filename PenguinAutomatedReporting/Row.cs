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
        public string Finish;
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
            ZengerJob = reportingData[13].ToString() ?? "";
            TypeOfFormat = reportingData[14].ToString() ?? "";
        }
        public void IngestJobData(IList<object> jobData)
        {
            if (jobData.Count < 15) jobData = AddBlanks(jobData, 15);
            DateReceived = jobData[0].ToString() ?? "";
            Buyer = jobData[2].ToString() ?? "";
            Color = jobData[7].ToString() ?? "";
            Stock = jobData[8].ToString() ?? "";
            Finish = jobData[9].ToString() ?? "";
            Status = jobData[11].ToString() ?? "";
            RequestedDeliveryDate = jobData[12].ToString() ?? "";
            RevisedDate = jobData[13].ToString() ?? "";
            ShipOnDate = jobData[14].ToString() ?? "";
        }
        private IList<object> AddBlanks(IList<object> data, int desiredCount)
        {
            int missing = desiredCount - data.Count;
            for (int i = 0; i < desiredCount; i++) data.Add("");
            return data;
        }
    }
}
