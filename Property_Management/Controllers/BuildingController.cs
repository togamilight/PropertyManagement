using Property_Management.BLL.IService;
using Property_Management.BLL.Service;
using Property_Management.DAL.Entities;
using Property_Management.Filters;
using Property_Management.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Property_Management.Controllers
{
    public class BuildingController : Controller
    {
        private IBuildingService buildingService = new BuildingService();
        // GET: Building
        public ActionResult Index()
        {
            return View("BuildingManage");
        }

        public ActionResult BuildingManage() {
            return View();
        }

        [JsonExceptionFilter]
        public ActionResult AddBuilding(Building building) {
            return Json(buildingService.Add(building));
        }

        [JsonExceptionFilter]
        public ActionResult UpdateBuilding(Building building) {
            return Json(buildingService.Update(building));
        }

        [JsonExceptionFilter]
        public ActionResult DeleteBuildings(int[] ids) {
            return Json(buildingService.Delete(ids));
        }


        [JsonExceptionFilter]
        public ActionResult GetBuildingPage(int page = 1, int pageSize = 10, string name = "", string beginDate = "", string endDate = "") {
            var where = PredicateBuilder.True<Building>();

            if (!string.IsNullOrEmpty(name)) {
                where = where.And(b => b.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(beginDate)) {
                DateTime begin;
                if(DateTime.TryParse(beginDate, out begin)) {
                    where = where.And(b => b.BuildDate >= begin);
                }
            }

            if (!string.IsNullOrEmpty(endDate)) {
                DateTime end;
                if (DateTime.TryParse(endDate, out end)) {
                    where = where.And(b => b.BuildDate <= end);
                }
            }

            return Json(buildingService.QueryToPage(where, page, pageSize));
        }
    }
}