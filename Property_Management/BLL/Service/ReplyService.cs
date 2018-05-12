using Property_Management.BLL.Base;
using Property_Management.BLL.IService;
using Property_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Property_Management.Models;
using System.Linq.Expressions;

namespace Property_Management.BLL.Service {
    public class ReplyService : BaseService<Reply>, IReplyService {
        public override ResultInfo Add(Reply reply) {
            string msg = ValidateEntity(reply);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var advice = dbContext.Advices.FirstOrDefault(a => a.Id == reply.AdviceId && !a.Disuse);

            if(advice == null) {
                return new ResultInfo(false, "该投诉建议记录不存在或业主已搬走", null);
            }

            var owner = dbContext.Owners.FirstOrDefault(o => o.Id == advice.OwnerId);

            advice.ReplyNum++;
            advice.NewReplyNum++;
            owner.NewReplyNum++;

            reply.DateTime = DateTime.Now;
            advice.LastReplyTime = reply.DateTime;

            dbContext.Replies.Add(reply);
            dbContext.SaveChanges();

            return new ResultInfo(true, "添加成功", new { reply.Id });
        }

        public override ResultInfo Delete(int id) {
            var reply = dbContext.Replies.FirstOrDefault(e => e.Id == id);

            if (reply == null) {
                return new ResultInfo(false, "不存在该Id的记录", null);
            }

            var advice = dbContext.Advices.FirstOrDefault(a => a.Id == reply.AdviceId /*&& !a.Disuse*/);

            advice.ReplyNum--;

            if(reply.DateTime > advice.LastLookTime) {
                advice.NewReplyNum--;
                var owner = dbContext.Owners.FirstOrDefault(o => o.Id == advice.OwnerId);
                owner.NewReplyNum--;
            }

            //if (advice == null) {
            //    return new ResultInfo(false, "该投诉建议记录不存在或业主已搬走", null);
            //}

            dbContext.Replies.Remove(reply);
            dbContext.SaveChanges();

            return new ResultInfo(true, "删除成功", null);
        }

        public override ResultInfo QueryToPage(Expression<Func<Reply, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 5 ? 5 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Replies.Where(whereLambda);
            var total = entities.Count();
            var replies = entities.OrderBy(e => e.DateTime).Skip(skipCount).Take(pageSize);

            var data = (from r in replies
                       join a in dbContext.Admins
                       on r.AdminId equals a.Id into grp1
                       from g in grp1.DefaultIfEmpty()
                       select new {
                           Reply = r,
                           AdminName = g.Name ?? "管理员"
                       }).OrderBy(e => e.Reply.DateTime);

            return new ResultInfo(true, "", new { Total = total, Data = data });
        }
    }
}