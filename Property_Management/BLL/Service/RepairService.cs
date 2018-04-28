using Property_Management.BLL.Base;
using Property_Management.BLL.IService;
using Property_Management.DAL.Entities;
using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Property_Management.BLL.Service {
    public class RepairService : BaseService<Repair>, IRepairService {
        public override ResultInfo Add(Repair repair) {
            string msg = ValidateEntity(repair);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            if (!dbContext.Set<Owner>().Any(o => o.Id == repair.OwnerId && !o.Disuse)) {
                return new ResultInfo(false, "该住户不存在或已搬走", null);
            }

            repair.FinishDate = null;
            repair.Money = 0;
            repair.Staff = null;
            repair.Disuse = false;

            dbContext.Set<Repair>().Add(repair);
            dbContext.SaveChanges();

            return new ResultInfo(true, "添加成功", new { repair.Id });
        }

        public override ResultInfo Update(Repair repair) {
            string msg = ValidateEntity(repair);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }


            var oldRepair = dbContext.Set<Repair>().FirstOrDefault(f => f.Id == repair.Id);

            if (oldRepair == null) {
                return new ResultInfo(false, "该记录不存在", null);
            }

            if (oldRepair.Disuse) {
                return new ResultInfo(false, "无法修改已搬走住户的记录", null);
            }

            if (oldRepair.OwnerId != repair.OwnerId) {
                if (!dbContext.Set<Owner>().Any(o => o.Id == repair.OwnerId && !o.Disuse)) {
                    return new ResultInfo(false, "该住户不存在或已搬走", null);
                }
                oldRepair.OwnerId = repair.OwnerId;
            }

            oldRepair.Name = repair.Name;
            oldRepair.Discription = repair.Discription;
            oldRepair.ApplyDate = repair.ApplyDate;
            oldRepair.FinishDate = repair.FinishDate;
            if (repair.FinishDate == null) {
                repair.Staff = null;
                repair.Money = 0;
            }
            oldRepair.Staff = repair.Staff;
            oldRepair.Money = repair.Money;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public ResultInfo UpdateByOwner(Repair repair) {
            string msg = ValidateEntity(repair);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }


            var oldRepair = dbContext.Set<Repair>().FirstOrDefault(f => f.Id == repair.Id);

            if (oldRepair == null) {
                return new ResultInfo(false, "该记录不存在", null);
            }

            if (oldRepair.Disuse) {
                return new ResultInfo(false, "无法修改已搬走住户的记录", null);
            }

            if(oldRepair.OwnerId != repair.OwnerId) {
                return new ResultInfo(false, "无法修改不属于你的记录", null);
            }

            if(oldRepair.FinishDate != null) {
                return new ResultInfo(false, "无法修改已完成维修的记录", null);
            }

            oldRepair.Name = repair.Name;
            oldRepair.Discription = repair.Discription;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public override ResultInfo QueryToPage(Expression<Func<Repair, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Set<Repair>().Where(whereLambda);
            var total = entities.Count();
            var repairs = entities.OrderByDescending(e => e.Id).Skip(skipCount).Take(pageSize).ToList();

            var data = from r in repairs
                       join o in dbContext.Set<Owner>()
                       on r.OwnerId equals o.Id into grp1
                       from g1 in grp1.DefaultIfEmpty()
                       select new {
                           Repair = r,
                           OwnerName = g1.Name
                       };

            return new ResultInfo(true, "", new { Total = total, Data = data });
        }

        public ResultInfo QueryToPageByOwner(Expression<Func<Repair, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Set<Repair>().Where(whereLambda);
            var total = entities.Count();
            var repairs = entities.OrderByDescending(e => e.Id).Skip(skipCount).Take(pageSize).ToList();

            return new ResultInfo(true, "", new { Total = total, Data = repairs });
        }

        public ResultInfo DeleteByOwner(int[] ids, int ownerId) {
            var dbSet = dbContext.Set<Repair>();
            foreach (var id in ids) {
                var t = dbSet.FirstOrDefault(e => e.Id == id && e.OwnerId == ownerId && e.FinishDate == null);
                if (t != null) {
                    dbSet.Remove(t);
                }
            }

            dbContext.SaveChanges();

            return new ResultInfo(true, "删除成功", null);
        }

        public override string ValidateEntity(Repair repair) {
            var validResults = new List<ValidationResult>();
            string msg = "";
            if (!Validator.TryValidateObject(repair, new ValidationContext(repair), validResults, true)) {
                foreach (var result in validResults) {
                    msg += result.ErrorMessage + "\n";
                }
            }
            else {
                if (repair.OwnerId <= 0) {
                    msg += "住户编号错误 \n";
                }
                if(repair.FinishDate != null) {
                    if (string.IsNullOrEmpty(repair.Staff)) {
                        msg += "维修人员不能为空 \n";
                    }
                }
            }

            return msg;
        }
    }
}