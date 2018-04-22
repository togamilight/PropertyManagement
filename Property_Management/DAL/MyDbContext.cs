using Property_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Property_Management.DAL {
    public class MyDbContext : DbContext{
        public MyDbContext() : base("name=MySqlDbConnStr") { }


        /// <summary>
        /// 管理员表
        /// </summary>
        public DbSet<Admin> Admins { get; set; }

        /// <summary>
        /// 楼盘表
        /// </summary>
        public DbSet<Building> Buildings { get; set; }

        /// <summary>
        /// 房子表
        /// </summary>
        public DbSet<Room> Rooms { get; set; }
    }
}