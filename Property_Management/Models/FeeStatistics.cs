using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Property_Management.Models {
    public class FeeStatistics {
        public string Month { get; set; }
        public string FeeItemName { get; set; }
        public int Count { get; set; }
        public float MoneySum { get; set; }
    }
}