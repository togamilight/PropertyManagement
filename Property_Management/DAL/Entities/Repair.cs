using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    [Table("repair")]
    public class Repair : BaseEntity{
        public int OwnerId { get; set; }

        [Display(Name = "维修项目"), Required, StringLength(20)]
        public string Name { get; set; }

        [Display(Name = "说明"), Required, StringLength(200)]
        public string Discription { get; set; }

        public DateTime ApplyDate { get; set; }

        public DateTime? FinishDate { get; set; }

        [Display(Name = "维修人员"), StringLength(20)]
        public string Staff { get; set; }

        public bool Disuse { get; set; }

        [Display(Name = "费用"), Range(0, float.MaxValue, ErrorMessage = "费用不能为负")]
        public float Money { get; set; }
    }
}