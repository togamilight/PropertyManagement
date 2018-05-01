using Property_Management.BLL.Base;
using Property_Management.BLL.IService;
using Property_Management.DAL.Entities;
using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Property_Management.BLL.Service {
    public class OwnerService: BaseService<Owner>, IOwnerService {
        public ResultInfo ChangePassword(Owner owner, string newPassword) {
            if (newPassword == null || newPassword.Length > 20 || newPassword.Length < 6) {
                return new ResultInfo(false, "密码不能大于20位或小于6位", null);
            }

            var oldOwner = dbContext.Set<Owner>().FirstOrDefault(o => o.Name == owner.Name && o.Password == owner.Password && !o.Disuse);
            if (oldOwner == null) {
                return new ResultInfo(false, "原密码错误", null);
            }

            oldOwner.Password = newPassword;
            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public int CheckAccount(Owner owner) {
            return dbContext.Set<Owner>().Where(o => o.Name == owner.Name && o.Password == owner.Password && !o.Disuse).Select(a => a.Id).FirstOrDefault();
        }

        public override ResultInfo QueryToPage(Expression<Func<Owner, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Set<Owner>().Where(whereLambda);
            var total = entities.Count();
            var owners = entities.OrderByDescending(e => e.Id).Skip(skipCount).Take(pageSize);

            var data = from o in owners
                       join r in dbContext.Set<Room>()
                       on o.RoomId equals r.Id into grp1
                       join p in dbContext.Set<Parking>()
                       on o.ParkingId equals p.Id into grp2
                       from g1 in grp1.DefaultIfEmpty()
                       join b in dbContext.Set<Building>()
                       on g1.BuildingId equals b.Id into grp3
                       from g2 in grp2.DefaultIfEmpty()
                       from g3 in grp3.DefaultIfEmpty()
                       select new {
                           Owner = o,
                           RoomName = g1.Name,
                           Floor = g1.Floor,
                           ParkingName = g2.Name,
                           BuildingName = g3.Name,
                           BuildingId = g3.Id
                       };

            return new ResultInfo(true, "", new { Total = total, Data = data.ToList() });
        }

        public ResultInfo QueryDisuseToPage(Expression<Func<Owner, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Set<Owner>().Where(whereLambda);
            var total = entities.Count();
            var owners = entities.OrderByDescending(e => e.Id).Skip(skipCount).Take(pageSize);

            return new ResultInfo(true, "", new { Total = total, Data = owners });
        }

        public override ResultInfo Query(int id) {
            var owner = dbContext.Set<Owner>().Where(o => o.Id == id);
            var data = (from o in owner
                       join r in dbContext.Set<Room>()
                       on o.RoomId equals r.Id into grp1
                       join p in dbContext.Set<Parking>()
                       on o.ParkingId equals p.Id into grp2
                       from g1 in grp1.DefaultIfEmpty()
                       join b in dbContext.Set<Building>()
                       on g1.BuildingId equals b.Id into grp3
                       from g2 in grp2.DefaultIfEmpty()
                       from g3 in grp3.DefaultIfEmpty()
                       select new {
                           Owner = o,
                           RoomName = g1.Name,
                           Floor = (int?)g1.Floor,
                           ParkingName = g2.Name,
                           BuildingName = g3.Name,
                           BuildingId = (int?)g3.Id
                       }).FirstOrDefault();

            return new ResultInfo(true, "", data);
        }

        public override ResultInfo Add(Owner owner) {
            string msg = ValidateEntity(owner);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            using (var transaction = dbContext.Database.BeginTransaction()) {
                try {
                    var owners = dbContext.Set<Owner>();
                    if (owners.Any(o => o.Name == owner.Name)) {
                        return new ResultInfo(false, "该名字已存在，如果是同名住户，请添加一些标识进行区分", null);
                    }

                    var room = dbContext.Set<Room>().FirstOrDefault(r => r.Id == owner.RoomId);
                    if (room == null) {
                        return new ResultInfo(false, "该房子不存在", null);
                    }
                    if (room.OwnerId != null) {
                        return new ResultInfo(false, "该房子已有住户", null);
                    }

                    owner.Disuse = false;
                    owner.ParkingId = null;
                    owners.Add(owner);
                    dbContext.SaveChanges();
                    room.OwnerId = owner.Id;
                    dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception) {
                    transaction.Rollback();
                    throw;
                }
            }

                return new ResultInfo(true, "添加成功", new { owner.Id });
        }

        /// <summary>
        /// 现有住户的更新方法
        /// </summary>
        /// <param name="owner"></param>
        public override ResultInfo Update(Owner owner) {
            string msg = ValidateEntity(owner);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            var owners = dbContext.Set<Owner>();
            var rooms = dbContext.Set<Room>();

            var oldOwner = owners.FirstOrDefault(o => o.Id == owner.Id && !o.Disuse);
            if(oldOwner == null) {
                return new ResultInfo(false, "该住户不存在或已搬走", null);
            }

            if (oldOwner.Name != owner.Name && owners.Any(o => o.Name == owner.Name)) {
                return new ResultInfo(false, "该名字已存在，如果是同名住户，请添加一些标识进行区分", null);
            }

            if(oldOwner.RoomId != owner.RoomId) {
                var newRoom = rooms.FirstOrDefault(r => r.Id == owner.RoomId);

                if(newRoom == null) {
                    return new ResultInfo(false, "该房子不存在", null);
                }

                if (newRoom.OwnerId != null) {
                    return new ResultInfo(false, "该房子已有其它住户", null);
                }

                var oldRoom = rooms.FirstOrDefault(r => r.Id == oldOwner.RoomId);
                oldRoom.OwnerId = null;

                newRoom.OwnerId = owner.Id;
                oldOwner.RoomId = owner.RoomId;
            }

            oldOwner.Name = owner.Name;
            oldOwner.Password = owner.Password;
            oldOwner.Sex = owner.Sex;
            oldOwner.PhoneNum = owner.PhoneNum;
            oldOwner.IDCard = owner.IDCard;
            oldOwner.Date = owner.Date;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public override ResultInfo Delete(int[] ids) {
            var owners = dbContext.Set<Owner>();
            foreach (var id in ids) {
                var owner = owners.FirstOrDefault(e => e.Id == id);
                if (owner != null) {
                    var room = dbContext.Set<Room>().FirstOrDefault(r => r.Id == owner.RoomId);
                    room.OwnerId = null;

                    var parking = dbContext.Set<Parking>().FirstOrDefault(r => r.Id == owner.ParkingId);
                    parking.OwnerId = null;
                    parking.CarNum = null;
                    parking.CarType = null;
                    parking.Date = null;

                    //TODO 删除收费和维修记录
                    var fees = dbContext.Set<Fee>().Where(f => f.OwnerId == owner.Id);
                    dbContext.Set<Fee>().RemoveRange(fees);

                    var repairs = dbContext.Set<Repair>().Where(r => r.OwnerId == owner.Id);
                    dbContext.Set<Repair>().RemoveRange(repairs);

                    owners.Remove(owner);
                }
            }

            dbContext.SaveChanges();

            return new ResultInfo(true, "删除成功", null);
        }

        public ResultInfo Disuse(int[] ids) {
            var owners = dbContext.Set<Owner>();
            foreach (var id in ids) {
                var owner = owners.FirstOrDefault(e => e.Id == id && !e.Disuse);
                if (owner != null) {
                    var room = dbContext.Set<Room>().FirstOrDefault(r => r.Id == owner.RoomId);
                    room.OwnerId = null;
                    owner.RoomId = null;

                    if(owner.ParkingId != null) {
                        var parking = dbContext.Set<Parking>().FirstOrDefault(r => r.Id == owner.ParkingId);
                        parking.OwnerId = null;
                        parking.CarNum = null;
                        parking.CarType = null;
                        parking.Date = null;

                        owner.ParkingId = null;
                    }

                    //TODO disuse收费和维修记录
                    var fees = dbContext.Set<Fee>().Where(f => f.OwnerId == owner.Id);
                    foreach(var fee in fees) {
                        fee.Disuse = true;
                    }

                    var repairs = dbContext.Set<Repair>().Where(r => r.OwnerId == owner.Id);
                    foreach(var repair in repairs) {
                        repair.Disuse = true;
                    }


                    owner.Disuse = true;
                    owner.DisuseDate = DateTime.Now.Date;
                }
            }

            dbContext.SaveChanges();

            return new ResultInfo(true, "搬走成功", null);
        }

        public ResultInfo GetCoreInfo(Expression<Func<Owner, bool>> whereLambda) {
            var data = dbContext.Set<Owner>().Where(whereLambda).Select(o => new { o.Id, o.Name });

            return new ResultInfo(true, "", data);
        }

        public ResultInfo Recover(Owner owner) {
            var owners = dbContext.Set<Owner>();
            var rooms = dbContext.Set<Room>();

            var oldOwner = owners.FirstOrDefault(o => o.Id == owner.Id && o.Disuse);
            if (oldOwner == null) {
                return new ResultInfo(false, "该住户不存在", null);
            }

            var newRoom = rooms.FirstOrDefault(r => r.Id == owner.RoomId);

            if (newRoom == null) {
                return new ResultInfo(false, "该房子不存在", null);
            }

            if (newRoom.OwnerId != null) {
                return new ResultInfo(false, "该房子已有其它住户", null);
            }

            newRoom.OwnerId = owner.Id;
            oldOwner.RoomId = owner.RoomId;

            oldOwner.Date = owner.Date;
            oldOwner.Disuse = false;
            oldOwner.DisuseDate = null;

            dbContext.SaveChanges();

            return new ResultInfo(true, "恢复成功", null);
        }

        public ResultInfo GetBaseInfoForOwner(int ownerId) {
            var owner = dbContext.Set<Owner>().Where(o => o.Id == ownerId && !o.Disuse);

            if (!owner.Any()) {
                return new ResultInfo(false, "该住户不存在", null);
            }

            var data = (from o in owner
                        join r in dbContext.Set<Room>()
                        on o.RoomId equals r.Id into grp1
                        join p in dbContext.Set<Parking>()
                        on o.ParkingId equals p.Id into grp2
                        from g1 in grp1.DefaultIfEmpty()
                        join b in dbContext.Set<Building>()
                        on g1.BuildingId equals b.Id into grp3
                        from g2 in grp2.DefaultIfEmpty()
                        from g3 in grp3.DefaultIfEmpty()
                        select new OwnerBaseInfo() {
                            OwnerName = o.Name,
                            Sex = o.Sex,
                            PhoneNum = o.PhoneNum,
                            IDCard = o.IDCard,
                            OwnerDate = o.Date,
                            RoomName = g1.Name,
                            RoomFloor = g1.Floor,
                            RoomArea = g1.Area,
                            RoomType = g1.Type,
                            BuildingName = g3.Name,
                            ParkingName = g2.Name,
                            ParkingDate = g2.Date,
                            CarNum = g2.CarNum,
                            CarType = g2.CarType
                        }).FirstOrDefault();

            return new ResultInfo(true, "", data);
        }
    }
}