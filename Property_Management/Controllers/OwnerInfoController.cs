using Property_Management.BLL.Service;
using Property_Management.DAL.Entities;
using Property_Management.Filters;
using Property_Management.Models;
using Property_Management.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property_Management.Controllers
{
    [AccountFilter("Owner")]
    public class OwnerInfoController : Controller
    {
        // GET: OwnerInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePassword() {
            return View();
        }

        [JsonExceptionFilter]
        public ActionResult DoChangePassword(string oldPassword, string newPassword) {
            var service = new OwnerService();

            var account = Session["Account"] as AccountInfo;
            var owner = new Owner() {
                Name = account.Name,
                Password = oldPassword
            };
            return Json(service.ChangePassword(owner, newPassword));
        }

        public ActionResult LogOut() {
            Session["Account"] = null;
            return RedirectToAction("OwnerLogin", "Account");
        }

        public ActionResult Announcement() {
            return View();
        }

        public ActionResult Fee() {
            return View();
        }

        public ActionResult Repair() {
            return View();
        }

        public ActionResult OwnerBaseInfo() {
            var ownerService = new OwnerService();
            var account = Session["Account"] as AccountInfo;
            var baseInfo = ownerService.GetBaseInfoForOwner(account.Id).Result as OwnerBaseInfo;
            return View(baseInfo);
        }

        [JsonExceptionFilter]
        public ActionResult GetAnnouncementPage(int page = 1, int pageSize = 10, string title = "", string beginDate = "", string endDate = "") {
            var announcementService = new AnnouncementService();
            var where = PredicateBuilder.True<Announcement>();

            if (!string.IsNullOrEmpty(title)) {
                where = where.And(a => a.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(beginDate)) {
                DateTime begin;
                if (DateTime.TryParse(beginDate, out begin)) {
                    where = where.And(a => a.Date >= begin);
                }
            }

            if (!string.IsNullOrEmpty(endDate)) {
                DateTime end;
                if (DateTime.TryParse(endDate, out end)) {
                    where = where.And(a => a.Date <= end);
                }
            }

            return Json(announcementService.QueryToPage(where, page, pageSize));
        }

        [JsonExceptionFilter]
        public ActionResult GetFeePage(int page = 1, int pageSize = 10, int feeItemId = 0, string beginDate = "", string endDate = "", int isFinish = 2) {
            var feeService = new FeeService();

            var where = PredicateBuilder.True<Fee>();

            var ownerId = (Session["Account"] as AccountInfo).Id;
            where = where.And(f => f.OwnerId == ownerId);

            if (feeItemId > 0) {
                where = where.And(f => f.FeeItemId == feeItemId);
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

            if (isFinish == 1) {
                where = where.And(f => f.FinishDate != null);
            }
            else if (isFinish == 0) {
                where = where.And(f => f.FinishDate == null);
            }

            return Json(feeService.QueryToPageByOwner(where, page, pageSize));
        }



        [JsonExceptionFilter]
        public ActionResult AddRepair(Repair repair) {
            repair.OwnerId = (Session["Account"] as AccountInfo).Id;
            repair.ApplyDate = DateTime.Now.Date;
            var repairService = new RepairService();
            return Json(repairService.Add(repair));
        }

        [JsonExceptionFilter]
        public ActionResult UpdateRepair(Repair repair) {
            repair.OwnerId = (Session["Account"] as AccountInfo).Id;
            var repairService = new RepairService();
            return Json(repairService.UpdateByOwner(repair));
        }

        [JsonExceptionFilter]
        public ActionResult DeleteRepairs(int[] ids) {
            var repairService = new RepairService();
            var ownerId = (Session["Account"] as AccountInfo).Id;
            return Json(repairService.DeleteByOwner(ids, ownerId));
        }


        [JsonExceptionFilter]
        public ActionResult GetRepairPage(int page = 1, int pageSize = 10, string beginDate = "", string endDate = "", int isFinish = 2) {
            var repairService = new RepairService();

            var where = PredicateBuilder.True<Repair>();

            var ownerId = (Session["Account"] as AccountInfo).Id;
            where = where.And(f => f.OwnerId == ownerId);

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

            return Json(repairService.QueryToPageByOwner(where, page, pageSize));
        }
    }
}