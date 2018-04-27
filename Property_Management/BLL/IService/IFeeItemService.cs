using Property_Management.BLL.Base;
using Property_Management.DAL.Entities;
using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Property_Management.BLL.IService {
    public interface IFeeItemService : IBaseService<FeeItem> {
        ResultInfo GetCoreInfos();
    }
}
