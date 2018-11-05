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
        /// 楼栋表
        /// </summary>
        public DbSet<Building> Buildings { get; set; }

        /// <summary>
        /// 房屋表
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

        /// <summary>
        /// 收费项目
        /// </summary>
        public DbSet<FeeItem> FeeItems { get; set; }

        /// <summary>
        /// 收费记录
        /// </summary>
        public DbSet<Fee> Fees { get; set; }

        /// <summary>
        /// 维修记录
        /// </summary>
        public DbSet<Repair> Repairs { get; set; }

        /// <summary>
        /// 公告
        /// </summary>
        public DbSet<Announcement> Announcements { get; set; }

        /// <summary>
        /// 建议
        /// </summary>
        public DbSet<Advice> Advices { get; set; }

        /// <summary>
        /// 回复
        /// </summary>
        public DbSet<Reply> Replies { get; set; }
    }
}