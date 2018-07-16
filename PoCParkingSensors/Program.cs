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
        private const string PrivateKey = "H9Kf4H7aWIivdDgCHgYIauQyd";
        private const string RefNumber = "dtpv-d4pf";
        private static readonly Dictionary<string, Data> Data = Socrata.GetDataSet(RefNumber, PrivateKey);

        static void Main(string[] args)
        {
            while (_status != null && !_status.ToLower().Equals("quit"))
            {
                Console.WriteLine("----------options avaible--------------");
                Console.WriteLine("display all data");
                Console.WriteLine("display random");
                Console.WriteLine("Convert to address (lat lon)");
                Console.WriteLine("quit");
                Console.WriteLine("");
                _status = Console.ReadLine();

                if (_status.ToLower().Contains("convert to address"))
                {
                    Console.WriteLine("please enter lat lon seperated by a space comma");
                    string latLon = Console.ReadLine();
                    string[] output;

                    if (latLon.Contains(','))
                        output = latLon.Split(',');
                    else if (latLon.Contains(' '))
                        output = latLon.Split(' ');
                    else
                        throw new Exception("unable to read input");

                    var results = Geocoding.GETAsync(Convert.ToDouble(output[0]), Convert.ToDouble(output[1])).Result.results.First();

                    Console.WriteLine(results.formatted_address);
                }
                if (_status.ToLower().Contains("display all"))
                {

                    foreach (var entry in Data)
                    {
                        Console.WriteLine(entry.Value.BayId);
                        Console.WriteLine(entry.Value.Status);
                        Console.WriteLine(entry.Value.Lat);
                        Console.WriteLine(entry.Value.Lon);
                        Console.WriteLine(" ");
                    }
                }
                if (_status.ToLower().Contains("random"))
                {
                    string randomKey = Data.Select(kv => kv.Key)
                        .OrderBy(k => Guid.NewGuid())
                        .Take(1)
                        .First();
                    Data pointToUse = Data[randomKey];
                    GeocodingModel.Result geocodingResult = Geocoding.GETAsync(pointToUse.Lat, pointToUse.Lon).Result.results.First();

                    Console.WriteLine(geocodingResult.formatted_address);
                    Console.WriteLine(pointToUse.Status);
                }
            }
        }
    }
}
