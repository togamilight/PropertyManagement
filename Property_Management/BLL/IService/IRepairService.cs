using Property_Management.BLL.Base;
using Property_Management.DAL.Entities;
using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Property_Management.BLL.IService {
    public interface IRepairService : IBaseService<Repair> {
        ResultInfo UpdateByOwner(Repair repair);
        ResultInfo QueryToPageByOwner(Expression<Func<Repair, bool>> whereLambda, int page, int pageSize);
        ResultInfo DeleteByOwner(int[] ids, int ownerId);
    }
}