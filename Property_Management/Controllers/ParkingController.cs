using Property_Management.BLL.IService;
using Property_Management.BLL.Service;
using Property_Management.DAL.Entities;
using Property_Management.Filters;
using Property_Management.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property_Management.Controllers
{
    public class ParkingController : Controller
    {
        private IParkingService parkingService = new ParkingService();
        // GET: Parking
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ParkingManage() {
            return View();
        }

        [JsonExceptionFilter]
        public ActionResult AddParking(Parking parking) {
            return Json(parkingService.Add(parking));
        }

        [JsonExceptionFilter]
        public ActionResult UpdateParking(Parking parking) {
            return Json(parkingService.Update(parking));
        }

        [JsonExceptionFilter]
        public ActionResult DeleteParkings(int[] ids) {
            return Json(parkingService.Delete(ids));
        }


        [JsonExceptionFilter]
        public ActionResult GetParkingPage(int page = 1, int pageSize = 10, string name = "", string carType = "", string carNum = "", string beginDate = "", string endDate = "") {
            var where = PredicateBuilder.True<Parking>();

            if (!string.IsNullOrEmpty(name)) {
                where = where.And(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(carType)) {
                where = where.And(p => p.CarType.Contains(carType));
            }

            if (!string.IsNullOrEmpty(carNum)) {
                where = where.And(p => p.CarNum.Contains(carNum));
            }

            if (!string.IsNullOrEmpty(beginDate)) {
                DateTime begin;
                if (DateTime.TryParse(beginDate, out begin)) {
                    where = where.And(p => p.Date >= begin);
                }
            }

            if (!string.IsNullOrEmpty(endDate)) {
                DateTime end;
                if (DateTime.TryParse(endDate, out end)) {
                    where = where.And(p => p.Date <= end);
                }
            }

            return Json(parkingService.QueryToPage(where, page, pageSize));
        }

        //[JsonExceptionFilter]
        //public ActionResult GetParkingsCoreInfo() {
        //    return Json(parkingService.GetCoreInfos());
        //}

        [JsonExceptionFilter]
        public ActionResult GetParkingInfo(int id) {
            return Json(parkingService.Query(id));
        }
    }
}