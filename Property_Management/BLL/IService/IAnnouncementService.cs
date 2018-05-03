﻿using Property_Management.BLL.Base;
using Property_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Property_Management.BLL.IService {
    public interface IAnnouncementService : IBaseService<Announcement> {
        int GetUnReadCountForOwner(int id);
    }
}
