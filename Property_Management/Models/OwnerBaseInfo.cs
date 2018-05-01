using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Property_Management.Models {
    public class OwnerBaseInfo {
        public string OwnerName { get; set; }
        public bool Sex { get; set; }
        public string PhoneNum { get; set; }
        public string IDCard { get; set; }
        public DateTime OwnerDate { get; set; }

        public string RoomName { get; set; }
        public int RoomFloor { get; set; }
        public float RoomArea { get; set;}
        public string RoomType { get; set; }
        public string BuildingName { get; set; }

        public string ParkingName { get; set; }
        public DateTime? ParkingDate { get; set; }
        public string CarType { get; set; }
        public string CarNum { get; set; }
    }
}