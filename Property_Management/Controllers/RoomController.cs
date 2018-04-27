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
    public class RoomController : Controller
    {
        private IRoomService roomService = new RoomService();
        // GET: Room
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RoomManage() {
            return View();
        }

        [JsonExceptionFilter]
        public ActionResult AddRoom(Room room) {
            return Json(roomService.Add(room));
        }

        [JsonExceptionFilter]
        public ActionResult UpdateRoom(Room room) {
            return Json(roomService.Update(room));
        }

        [JsonExceptionFilter]
        public ActionResult DeleteRooms(int[] ids) {
            return Json(roomService.Delete(ids));
        }


        [JsonExceptionFilter]
        public ActionResult GetRoomPage(int page = 1, int pageSize = 10, string name = "", string type = "", int buildingId = 0) {
            var where = PredicateBuilder.True<Room>();

            if (!string.IsNullOrEmpty(name)) {
                where = where.And(r => r.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(type)) {
                where = where.And(r => r.Type.Contains(type));
            }

            if(buildingId > 0) {
                where = where.And(r => r.BuildingId == buildingId);
            }

            return Json(roomService.QueryToPage(where, page, pageSize));
        }

        [JsonExceptionFilter]
        public ActionResult GetEmptyRoomInBuilding(int buildingId) {
            return Json(roomService.GetEmptyRoomInBuilding(buildingId));
        }

        [JsonExceptionFilter]
        public ActionResult GetRoomInfo(int id) {
            return Json(roomService.Query(id));
        }
    }
}