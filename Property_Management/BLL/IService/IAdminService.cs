using Property_Management.BLL.Base;
using Property_Management.DAL.Entities;
using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Property_Management.BLL.IService {
    public interface IAdminService : IBaseService<Admin> {
        /// <summary>
        /// 确认账号密码的准确性
        /// </summary>
        int CheckAccount(Admin admin);

        /// <summary>
        /// 修改密码，需要传入帐户名和原密码
        /// </summary>
        ResultInfo ChangePassword(Admin admin, string newPassword);
    }
}
