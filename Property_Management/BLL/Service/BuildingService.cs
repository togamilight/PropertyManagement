using Property_Management.BLL.Base;
using Property_Management.BLL.IService;
using Property_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Property_Management.Models;

namespace Property_Management.BLL.Service {
    public class BuildingService : BaseService<Building>, IBuildingService {

        public override ResultInfo Add(Building building) {
            string msg = ValidateEntity(building);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var buildings = dbContext.Set<Building>();
            if(buildings.Any(b => b.Name == building.Name)) {
                return new ResultInfo(false, "该楼栋名称已存在，请换另一个名称", null);
            }

            buildings.Add(building);
            dbContext.SaveChanges();

            return new ResultInfo(true, "添加成功", new { building.Id });
        }

        public override ResultInfo Update(Building building) {
            string msg = ValidateEntity(building);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            if(building.Id <= 0) {
                return new ResultInfo(false, "Id错误，修改失败", null);
            }

            if(dbContext.Set<Building>().Any(b => b.Name == building.Name && b.Id != building.Id)) {
                return new ResultInfo(false, "该名称已存在，修改失败", null);
            }

            dbContext.Entry(building).State = System.Data.Entity.EntityState.Modified;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public override ResultInfo Delete(int[] ids) {
            var buildings = dbContext.Set<Building>();
            var rooms = dbContext.Set<Room>();
            foreach (int id in ids) {
                if (id > 0) {
                    //当楼中有房子已经有业主，则不能删除
                    if (rooms.Any(r => r.BuildingId == id && r.OwnerId != null)) {
                        return new ResultInfo(false, "删除失败，编号为" + id + "的楼中已有业主，请先为他们分配其它房子", null);
                    }
                }
            }

            foreach (int id in ids) {
                if (id > 0) {
                    var building = buildings.FirstOrDefault(b => b.Id == id);
                    if (building != null) {
                        var deleteRooms = rooms.Where(r => r.BuildingId == id);
                        rooms.RemoveRange(deleteRooms);

                        buildings.Remove(building);
                    }
                }
            }

            dbContext.SaveChanges();
            return new ResultInfo(true, "删除成功", null);
        }

        public ResultInfo GetCoreInfos() {
            var result = dbContext.Set<Building>().Select(b => new {
                b.Id, b.Name, b.FloorNum
            }).ToList();

            return new ResultInfo(true, "", result);
        }
    }
}