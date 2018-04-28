using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    [Table("feeitem")]
    public class FeeItem : BaseEntity{
        [Display(Name = "项目名称"), Required, StringLength(20)]
        public string Name { get; set; }

        [Display(Name = "收费标准"), Required, StringLength(20)]
        public string Scale { get; set; }

        [Display(Name = "描述"), StringLength(200)]
        public string Discription { get; set; }
    }
}