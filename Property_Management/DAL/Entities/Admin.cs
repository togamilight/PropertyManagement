using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    [Table("admin")]
    public class Admin : BaseEntity{
        [Display(Name = "用户名"), Required, StringLength(20)]
        public string Name { get; set; }

        [Display(Name = "密码"), Required, StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
    }
}