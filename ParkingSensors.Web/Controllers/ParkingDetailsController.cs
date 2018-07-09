using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PoCParkingSensors.DataAccess;
using PoCParkingSensors.Models;

namespace ParkingSensors.Web.Controllers
{
    public class ParkingDetailsController : Controller
    {
        private static List<Data> _data = new List<Data>();
        private List<Data> _model = new List<Data>();
        private const string CityOfMelbournePrivateKey = "H9Kf4H7aWIivdDgCHgYIauQyd";
        private const string RefNumber = "dtpv-d4pf";

        private static Data currentData = new Data();
        private const string KeyForMaps = "AIzaSyDklNWhszRKRIj_KOFO_5-xGQs-CeoRdoI";

        public ActionResult ParkingDetails()
        {
            return View(_model);
        }

        public ActionResult Refresh()
        {
            _data = Socrata.GetDataSet(RefNumber, CityOfMelbournePrivateKey).Values.ToList();
            _model = _data;

            var occupiedBays = _model.Count(x => x.Status.Equals("Present"));
            var unOccupiedBays = _model.Count(x => x.Status.Equals("Unoccupied"));

            ViewData.Add("occupiedBays", occupiedBays);
            ViewData.Add("UnOccupiedBays", unOccupiedBays);
            return View("ParkingListPartial", _model);
        }

        public ActionResult Search(string searchString)
        {
            if (_data.Count < 1)
            {
                _data = Socrata.GetDataSet(RefNumber, CityOfMelbournePrivateKey).Values.ToList();
                _model = _data;
            }

            try
            {
                _model = _data.Where(x => x.BayId.Equals(Convert.ToInt32(searchString))).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View("ParkingListPartial", _model);
        }

        public ActionResult ParkingSpotDetail(int bayId)
        {
            currentData = _data.FirstOrDefault(x => x.BayId == bayId);
            ViewData.Add("mapImage", MapUrlGenerator(currentData.Lat, currentData.Lon, KeyForMaps));

            GeocodingModel.Result geocodingResult = Geocoding.GETAsync(currentData.Lat, currentData.Lon).Result.results.First();

            currentData.Address = geocodingResult.formatted_address;

            return View(currentData);
        }

        private string MapUrlGenerator(double lat, double lon, string key)
        {
            return "https://maps.googleapis.com/maps/api/staticmap?center=Melbourne,Victoria&zoom=14&size=600x600&maptype=roadmap&markers=color:blue%7Clabel:S%7C" + lat + "," + lon + "&key=" + key;
        }
    }
}