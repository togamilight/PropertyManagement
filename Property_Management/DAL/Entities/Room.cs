using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    [Table("room")]
    public class Room : BaseEntity{
        public int BuildingId { get; set; }

        [Display(Name = "所在楼层"), Range(1, 10000)]
        public int Floor { get; set; }

        [Display(Name = "房号"), Required, StringLength(20)]
        public string Name { get; set; }

        [Display(Name = "面积"), Range(0, 1000000)]
        public float Area { get; set; }

        public int? OwnerId { get; set; }
    }
}