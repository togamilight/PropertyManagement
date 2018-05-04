using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    public class Reply : BaseEntity{
        [Display(Name = "内容"), Required, StringLength(200)]
        public string Content { get; set; }

        public DateTime DateTime { get; set; }

        public int? AdminId { get; set; }

        public int AdviceId { get; set; }
    }
}