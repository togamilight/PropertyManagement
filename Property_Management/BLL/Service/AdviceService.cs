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
    public class AdviceService : BaseService<Advice>, IAdviceService {
        public override ResultInfo QueryToPage(Expression<Func<Advice, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Advices.Where(whereLambda);
            var total = entities.Count();
            var advices = entities.OrderByDescending(e => e.DateTime).Skip(skipCount).Take(pageSize);

            var data = from a in advices
                       join o in dbContext.Owners
                       on a.OwnerId equals o.Id into grp1
                       from g in grp1.DefaultIfEmpty()
                       select new {
                           Advice = a,
                           OwnerName = g.Name
                       };

            return new ResultInfo(true, "", new { Total = total, Data = data });
        }

        public override ResultInfo Add(Advice advice) {
            string msg = ValidateEntity(advice);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            if(!dbContext.Owners.Any(o => o.Id == advice.OwnerId && !o.Disuse)) {
                return new ResultInfo(false, "该业主不存在或已搬走", null);
            }

            advice.DateTime = DateTime.Now;
            advice.LastReplyTime = advice.DateTime;
            advice.LastLookTime = advice.DateTime;
            advice.NewReplyNum = 0;
            advice.ReplyNum = 0;
            advice.Disuse = false;

            dbContext.Advices.Add(advice);
            dbContext.SaveChanges();

            return new ResultInfo(true, "添加成功", new { advice.Id });
        }

        public override ResultInfo Update(Advice advice) {
            string msg = ValidateEntity(advice);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var oldAdvice = dbContext.Advices.FirstOrDefault(a => a.Id == advice.Id);
            if(oldAdvice == null) {
                return new ResultInfo(false, "该记录不存在", null);
            }

            oldAdvice.Content = advice.Content;
            oldAdvice.Title = advice.Title;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public override ResultInfo Delete(int[] ids) {
            foreach (var id in ids) {
                var advice = dbContext.Advices.FirstOrDefault(e => e.Id == id);
                if (advice != null) {
                    var owner = dbContext.Owners.FirstOrDefault(o => o.Id == advice.OwnerId);
                    owner.NewReplyNum -= advice.NewReplyNum;

                    var replies = dbContext.Replies.Where(r => r.AdviceId == advice.Id);

                    dbContext.Replies.RemoveRange(replies);
                    dbContext.Advices.Remove(advice);
                }
            }

            dbContext.SaveChanges();

            return new ResultInfo(true, "删除成功", null);
        }

        public ResultInfo QueryToPageByOwner(Expression<Func<Advice, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Advices.Where(whereLambda);
            var total = entities.Count();
            var data = entities.OrderByDescending(e => e.LastReplyTime).Skip(skipCount).Take(pageSize).ToList();

            return new ResultInfo(true, "", new { Total = total, Data = data });
        }

        public ResultInfo UpdateByOwner(Advice advice) {
            string msg = ValidateEntity(advice);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var oldAdvice = dbContext.Advices.FirstOrDefault(a => a.Id == advice.Id && a.OwnerId == advice.OwnerId && !a.Disuse);
            if (oldAdvice == null) {
                return new ResultInfo(false, "该记录不存在", null);
            }

            if(oldAdvice.ReplyNum > 0) {
                return new ResultInfo(false, "无法修改已被管理员回复的记录", null);
            }

            oldAdvice.Content = advice.Content;
            oldAdvice.Title = advice.Title;
            oldAdvice.DateTime = DateTime.Now;
            oldAdvice.LastReplyTime = oldAdvice.DateTime;
            oldAdvice.LastLookTime = oldAdvice.DateTime;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public ResultInfo DeleteByOwner(int[] ids, int ownerId) {
            foreach (var id in ids) {
                var advice = dbContext.Advices.FirstOrDefault(e => e.Id == id && e.OwnerId == ownerId && e.ReplyNum <= 0 && !e.Disuse);
                if (advice != null) {
                    dbContext.Advices.Remove(advice);
                }
            }

            dbContext.SaveChanges();

            return new ResultInfo(true, "删除成功", null);
        }

        public ResultInfo UpdateLook(int id, int ownerId) {
            var owner = dbContext.Owners.FirstOrDefault(o => o.Id == ownerId && !o.Disuse);

            if(owner == null) {
                return new ResultInfo(false, "该业主不存在", null);
            }

            var advice = dbContext.Advices.FirstOrDefault(a => a.Id == id && a.OwnerId == ownerId);
            if (advice == null) {
                return new ResultInfo(false, "该记录不存在", null);
            }

            owner.NewReplyNum -= advice.NewReplyNum;
            advice.NewReplyNum = 0;
            advice.LastLookTime = DateTime.Now;

            dbContext.SaveChanges();

            return new ResultInfo(true, "", null);
        }

        public int GetUnReplyCount() {
            return dbContext.Advices.Where(a => a.ReplyNum < 1 && !a.Disuse).Count();
        }

        public ResultInfo GetBarData() {
            var sql = "select Date_Format(DateTime, '%Y-%m') as Month, count(*) as Count from advice group by Month order by Month; ";

            var data = dbContext.Database.SqlQuery<AdviceBarData>(sql).ToList();

            return new ResultInfo(true, "", data);
        }
    }
}