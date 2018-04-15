using Property_Management.DAL;
using Property_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property_Management.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAdmin() {
            using (var dbContext = new MyDbContext()) {
                return Json(dbContext.Admins.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TestDbSet() {
            using (var dbContext = new MyDbContext()) {
                string hashCode1 = "HashCode1: " + dbContext.Admins.GetHashCode() + "\n";
                string hashCode2 = "HashCode2(.Set<>()): " + dbContext.Set<Admin>().GetHashCode();

                return Content(hashCode1 + hashCode2);
            }
        }
    }
}