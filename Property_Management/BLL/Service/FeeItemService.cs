using Property_Management.BLL.Base;
using Property_Management.BLL.IService;
using Property_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Property_Management.Models;
using System.Linq.Expressions;

namespace Property_Management.BLL.Service {
    public class FeeItemService : BaseService<FeeItem>, IFeeItemService {
        public ResultInfo GetCoreInfos() {
            var data = dbContext.Set<FeeItem>().Select(i => new { i.Id, i.Name, i.Scale });

            return new ResultInfo(true, "", data);
        }

        public override ResultInfo Add(FeeItem feeItem) {
            string msg = ValidateEntity(feeItem);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var feeItems = dbContext.Set<FeeItem>();
            if (feeItems.Any(i => i.Name == feeItem.Name)) {
                return new ResultInfo(false, "该项目名称已存在", null);
            }

            //将换行符转换
            //feeItem.Discription = feeItem.Discription.Replace("\r\n", @"<br/>").Replace("\r", @"<br/>").Replace("\n", @"<br/>");

            feeItems.Add(feeItem);
            dbContext.SaveChanges();

            return new ResultInfo(true, "添加成功", new { feeItem.Id });
        }

        public override ResultInfo Update(FeeItem feeItem) {
            string msg = ValidateEntity(feeItem);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            if (feeItem.Id <= 0) {
                return new ResultInfo(false, "Id错误，修改失败", null);
            }

            if (dbContext.Set<Building>().Any(i => i.Name == feeItem.Name && i.Id != feeItem.Id)) {
                return new ResultInfo(false, "该名称已存在，修改失败", null);
            }

            //将换行符转换
            //feeItem.Discription = feeItem.Discription.Replace("\r\n", @"<br/>").Replace("\r", @"<br/>").Replace("\n", @"<br/>");

            dbContext.Entry<FeeItem>(feeItem).State = System.Data.Entity.EntityState.Modified;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public override ResultInfo Delete(int[] ids) {
            var feeItems = dbContext.Set<FeeItem>();
            var fees = dbContext.Set<Fee>();

            using (var transaction = dbContext.Database.BeginTransaction()) {
                try {
                    foreach (var id in ids) {
                        var feeItem = feeItems.FirstOrDefault(e => e.Id == id);
                        if (feeItem != null) {

                            var sql = "delete from fee where FeeItemId = '"+ feeItem.Id +"';";
                            dbContext.Database.ExecuteSqlCommand(sql);

                            feeItems.Remove(feeItem);
                        }
                    }

                    dbContext.SaveChanges();

                    transaction.Commit();

                    return new ResultInfo(true, "删除成功", null);
                }
                catch (Exception) {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}