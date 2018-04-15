using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Property_Management.Models {
    /// <summary>
    /// 登录用户的信息
    /// </summary>
    [Serializable]
    public class AccountInfo {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}