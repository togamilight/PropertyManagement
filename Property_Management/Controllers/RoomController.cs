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

        //[JsonExceptionFilter]
        //public ActionResult AddRoom(Room room) {
        //    return Json(buildingService.Add(room));
        //}

        //[JsonExceptionFilter]
        //public ActionResult UpdateRoom(Room room) {
        //    return Json(buildingService.Update(room));
        //}

        //[JsonExceptionFilter]
        //public ActionResult DeleteRooms(int[] ids) {
        //    return Json(buildingService.Delete(ids));
        //}


        [JsonExceptionFilter]
        public ActionResult GetRoomPage(int page = 1, int pageSize = 10, string name = "") {
            var where = PredicateBuilder.True<Room>();

            if (!string.IsNullOrEmpty(name)) {
                where = where.And(b => b.Name.Contains(name));
            }

            return Json(roomService.QueryToPage(where, page, pageSize));
        }
    }
}