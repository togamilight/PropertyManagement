using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    public class Fee : BaseEntity{
        public int FeeItemId { get; set; }

        public int OwnerId { get; set; }

        public float Money { get; set; }

        public bool? FinishDate { get; set; }

        public DateTime Date { get; set; }

        public bool Disuse { get; set; }

    }
}