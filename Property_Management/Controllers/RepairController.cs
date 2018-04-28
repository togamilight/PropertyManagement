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
    [AccountFilter("Admin")]
    public class RepairController : Controller
    {
        private IRepairService repairService = new RepairService();
        // GET: Repair
        public ActionResult Index() {
            return View();
        }

        public ActionResult RepairManage() {
            return View();
        }

        [JsonExceptionFilter]
        public ActionResult AddRepair(Repair repair) {
            return Json(repairService.Add(repair));
        }

        [JsonExceptionFilter]
        public ActionResult UpdateRepair(Repair repair) {
            return Json(repairService.Update(repair));
        }

        [JsonExceptionFilter]
        public ActionResult DeleteRepairs(int[] ids) {
            return Json(repairService.Delete(ids));
        }


        [JsonExceptionFilter]
        public ActionResult GetRepairPage(int page = 1, int pageSize = 10, int ownerId = 0, string beginDate = "", string endDate = "", int isFinish = 2, int isDisuse = 2) {
            var where = PredicateBuilder.True<Repair>();

            if (ownerId > 0) {
                where = where.And(f => f.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(beginDate)) {
                DateTime begin;
                if (DateTime.TryParse(beginDate, out begin)) {
                    where = where.And(f => f.ApplyDate >= begin);
                }
            }

            if (!string.IsNullOrEmpty(endDate)) {
                DateTime end;
                if (DateTime.TryParse(endDate, out end)) {
                    where = where.And(f => f.ApplyDate <= end);
                }
            }

            if (isFinish == 1) {
                where = where.And(f => f.FinishDate != null);
            }
            else if (isFinish == 0) {
                where = where.And(f => f.FinishDate == null);
            }

            if (isDisuse == 1) {
                where = where.And(f => f.Disuse);
            }
            else if (isDisuse == 0) {
                where = where.And(f => !f.Disuse);
            }

            return Json(repairService.QueryToPage(where, page, pageSize));
        }
    }
}