using Property_Management.BLL.Base;
using Property_Management.BLL.IService;
using Property_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Property_Management.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Property_Management.BLL.Service {
    public class FeeService : BaseService<Fee>, IFeeService {
        public override ResultInfo Add(Fee fee) {
            string msg = ValidateEntity(fee);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            if(!dbContext.Set<Owner>().Any(o => o.Id == fee.OwnerId && !o.Disuse)) {
                return new ResultInfo(false, "该业主不存在或已搬走", null);
            }

            fee.FinishDate = null;
            fee.Disuse = false;

            dbContext.Set<Fee>().Add(fee);
            dbContext.SaveChanges();

            return new ResultInfo(true, "添加成功", new { fee.Id });
        }

        public override ResultInfo Update(Fee fee) {
            string msg = ValidateEntity(fee);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }


            var oldFee = dbContext.Set<Fee>().FirstOrDefault(f => f.Id == fee.Id);

            if(oldFee == null) {
                return new ResultInfo(false, "该记录不存在", null);
            }

            if (oldFee.Disuse) {
                return new ResultInfo(false, "无法修改已搬走业主的记录", null);
            }

            if(oldFee.OwnerId != fee.OwnerId) {
                if(!dbContext.Set<Owner>().Any(o => o.Id == fee.OwnerId && !o.Disuse)) {
                    return new ResultInfo(false, "该业主不存在或已搬走", null);
                }
                oldFee.OwnerId = fee.OwnerId;
            }

            oldFee.Money = fee.Money;
            oldFee.Date = fee.Date;
            oldFee.FeeItemId = fee.FeeItemId;
            oldFee.FinishDate = fee.FinishDate;
            oldFee.Remark = fee.Remark;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public override ResultInfo QueryToPage(Expression<Func<Fee, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Set<Fee>().Where(whereLambda);
            var total = entities.Count();
            var fees = entities.OrderByDescending(e => e.Date).Skip(skipCount).Take(pageSize).ToList();

            var data = from f in fees
                       join i in dbContext.Set<FeeItem>()
                       on f.FeeItemId equals i.Id into grp1
                       join o in dbContext.Set<Owner>()
                       on f.OwnerId equals o.Id into grp2
                       from g1 in grp1.DefaultIfEmpty()
                       from g2 in grp2.DefaultIfEmpty()
                       select new {
                           Fee = f,
                           FeeItemName = g1.Name,
                           Scale = g1.Scale,
                           OwnerName = g2.Name
                       };

            return new ResultInfo(true, "", new { Total = total, Data = data.OrderByDescending(e => e.Fee.Date) });
        }

        public ResultInfo QueryToPageByOwner(Expression<Func<Fee, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Set<Fee>().Where(whereLambda);
            var total = entities.Count();
            var fees = entities.OrderByDescending(e => e.Date).Skip(skipCount).Take(pageSize).ToList();

            var data = from f in fees
                       join i in dbContext.Set<FeeItem>()
                       on f.FeeItemId equals i.Id into grp1
                       from g1 in grp1.DefaultIfEmpty()
                       select new {
                           Fee = f,
                           FeeItemName = g1.Name,
                           Scale = g1.Scale,
                       };

            return new ResultInfo(true, "", new { Total = total, Data = data.OrderByDescending(e => e.Fee.Date) });
        }

        public override string ValidateEntity(Fee fee) {
            var validResults = new List<ValidationResult>();
            string msg = "";
            if (!Validator.TryValidateObject(fee, new ValidationContext(fee), validResults, true)) {
                foreach (var result in validResults) {
                    msg += result.ErrorMessage + "\n";
                }
            }else {
                if(fee.OwnerId <= 0) {
                    msg += "业主编号错误 \n";
                }
                if(fee.FeeItemId <= 0) {
                    msg += "收费项目编号错误 \n";
                }
                if(fee.Money <= 0) {
                    msg += "费用必须大于0 \n";
                }
            }

            return msg;
        }

        public int GetUnFinishCountForOwner(int ownerId) {
            return dbContext.Set<Fee>().Where(f => f.OwnerId == ownerId && f.FinishDate == null).Count();
        }

        public ResultInfo GetBarData() {
            var sql = "select Date_Format(Date, '%Y-%m') as Month, count(*) as Count, sum(Money) as MoneySum from fee group by Month order by Month; ";

            var data = dbContext.Database.SqlQuery<FeeBarData>(sql).ToList();

            return new ResultInfo(true, "", data);
        }

        public ResultInfo GetPieData(string beginDate, string endDate) {
            var strBeginDate = "";
            var strEndDate = "";
            
            if (!string.IsNullOrEmpty(beginDate)) {
                DateTime begin;
                if (DateTime.TryParse(beginDate, out begin)) {
                    strBeginDate += "and Date_Format(Date, '%Y-%m') >= '" + begin.ToString("yyyy-MM") + "' ";
                }
            }

            if (!string.IsNullOrEmpty(endDate)) {
                DateTime end;
                if (DateTime.TryParse(endDate, out end)) {
                    strEndDate += "and Date_Format(Date, '%Y-%m') <= '" + end.ToString("yyyy-MM") + "' ";
                }
            }

            var sql = "select feeitem.Name as FeeItemName, count(*) as Count, sum(Money) as MoneySum from fee join feeitem on fee.FeeItemId = feeitem.Id where 1 = 1 " + strBeginDate + strEndDate + "group by FeeItemId; ";

            var data = dbContext.Database.SqlQuery<FeePieData>(sql).ToList();

            return new ResultInfo(true, "", data);
        }
    }
}