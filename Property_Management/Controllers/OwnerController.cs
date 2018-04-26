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
    public class OwnerController : Controller
    {
        private IOwnerService ownerService = new OwnerService();
        //// GET: Owner
        //public ActionResult Owner() {
        //    return View();
        //}

        public ActionResult OwnerManage() {
            return View();
        }

        [JsonExceptionFilter]
        public ActionResult AddOwner(Owner owner) {
            return Json(ownerService.Add(owner));
        }

        [JsonExceptionFilter]
        public ActionResult UpdateOwner(Owner owner) {
            return Json(ownerService.Update(owner));
        }

        [JsonExceptionFilter]
        public ActionResult DeleteOwners(int[] ids) {
            return Json(ownerService.Delete(ids));
        }


        [JsonExceptionFilter]
        public ActionResult GetOwnerPage(int page = 1, int pageSize = 10, string name = "", int sex = 2, string beginDate = "", string endDate = "", bool disuse = false) {
            var where = PredicateBuilder.True<Owner>();

            where = where.And(o => o.Disuse == disuse);

            if (!string.IsNullOrEmpty(name)) {
                where = where.And(o => o.Name.Contains(name));
            }

            switch (sex) {
                case 0: //女
                    where = where.And(o => !o.Sex);
                    break;
                case 1: //男
                    where = where.And(o => o.Sex);
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(beginDate)) {
                DateTime begin;
                if (DateTime.TryParse(beginDate, out begin)) {
                    where = where.And(o => o.Date >= begin);
                }
            }

            if (!string.IsNullOrEmpty(endDate)) {
                DateTime end;
                if (DateTime.TryParse(endDate, out end)) {
                    where = where.And(o => o.Date <= end);
                }
            }

            return Json(ownerService.QueryToPage(where, page, pageSize));
        }
    }
}