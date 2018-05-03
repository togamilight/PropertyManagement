using Property_Management.BLL.Base;
using Property_Management.BLL.IService;
using Property_Management.DAL.Entities;
using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace Property_Management.BLL.Service {
    public class AnnouncementService : BaseService<Announcement>, IAnnouncementService {
        public override ResultInfo QueryToPage(Expression<Func<Announcement, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Set<Announcement>().Where(whereLambda);
            var total = entities.Count();
            var data = entities.OrderByDescending(e => e.DateTime).Skip(skipCount).Take(pageSize).ToList();

            return new ResultInfo(true, "", new { Total = total, Data = data });
        }

        public override ResultInfo Add(Announcement a) {
            string msg = ValidateEntity(a);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            a.DateTime = DateTime.Now;

            dbContext.Set<Announcement>().Add(a);
            dbContext.SaveChanges();

            return new ResultInfo(true, "添加成功", new { a.Id });
        }

        public override ResultInfo Update(Announcement a) {
            string msg = ValidateEntity(a);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var announ = dbContext.Announcements.FirstOrDefault(e => e.Id == a.Id);
            if(announ == null) {
                return new ResultInfo(false, "该公告不存在", null);
            }

            announ.Title = a.Title;
            announ.Content = a.Content;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public int GetUnReadCountForOwner(int id) {
            var owner = dbContext.Owners.FirstOrDefault(o => o.Id == id);
            if(owner == null) {
                return 0;
            }

            var count = dbContext.Announcements.Where(a => a.DateTime > owner.LastLookTime).Count();
            return count;
        }
    }
}