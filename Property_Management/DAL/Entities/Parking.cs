using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    [Table("parking")]
    public class Parking: BaseEntity{
        [Display(Name = "车位号"), Required, StringLength(20)]
        public string Name { get; set; }

        [Display(Name = "分配日期")]
        public DateTime? Date { get; set; }

        [Display(Name = "车型")]
        public string CarType { get; set; }

        [Display(Name = "车牌号")]
        public string CarNum { get; set; }

        public int? OwnerId { get; set; }
    }
}