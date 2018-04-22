using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Property_Management.Models;
using Property_Management.DAL;
using Property_Management.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Property_Management.BLL.Base {
    public class BaseService<T> : IBaseService<T> where T : BaseEntity, new(){

        protected MyDbContext dbContext;

        public BaseService() {
            dbContext = MyDbContextFactory.GetMyDbContext();
        }

        public virtual ResultInfo Add(T t) {
            string msg = ValidateEntity(t);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            dbContext.Set<T>().Add(t);
            dbContext.SaveChanges();

            return new ResultInfo(true, "添加成功", new { t.Id });
        }

        public virtual ResultInfo Delete(int id) {
            var dbSet = dbContext.Set<T>();
            var t = dbSet.FirstOrDefault(e => e.Id == id);

            if(t == null) {
                return new ResultInfo(false, "不存在该Id的记录", null);
            }

            dbSet.Remove(t);
            dbContext.SaveChanges();

            return new ResultInfo(true, "删除成功", null);
        }

        public virtual ResultInfo Delete(T t) {
            return Delete(t.Id);
        }

        public virtual ResultInfo Delete(int[] ids) {
            var dbSet = dbContext.Set<T>();
            foreach(var id in ids) {
                var t = dbSet.FirstOrDefault(e => e.Id == id);
                if(t != null) {
                    dbSet.Remove(t);
                }
            }

            dbContext.SaveChanges();

            return new ResultInfo(true, "删除成功", null);
        }

        public virtual ResultInfo Query(int id) {
            var t = dbContext.Set<T>().FirstOrDefault(e => e.Id == id);
            if(t == null) {
                return new ResultInfo(false, "不存在该Id的记录", null);
            }
            return new ResultInfo(true, "", t);
        }

        public virtual ResultInfo Query(Expression<Func<T, bool>> whereLambda) {
            var list = dbContext.Set<T>().Where(whereLambda).OrderByDescending(e => e.Id).ToList();

            return new ResultInfo(true, "", list);
        }

        public virtual ResultInfo QueryToPage(Expression<Func<T, bool>> whereLambda, int page, int pageSize) {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;
            var skipCount = pageSize * (page - 1);

            var entities = dbContext.Set<T>().Where(whereLambda);
            var total = entities.Count();
            var data = entities.OrderByDescending(e => e.Id).Skip(skipCount).Take(pageSize).ToList();

            return new ResultInfo(true, "", new { Total = total, Data = data});
        }

        public virtual ResultInfo Update(T t) {
            string msg = ValidateEntity(t);
            if (!string.IsNullOrEmpty(msg)) {
                return new ResultInfo(false, msg, null);
            }

            dbContext.Entry<T>(t).State = System.Data.Entity.EntityState.Modified;

            dbContext.SaveChanges();

            return new ResultInfo(true, "修改成功", null);
        }

        public virtual string ValidateEntity(T t) {
            var validResults = new List<ValidationResult>();
            string msg = "";
            if (!Validator.TryValidateObject(t, new ValidationContext(t), validResults, true)) {
                foreach (var result in validResults) {
                    msg += result.ErrorMessage + "\n";
                }
            }

            return msg;
        }
    }
}