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
            var data = entities.OrderByDescending(e => e.Date).Skip(skipCount).Take(pageSize).ToList();

            return new ResultInfo(true, "", new { Total = total, Data = data });
        }
    }
}