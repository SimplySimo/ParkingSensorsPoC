using System;
using System.Collections.Generic;
using System.Linq;
using PoCParkingSensors.Models;
using SODA;

namespace PoCParkingSensors.DataAccess
{
    public static class Socrata
    {
        private static Dictionary<string, Data> _dataSet = new Dictionary<string, Data>();

        public static Dictionary<string, Data> GetDataSet(string referenceName, string privateKey)
        {
            SodaClient client = new SodaClient("https://data.melbourne.vic.gov.au", privateKey);
            var dataset = client.GetResource<Dictionary<string, object>>(referenceName);

            IEnumerable<Dictionary<string, object>> rows = dataset.GetRows(5000);

            Console.WriteLine("Got {0} results.", rows.Count());

            _dataSet = ExtractData(rows);

            return _dataSet;
        }

        private static Dictionary<string, Data> ExtractData(IEnumerable<Dictionary<string, object>> rows)
        {
            Dictionary<string, Data> localData = new Dictionary<string, Data>();

            foreach (Dictionary<string, object> keyValue in rows)
            {
                localData.Add(keyValue["bay_id"].ToString(), new Data
                {
                    BayId = Convert.ToInt32(keyValue["bay_id"]),
                    Lat = Convert.ToDouble(keyValue["lat"]),
                    Lon = Convert.ToDouble(keyValue["lon"]),
                    Status = keyValue["status"].ToString()
                });
            }
            return localData;
        }
    }
}
