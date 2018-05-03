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
    public class AnnouncementController : Controller
    {
        private IAnnouncementService announcementService = new AnnouncementService();
        // GET: Announcement
        public ActionResult Index() {
            return View();
        }

        public ActionResult AnnouncementManage() {
            return View();
        }

        [JsonExceptionFilter]
        public ActionResult AddAnnouncement(Announcement announcement) {
            return Json(announcementService.Add(announcement));
        }

        [JsonExceptionFilter]
        public ActionResult UpdateAnnouncement(Announcement announcement) {
            return Json(announcementService.Update(announcement));
        }

        [JsonExceptionFilter]
        public ActionResult DeleteAnnouncements(int[] ids) {
            return Json(announcementService.Delete(ids));
        }


        [JsonExceptionFilter]
        public ActionResult GetAnnouncementPage(int page = 1, int pageSize = 10, string title = "", string beginDate = "", string endDate = "") {
            var where = PredicateBuilder.True<Announcement>();

            if (!string.IsNullOrEmpty(title)) {
                where = where.And(a => a.Title.Contains(title));
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
                    end.AddDays(1);
                    where = where.And(a => a.DateTime < end);
                }
            }

            return Json(announcementService.QueryToPage(where, page, pageSize));
        }
    }
}