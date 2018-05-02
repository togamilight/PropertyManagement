using Property_Management.BLL.Base;
using Property_Management.DAL.Entities;
using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Property_Management.BLL.IService {
    public interface IFeeService : IBaseService<Fee> {
        ResultInfo QueryToPageByOwner(Expression<Func<Fee, bool>> whereLambda, int page, int pageSize);

        int GetUnFinishCountForOwner(int ownerId);

        ResultInfo GetBarData();
        ResultInfo GetPieData(string beginDate, string endDate);
    }
}
