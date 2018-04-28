using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    [Table("fee")]
    public class Fee : BaseEntity{
        public int FeeItemId { get; set; }

        public int OwnerId { get; set; }

        public float Money { get; set; }

        public DateTime? FinishDate { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "备注"), StringLength(50)]
        public string Remark { get; set; }

        public bool Disuse { get; set; }

    }
}