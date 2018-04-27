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
    public class FeeItemController : Controller
    {
        private IFeeItemService feeItemService = new FeeItemService();
        // GET: FeeItem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FeeItemManage() {
            return View();
        }

        [JsonExceptionFilter]
        public ActionResult AddFeeItem(FeeItem feeItem) {
            return Json(feeItemService.Add(feeItem));
        }

        [JsonExceptionFilter]
        public ActionResult UpdateFeeItem(FeeItem feeItem) {
            return Json(feeItemService.Update(feeItem));
        }

        [JsonExceptionFilter]
        public ActionResult DeleteFeeItems(int[] ids) {
            return Json(feeItemService.Delete(ids));
        }


        [JsonExceptionFilter]
        public ActionResult GetFeeItemPage(int page = 1, int pageSize = 10, string name = "") {
            var where = PredicateBuilder.True<FeeItem>();

            if (!string.IsNullOrEmpty(name)) {
                where = where.And(b => b.Name.Contains(name));
            }

            return Json(feeItemService.QueryToPage(where, page, pageSize));
        }

        [JsonExceptionFilter]
        public ActionResult GetFeeItemsCoreInfo() {
            return Json(feeItemService.GetCoreInfos());
        }

        [JsonExceptionFilter]
        public ActionResult GetFeeItemInfo(int id) {
            return Json(feeItemService.Query(id));
        }
    }
}