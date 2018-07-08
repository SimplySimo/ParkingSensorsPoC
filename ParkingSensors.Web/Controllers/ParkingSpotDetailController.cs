using System.Linq;
using System.Web.Mvc;
using PoCParkingSensors.DataAccess;
using PoCParkingSensors.Models;

namespace ParkingSensors.Web.Controllers
{
    public class ParkingSpotDetailController : Controller
    {
        private const string Key = "AIzaSyDklNWhszRKRIj_KOFO_5-xGQs-CeoRdoI";
        private static Data _model = new Data();
        // GET: ParkingSpotDetails
        public ActionResult ParkingSpotDetail(int bayId, double lat, double lon, string status)
        {
            _model.BayId = bayId;
            _model.Lat = lat;
            _model.Lon = lon;
            _model.Status = status;
            ViewData.Add("mapImage", MapUrlGenerator(lat, lon, Key));
            GeocodingModel.Result geocodingResult = Geocoding.GETAsync(lat, lon).Result.results.First();
            _model.Address = geocodingResult.formatted_address;
            return View(_model);
        }

        private string MapUrlGenerator(double lat, double lon, string key)
        {
            return "https://maps.googleapis.com/maps/api/staticmap?center=Melbourne,Victoria&zoom=14&size=600x600&maptype=roadmap&markers=color:blue%7Clabel:S%7C" + lat + "," + lon + "&key=" + key;
        }
    }
}