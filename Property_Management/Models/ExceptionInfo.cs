using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Property_Management.Models {
    public class ExceptionInfo {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public Exception Exception { get; set; }
        public DateTime DateTime { get; set; }
    }
}