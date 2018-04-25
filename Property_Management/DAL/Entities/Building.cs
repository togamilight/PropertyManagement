using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    [Table("building")]
    public class Building: BaseEntity {
        [Display(Name = "楼栋名称"), Required, StringLength(20)]
        public string Name { get; set; }

        [Display(Name = "面积"), Range(0, 1000000)]
        public float Area { get; set; }

        [Display(Name = "建成日期"), Required]
        public DateTime BuildDate { get; set; }

        [Display(Name = "层数"), Range(1, 10000)]
        public int FloorNum { get; set; }
    }
}