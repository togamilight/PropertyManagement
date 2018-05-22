using Property_Management.BLL.Base;
using Property_Management.BLL.IService;
using Property_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Property_Management.Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Property_Management.BLL.Service {
    public class ParkingService : BaseService<Parking>, IParkingService {
        public override ResultInfo Add(Parking parking) {
            //添加时不设置业主
            parking.OwnerId = null;
            parking.CarNum = null;
            parking.CarType = null;
            parking.Date = null;

            string msg = ValidateEntity(parking);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var parkings = dbContext.Set<Parking>();

            if(parkings.Any(p => p.Name == parking.Name)) {
                return new ResultInfo(false, "该车位号已存在", null);
            }

            parkings.Add(parking);
            dbContext.SaveChanges();

            return new ResultInfo(true, "添加成功", new { parking.Id });
        }

        public override ResultInfo Update(Parking parking) {
            string msg = ValidateEntity(parking);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var parkings = dbContext.Set<Parking>();
            var owners = dbContext.Set<Owner>();
            var oldParking = parkings.FirstOrDefault(p => p.Id == parking.Id);

            if(oldParking == null) {
                return new ResultInfo(false, "该车位不存在", null);
            }

            if (oldParking.Name != parking.Name) {
                if (parkings.Any(p => p.Name == parking.Name)) {
                    return new ResultInfo(false, "该车位号已存在", null);
                }

                oldParking.Name = parking.Name;
            }

            if (oldParking.OwnerId != parking.OwnerId) {
                if(oldParking.OwnerId != null) {
                    var oldOwner = owners.FirstOrDefault(o => o.Id == oldParking.OwnerId);
                    oldOwner.ParkingId = null;
                }

                if(parking.OwnerId != null) {
                    var newOwner = owners.FirstOrDefault(o => o.Id == parking.OwnerId && !o.Disuse);
                    if(newOwner == null) {
                        return new ResultInfo(false, "该业主不存在或已搬走", null);
                    }

                    if(newOwner.ParkingId != null) {
                        var oldParking2 = parkings.FirstOrDefault(p => p.Id == newOwner.ParkingId);
                        oldParking2.OwnerId = null;
                        oldParking2.Date = null;
                        oldParking2.CarNum = null;
                        oldParking2.CarType = null;
                    }
                    newOwner.ParkingId = parking.Id;
                    oldParking.CarNum = parking.CarNum;
                    oldParking.CarType = parking.CarType;
                    oldParking.Date = parking.Date;
                }else {
                    oldParking.CarNum = null;
                    oldParking.CarType = null;
                    oldParking.Date = null;
                }

                oldParking.OwnerId = parking.OwnerId;
            }

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public override ResultInfo Delete(int[] ids) {
            var parkings = dbContext.Set<Parking>();
            foreach (var id in ids) {
                var parking = parkings.FirstOrDefault(p => p.Id == id);
                if (parking != null) {
                    if(parking.OwnerId != null) {
                        var owner = dbContext.Set<Owner>().FirstOrDefault(o => o.Id == parking.OwnerId);
                        owner.ParkingId = null;
                    }
                    parkings.Remove(parking);
                }
            }

            dbContext.SaveChanges();

            return new ResultInfo(true, "删除成功", null);
        }

        public override ResultInfo QueryToPage(Expression<Func<Parking, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Set<Parking>().Where(whereLambda);
            var total = entities.Count();
            var parkings = entities.OrderByDescending(e => e.Id).Skip(skipCount).Take(pageSize);

            var data = (from p in parkings
                       join o in dbContext.Set<Owner>()
                       on p.OwnerId equals o.Id into grp1
                       from g in grp1.DefaultIfEmpty()
                       select new {
                           Parking = p,
                           OwnerName = g.Name
                       }).OrderByDescending(e => e.Parking.Id);

            return new ResultInfo(true, "", new { Total = total, Data = data });
        }

        public override string ValidateEntity(Parking parking) {
            if(parking.OwnerId <= 0) {
                parking.OwnerId = null;
            }
            var validResults = new List<ValidationResult>();
            string msg = "";
            if (!Validator.TryValidateObject(parking, new ValidationContext(parking), validResults, true)) {
                foreach (var result in validResults) {
                    msg += result.ErrorMessage + "\n";
                }
            }else if(parking.OwnerId != null) {
                if(parking.Date == null || string.IsNullOrEmpty(parking.CarNum) || string.IsNullOrEmpty(parking.CarType)) {
                    msg += "分配日期，车型和车牌号都不能为空\n";
                }else if(parking.CarType.Length > 20) {
                    msg += "车型不能超过20位";
                }
                else {
                    var regex = new Regex(@"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9挂学警港澳]{1}$");
                    if (!regex.IsMatch(parking.CarNum)) {
                        msg += "车牌号的格式不正确\n";
                    }
                }
            }
            return msg;
        }
    }
}