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
    public class AdviceController : Controller {
        private IAdviceService adviceService = new AdviceService();
        // GET: Advice
        public ActionResult Index() {
            return View();
        }

        public ActionResult AdviceManage() {
            return View();
        }

        public ActionResult AdviceStatistics() {
            return View();
        }

        //[JsonExceptionFilter]
        //public ActionResult AddAdvice(Advice advice) {
        //    return Json(adviceService.Add(advice));
        //}

        [JsonExceptionFilter]
        public ActionResult UpdateAdvice(Advice advice) {
            return Json(adviceService.Update(advice));
        }

        [JsonExceptionFilter]
        public ActionResult DeleteAdvices(int[] ids) {
            return Json(adviceService.Delete(ids));
        }


        [JsonExceptionFilter]
        public ActionResult GetAdvicePage(int page = 1, int pageSize = 10, string title = "", int ownerId = 0, string beginDate = "", string endDate = "", int isFinish = 2, int isDisuse = 2) {
            var where = PredicateBuilder.True<Advice>();

            if (!string.IsNullOrEmpty(title)) {
                where = where.And(a => a.Title.Contains(title));
            }

            if (ownerId > 0) {
                where = where.And(a => a.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(beginDate)) {
                DateTime begin;
                if (DateTime.TryParse(beginDate, out begin)) {
                    where = where.And(a => a.DateTime >= begin);
                }
            }

            if (!string.IsNullOrEmpty(endDate)) {
                DateTime end;
                if (DateTime.TryParse(endDate, out end)) {
                    end = end.AddDays(1);
                    where = where.And(a => a.DateTime < end);
                }
            }

            if (isFinish == 1) {
                where = where.And(a => a.ReplyNum > 0);
            }
            else if (isFinish == 0) {
                where = where.And(a => a.ReplyNum <= 0);
            }

            if (isDisuse == 1) {
                where = where.And(a => a.Disuse);
            }
            else if (isDisuse == 0) {
                where = where.And(a => !a.Disuse);
            }

            return Json(adviceService.QueryToPage(where, page, pageSize));
        }

        [JsonExceptionFilter]
        public ActionResult GetAdviceBarData() {
            return Json(adviceService.GetBarData());
        }
    }
}