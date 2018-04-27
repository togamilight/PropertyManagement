using Property_Management.BLL.Base;
using Property_Management.BLL.IService;
using Property_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Property_Management.Models;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

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
                       join o in dbContext.Set<Owner>()
                       on r.OwnerId equals o.Id into grp2
                       from g1 in grp1.DefaultIfEmpty()
                       from g2 in grp2.DefaultIfEmpty()
                       select new {
                           Room = r,
                           BuildingName = g1.Name,
                           OwnerName = g2.Name
                       };

            return new ResultInfo(true, "", new { Total = total, Data = data });
        }

        public override ResultInfo Query(int id) {
            var room = dbContext.Set<Room>().Where(r => r.Id == id);
            var data = (from r in room
                       join b in dbContext.Set<Building>()
                       on r.BuildingId equals b.Id into grp1
                       join o in dbContext.Set<Owner>()
                       on r.OwnerId equals o.Id into grp2
                       from g1 in grp1.DefaultIfEmpty()
                       from g2 in grp2.DefaultIfEmpty()
                       select new {
                           Room = r,
                           BuildingName = g1.Name,
                           OwnerName = g2.Name
                       }).FirstOrDefault();

            return new ResultInfo(true, "", data);
        }

        public override ResultInfo Add(Room room) {
            string msg = ValidateEntity(room);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var rooms = dbContext.Set<Room>();
            if (rooms.Any(r => r.Name == room.Name && r.BuildingId == room.BuildingId)) {
                return new ResultInfo(false, "该房号在该楼中已存在", null);
            }
            rooms.Add(room);

            dbContext.SaveChanges();

            return new ResultInfo(true, "添加成功", new { room.Id });
        }

        public override ResultInfo Update(Room room) {
            string msg = ValidateEntity(room);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var rooms = dbContext.Set<Room>();
            var oldRoom = rooms.Where(r => r.Id == room.Id).FirstOrDefault();

            if(oldRoom == null) {
                return new ResultInfo(false, "该房子不存在，修改失败", null);
            }

            if(rooms.Any(r => r.Name == room.Name && r.BuildingId == room.BuildingId && r.Id != room.Id)) {
                return new ResultInfo(false, "该房号在该楼中已存在", null);
            }

            oldRoom.Name = room.Name;
            oldRoom.Area = room.Area;
            oldRoom.BuildingId = room.BuildingId;
            oldRoom.Floor = room.Floor;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public override ResultInfo Delete(int[] ids) {
            var rooms = dbContext.Set<Room>();
            foreach (int id in ids) {
                if (id > 0) {
                    var room = rooms.FirstOrDefault(r => r.Id == id);
                    if (room.OwnerId != null) {
                        return new ResultInfo(false, "删除失败，"+ room.Name + "房子中已有住户，请先为他们分配其它房子", null);
                    }
                    rooms.Remove(room);
                }
            }

            dbContext.SaveChanges();
            return new ResultInfo(true, "删除成功", null);
        }

        public override string ValidateEntity(Room room) {
            var validResults = new List<ValidationResult>();
            string msg = "";
            if (!Validator.TryValidateObject(room, new ValidationContext(room), validResults, true)) {
                foreach (var result in validResults) {
                    msg += result.ErrorMessage + "\n";
                }
            }else {
                var buildings = dbContext.Set<Building>();
                var building = buildings.Where(b => b.Id == room.BuildingId).FirstOrDefault();
                if(building == null) {
                    msg += "该楼栋不存在\n";
                }else if(room.Floor > building.FloorNum) {
                    msg += "所在楼层不能大于楼栋的楼层数\n";
                }
            }

            return msg;
        }

        public ResultInfo GetEmptyRoomInBuilding(int buildingId) {
            var rooms = dbContext.Set<Room>().Where(r => r.BuildingId == buildingId && r.OwnerId == null).Select(r => new { r.Id, r.Name });

            return new ResultInfo(true, "", rooms);
        }
    }
}