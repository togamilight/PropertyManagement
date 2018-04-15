using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Property_Management.Models {
    public class ResultInfo {

        public ResultInfo(bool success, string msg, object result) {
            Success = success;
            Msg = msg;
            Result = result;
        }

        public ResultInfo() {
            Success = false;
            Msg = "";
            Result = null;
        }

        public bool Success { get; set; }
        public string Msg { get; set; }
        public object Result { get; set; }
    }
}