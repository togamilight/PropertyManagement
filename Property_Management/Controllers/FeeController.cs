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
    public class FeeController : Controller
    {
        private IFeeService feeService = new FeeService();
        // GET: Fee
        public ActionResult Index() {
            return View();
        }

        public ActionResult FeeManage() {
            return View();
        }

        public ActionResult FeeStatistics() {
            return View();
        }

        [JsonExceptionFilter]
        public ActionResult AddFee(Fee fee) {
            return Json(feeService.Add(fee));
        }

        [JsonExceptionFilter]
        public ActionResult UpdateFee(Fee fee) {
            return Json(feeService.Update(fee));
        }

        [JsonExceptionFilter]
        public ActionResult DeleteFees(int[] ids) {
            return Json(feeService.Delete(ids));
        }


        [JsonExceptionFilter]
        public ActionResult GetFeePage(int page = 1, int pageSize = 10, int feeItemId = 0, int ownerId = 0, string beginDate = "", string endDate = "", int isFinish = 2, int isDisuse = 2 ) {
            var where = PredicateBuilder.True<Fee>();

            if (feeItemId > 0) {
                where = where.And(f => f.FeeItemId == feeItemId);
            }

            if (ownerId > 0) {
                where = where.And(f => f.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(beginDate)) {
                DateTime begin;
                if (DateTime.TryParse(beginDate, out begin)) {
                    where = where.And(f => f.Date >= begin);
                }
            }

            if (!string.IsNullOrEmpty(endDate)) {
                DateTime end;
                if (DateTime.TryParse(endDate, out end)) {
                    where = where.And(f => f.Date <= end);
                }
            }

            if(isFinish == 1) {
                where = where.And(f => f.FinishDate != null);
            }else if(isFinish == 0) {
                where = where.And(f => f.FinishDate == null);
            }

            if (isDisuse == 1) {
                where = where.And(f => f.Disuse);
            }
            else if (isDisuse == 0) {
                where = where.And(f => !f.Disuse);
            }

            return Json(feeService.QueryToPage(where, page, pageSize));
        }

        [JsonExceptionFilter]
        public ActionResult GetFeeBarData() {
            return Json(feeService.GetBarData());
        }

        [JsonExceptionFilter]
        public ActionResult GetFeePieData(string beginDate, string endDate) {
            return Json(feeService.GetPieData(beginDate, endDate));
        }
    }
}