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
        public DataHandler(IList<IList<object>> reportingData)
        {
            Rows = new Row[reportingData.Count - 1];
            for (int i = 1; i < reportingData.Count; i++)
            {
                Rows[i - 1] = new Row(reportingData[i]);
            }
        }
        public void AddJobData(IList<IList<object>> jobData)
        {
            List<int> badRows = new List<int>();
            foreach(Row row in Rows)
            {
                for(int i = 2; i < jobData.Count; i++)
                {
                    if (badRows.Contains(i)) continue;
                    if (jobData[i].Count < 2)
                    {
                        Logger.WriteLog("Issue on row {0} of jobs sheet.", false, i.ToString());
                        badRows.Add(i);
                        continue;
                    }
                    if (jobData[i][1].ToString() == row.PO)
                    {
                        row.IngestJobData(jobData[i]);
                        break;
                    }
                    
                }
            }
        }
        public Row[] SortRows()
        {
            List<Row> sortedRows = new List<Row>();
            List<Row> unsortedRows = Rows.ToList<Row>();
            while(unsortedRows.Count > 0)
            {
                Row lowestRow = unsortedRows[0];
                int lowestIndex = 0;
                for(int i = 1; i < unsortedRows.Count; i++)
                {
                    if (DateTime.Parse(unsortedRows[i].RequestedDeliveryDate) < DateTime.Parse(lowestRow.RequestedDeliveryDate))
                    {
                        lowestRow = unsortedRows[i];
                        lowestIndex = i;
                    }
                }
                sortedRows.Add(lowestRow);
                unsortedRows.RemoveAt(lowestIndex);
            }
            return sortedRows.ToArray();
        }
    }
}