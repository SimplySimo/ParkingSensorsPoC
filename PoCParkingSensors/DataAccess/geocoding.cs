using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PoCParkingSensors.Models;

namespace PoCParkingSensors.DataAccess
{
    class Geocoding
    {
        private static string key = "AIzaSyDklNWhszRKRIj_KOFO_5-xGQs-CeoRdoI";

        public static async Task<GeocodingModel.RootObject> GETAsync(double lat, double lng)
        {
            GeocodingModel.RootObject root = new GeocodingModel.RootObject();
            string requrstUrl = "https://maps.googleapis.com/maps/api/geocode/json?";
            string latlng = "latlng=" + lat + "," + lng;
            string keyString = "&key=" + key;

            HttpClient request = new HttpClient();

            try
            {
                HttpResponseMessage response = await request.GetAsync(requrstUrl + latlng + keyString);
                string result = await response.Content.ReadAsStringAsync();
                root = JsonConvert.DeserializeObject<GeocodingModel.RootObject>(result);

            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    string errorText = reader.ReadToEnd();
                    Console.WriteLine(errorText);
                }
                throw;
            }
            return root;
        }
    }
}
