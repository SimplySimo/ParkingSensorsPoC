using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoCParkingSensors.Models;

namespace ParkingSensors.Web.Controllers
{
    public class ParkingDetailsController : Controller
    {
        public List<Data> model = new List<Data>();
        public string currentPartial = "Default";
        
        public ActionResult ParkingDetails()
        {
            return View(model);
        }

        
        public ActionResult Refresh()
        {
            var privateKey = "H9Kf4H7aWIivdDgCHgYIauQyd";
            const string refNumber = "dtpv-d4pf";
            model = PoCParkingSensors.DataAccess.Socrata.GetDataSet(refNumber, privateKey).Values.ToList();
            return View("ParkingListPartial", model);
        }

        [HttpPost]
        public ActionResult Search(string screen)
        {
            return View("ParkingDetails", model);
        }



    }
}