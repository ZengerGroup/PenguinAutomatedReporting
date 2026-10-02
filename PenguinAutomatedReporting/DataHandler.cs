using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace PenguinAutomatedReporting
{
    internal class DataHandler
    {
        public Row[] Rows;
        public List<Row> LiveRows;
        public List<Row> ShippedRows;
        public DataHandler(IList<IList<object>> reportingData)
        {
            Rows = new Row[reportingData.Count - 1];
            LiveRows = new List<Row>();
            ShippedRows = new List<Row>();
            for (int i = 1; i < reportingData.Count; i++)
            {
                Rows[i - 1] = new Row(reportingData[i]);
            }
        }
        public void AddJobData(IList<IList<object>> jobData)
        {
            List<int> badRows = new List<int>();
            for(int i = 0; i < Rows.Length; i++)
            {
                for(int ii = 2; ii < jobData.Count; ii++)
                {
                    if (badRows.Contains(ii)) continue;
                    if (jobData[ii].Count < 2)
                    {
                        Logger.WriteLog("Issue on row {0} of jobs sheet.", false, ii.ToString());
                        badRows.Add(ii);
                        continue;
                    }
                    if (jobData[ii][1].ToString() == Rows[i].PO)
                    {
                        Rows[i].IngestJobData(jobData[ii]);
                        LiveRows.Add(Rows[i]);
                        Rows[i] = null;
                        break;
                    }
                }
            }
        }
        public void AddShippedData(IList<IList<object>> shippedData)
        {
            List<int> badRows = new List<int>();
            for (int i = 0; i < Rows.Length; i++)
            {
                if (Rows[i] == null) continue;
                for (int ii = 2; ii < shippedData.Count; ii++)
                {
                    if (badRows.Contains(ii)) continue;
                    if (shippedData[ii].Count < 2)
                    {
                        Logger.WriteLog("Issue on row {0} of jobs sheet.", false, ii.ToString());
                        badRows.Add(ii);
                        continue;
                    }
                    if (shippedData[ii][1].ToString() == Rows[i].PO)
                    {
                        Rows[i].IngestJobData(shippedData[ii]);
                        ShippedRows.Add(Rows[i]);
                        Rows[i] = null;
                        break;
                    }
                }
            }
        }
        public Row[] SortRows(bool liveJobs)
        {
            List<List<Row>> splitRows = (liveJobs) ? SplitByBuyer(LiveRows) : SplitByBuyer(ShippedRows);
            for (int i = 0; i < splitRows.Count; i++) splitRows[i] = SortByDate(splitRows[i], liveJobs);
            splitRows = SortByBuyer(splitRows);
            List<Row> reorderedRows = new List<Row>();
            for (int i = 0; i < splitRows.Count; i++) for (int ii = 0; ii < splitRows[i].Count; ii++) reorderedRows.Add(splitRows[i][ii]);
            return reorderedRows.ToArray();
        }
        private List<List<Row>> SplitByBuyer(List<Row> unsortedRows)
        {
            List<List<Row>> splitRows = new List<List<Row>>();
            for (int i = 0; i < unsortedRows.Count; i++)
            {
                bool found = false;
                for (int ii = 0; ii < splitRows.Count; ii++)
                {
                    if (splitRows[ii][0].Buyer == unsortedRows[i].Buyer)
                    {
                        found = true;
                        splitRows[ii].Add(unsortedRows[i]);
                    }
                }
                if (!found) splitRows.Add(new List<Row> { unsortedRows[i] });
            }
            return splitRows;
        }
        private List<Row> SortByDate(List<Row> unorderedRows, bool liveJobs)
        {
            List<Row> sortedRows = new List<Row>();
            DateTime cutoff = (liveJobs) ? DateTime.Now.AddYears(-5) : DateTime.Now.AddDays(-14);
            while(unorderedRows.Count > 0)
            {
                if (!CheckRowDateQuality(unorderedRows[0], liveJobs))
                {
                    unorderedRows.RemoveAt(0);
                    continue;
                }
                Row lowestRow = unorderedRows[0];
                int lowestIndex = 0;
                for(int i = 1; i < unorderedRows.Count; i++)
                {
                    if (!CheckRowDateQuality(unorderedRows[i], liveJobs)) continue;
                    if (FoundNewLowestRow(lowestRow, unorderedRows[i], liveJobs))
                    {
                        lowestRow = unorderedRows[i];
                        lowestIndex = i;
                    }
                }
                sortedRows.Add(lowestRow);
                unorderedRows.RemoveAt(lowestIndex);
            }
            return sortedRows;
        }
        private List<List<Row>> SortByBuyer(List<List<Row>> unsortedRows)
        {
            List<List<Row>> sortedRows = new List<List<Row>>();
            List <string> buyerNames = new List<string>();
            Console.WriteLine("{0} : {1}", buyerNames.Count, unsortedRows.Count);
            for (int i = 0; i < unsortedRows.Count; i++)
            {
                if (unsortedRows[i].Count == 0) continue;
                Console.WriteLine(i);
                buyerNames.Add(unsortedRows[i][0].Buyer);
            }
            buyerNames.Sort();
            for(int i = 0; i < buyerNames.Count; i++)
            {
                for (int ii = 0; ii < unsortedRows.Count; ii++) 
                {
                    if (unsortedRows[ii].Count == 0) continue;
                    if (buyerNames[i] == unsortedRows[ii][0].Buyer)
                    {
                        sortedRows.Add(unsortedRows[ii]);
                        break;
                    }
                }       
            }
            return sortedRows;
        }
        private bool CheckRowDateQuality(Row row, bool liveJobs)
        {
            if (row == null) return false;
            if (liveJobs) return DateTime.TryParse(row.RequestedDeliveryDate, out _);
            else return DateTime.TryParse(row.ShipOnDate, out _);
        }
        private bool FoundNewLowestRow(Row lowestRow, Row nextRow, bool liveJobs)
        {
            if (liveJobs) return (DateTime.Parse(nextRow.RequestedDeliveryDate) < DateTime.Parse(lowestRow.RequestedDeliveryDate));
            else return (DateTime.Parse(nextRow.ShipOnDate) < DateTime.Parse(lowestRow.ShipOnDate));
        }
    }
}