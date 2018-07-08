using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PoCParkingSensors.Models;

namespace ParkingSensors.Web.Controllers
{
    public class ParkingDetailsController : Controller
    {
        private static List<Data> _data = new List<Data>();
        private List<Data> _model = new List<Data>();
        private const string PrivateKey = "H9Kf4H7aWIivdDgCHgYIauQyd";
        private const string RefNumber = "dtpv-d4pf";

        public ActionResult ParkingDetails()
        {
            return View(_model);
        }

        public ActionResult Refresh()
        {
            _data = PoCParkingSensors.DataAccess.Socrata.GetDataSet(RefNumber, PrivateKey).Values.ToList();
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
                _data = PoCParkingSensors.DataAccess.Socrata.GetDataSet(RefNumber, PrivateKey).Values.ToList();
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

        public ActionResult Details(int bayId, double lat, double lon, string status)
        {
            return RedirectToAction("ParkingSpotDetail", "ParkingSpotDetail", new { bayId, lat, lon, status });
        }
    }
}