﻿using Property_Management.BLL.Base;
using Property_Management.BLL.IService;
using Property_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Property_Management.Models;

namespace Property_Management.BLL.Service {
    public class AdminService : BaseService<Admin>, IAdminService {
        public ResultInfo ChangePassword(Admin admin, string newPassword) {
            if (newPassword == null || newPassword.Length > 20 || newPassword.Length < 6) {
                return new ResultInfo(false, "密码不能大于20位或小于6位", null);
            }

            int id = CheckAccount(admin);
            if (id == 0) {
                return new ResultInfo(false, "原密码错误", null);
            }

            admin.Id = id;
            admin.Password = newPassword;
           
            return Update(admin);
        }

        public int CheckAccount(Admin admin) {
            if (ValidateEntity(admin) != "")
                return 0;
            return dbContext.Set<Admin>().Where(a => a.Name == admin.Name && a.Password == admin.Password).Select(a => a.Id).FirstOrDefault();
        }

        public override ResultInfo Add(Admin admin) {
            string msg = ValidateEntity(admin);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            if(dbContext.Admins.Any(a => a.Name == admin.Name)) {
                return new ResultInfo(false, "该用户名已存在", null);
            }

            dbContext.Admins.Add(admin);
            dbContext.SaveChanges();

            return new ResultInfo(true, "注册成功", new { admin.Id });
        }
    }
}