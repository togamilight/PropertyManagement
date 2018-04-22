using Property_Management.DAL.Entities;
using Property_Management.Models;
using System;
using System.Linq.Expressions;

namespace Property_Management.BLL.Base {
    public interface IBaseService<T> where T: BaseEntity, new() {
        ResultInfo Add(T t);
        ResultInfo Update(T t);
        ResultInfo Delete(T t);
        ResultInfo Delete(int id);
        ResultInfo Delete(int[] ids);
        ResultInfo Query(int id);
        ResultInfo Query(Expression<Func<T, bool>> whereLambda);
        ResultInfo QueryToPage(Expression<Func<T, bool>> whereLambda, int page, int pageSize);
        string ValidateEntity(T t);
    }
}
