using PoCParkingSensors.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using PoCParkingSensors.Models;

namespace PoCParkingSensors
{
    public class Program
    {
        private static string _status = "Start";

        static void Main(string[] args)
        {
            var privateKey = "H9Kf4H7aWIivdDgCHgYIauQyd";
            const string refNumber = "dtpv-d4pf";
            //extract data
            Dictionary<string, Data> data = Socrata.GetDataSet(refNumber, privateKey);
            new Random();
            // make it searchable
            string randomKey = data.Select(kv => kv.Key)
                .OrderBy(k => Guid.NewGuid())
                .Take(1)
                .First();
            Data pointToUse = data[randomKey];
            GeocodingModel.Result geocodingResult = Geocoding.GETAsync(pointToUse.Lat, pointToUse.Lon).Result.results.First();

            Console.WriteLine(geocodingResult.formatted_address);
            Console.WriteLine(pointToUse.Status);

            while (_status != null && !_status.ToLower().Equals("quit"))
            {
                _status = Console.ReadLine();
            }
            //change lat lon to street address //google geo coding

            //geocoding key AIzaSyCwaWfnfkZV3GOkiR6SeD9sIchKok2hIiA

            //mvc website

            //make the dots apear on the map 

            //view searched on on map
        }


    }
}
