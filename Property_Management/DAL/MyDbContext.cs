﻿using Property_Management.DAL.Entities;
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
        /// 楼栋表
        /// </summary>
        public DbSet<Building> Buildings { get; set; }

        /// <summary>
        /// 房子表
        /// </summary>
        public DbSet<Room> Rooms { get; set; }
    
        /// <summary>
        /// 业主表
        /// </summary>
        public DbSet<Owner> Owners { get; set; }

        /// <summary>
        /// 车位表
        /// </summary>
        public DbSet<Parking> Parkings { get; set; }
    }
}