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
    public class RoomService : BaseService<Room>, IRoomService {
        public override ResultInfo QueryToPage(Expression<Func<Room, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Set<Room>().Where(whereLambda);
            var total = entities.Count();
            var rooms = entities.OrderByDescending(e => e.Id).Skip(skipCount).Take(pageSize);

            var data = from r in rooms
                       join b in dbContext.Set<Building>()
                       on r.BuildingId equals b.Id into grp1
                       from g1 in grp1.DefaultIfEmpty()
                       select new {
                           r.Id,
                           r.Name,
                           r.Area,
                           r.Floor,
                           r.BuildingId,
                           r.OwnerId,
                           BuildingName = g1.Name,
                           OwnerName = ""
                       };

            return new ResultInfo(true, "", new { Total = total, Data = data });
        }
    }
}