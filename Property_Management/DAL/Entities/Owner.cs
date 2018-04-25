using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    [Table("owner")]
    public class Owner : BaseEntity{
        [Display(Name = "名称"), Required, StringLength(20)]
        public string Name { get; set; }

        [Display(Name="密码"), Required, StringLength(20)]
        public string Password { get; set; }

        public bool Sex { get; set; }

        [Display(Name="联系电话"), Required, Phone, StringLength(20)]
        public string PhoneNum { get; set; }

        [Display(Name="身份证号"), Required, StringLength(20)]
        public string IDCard { get; set; }

        public int RoomId { get; set; }

        public int ParkingId { get; set; }

        public DateTime Date { get; set; }

        public bool Disuse { get; set; }

        public DateTime? DisuseDate { get; set; }
    }
}