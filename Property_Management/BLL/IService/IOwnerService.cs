using Property_Management.BLL.Base;
using Property_Management.DAL.Entities;
using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Property_Management.BLL.IService {
    public interface IOwnerService : IBaseService<Owner>{
        /// <summary>
        /// 确认账号密码的准确性
        /// </summary>
        int CheckAccount(Owner owner);

        /// <summary>
        /// 修改密码，需要传入帐户名和原密码
        /// </summary>
        ResultInfo ChangePassword(Owner admin, string newPassword);

        /// <summary>
        /// 搬走住户
        /// </summary>
        ResultInfo Disuse(int[] ids);

        /// <summary>
        /// 获取所有现有住户的姓名和id
        /// </summary>
        /// <returns></returns>
        ResultInfo GetCoreInfo(Expression<Func<Owner, bool>> whereLambda);

        /// <summary>
        /// 分页查询搬走住户
        /// </summary>
        ResultInfo QueryDisuseToPage(Expression<Func<Owner, bool>> whereLambda, int page, int pageSize);

        /// <summary>
        /// 恢复搬走住户
        /// </summary>
        ResultInfo Recover(Owner owner);

        ResultInfo GetBaseInfoForOwner(int ownerId);

        DateTime UpdateLastLookTime(int id);
    }
}