using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    [Table("announcement")]
    public class Announcement : BaseEntity {
        [Display(Name = "标题")Required, StringLength(30)]
        public string Title { get; set; }

        [Display(Name = "内容"), Required, StringLength(500)]
        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}