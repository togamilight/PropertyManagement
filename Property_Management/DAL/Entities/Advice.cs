using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    public class Advice : BaseEntity{
        [Display(Name = "标题")Required, StringLength(30)]
        public string Title { get; set; }

        [Display(Name = "内容"), Required, StringLength(500)]
        public string Content { get; set; }

        public DateTime DateTime { get; set; }

        public int OwnerId { get; set; }

        public int ReplyNum { get; set; }

        public int NewReplyNum { get; set; }

        public bool Disuse { get; set; }
    }
}