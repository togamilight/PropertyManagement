using Property_Management.BLL.IService;
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
    public class ReplyController : Controller
    {
        private IReplyService replyService = new ReplyService();

        [AccountFilter("Admin")]
        [JsonExceptionFilter]
        public ActionResult AddReply(Reply reply) {
            var adminId = (Session["Account"] as AccountInfo).Id;
            reply.AdminId = adminId;

            return Json(replyService.Add(reply));
        }

        [AccountFilter("Admin")]
        [JsonExceptionFilter]
        public ActionResult DeleteReply(int id) {
            return Json(replyService.Delete(id));
        }

        [AccountFilter("Both")]
        [JsonExceptionFilter]
        public ActionResult GetReplyPage(int page = 1, int pageSize = 5, int adviceId = 0) {
            var where = PredicateBuilder.True<Reply>();

            if(adviceId <= 0) {
                return Json(new ResultInfo(false, "该投诉建议记录不存在", null));
            }

            where = where.And(r => r.AdviceId == adviceId);
            return Json(replyService.QueryToPage(where, page, pageSize));
        }
    }
}