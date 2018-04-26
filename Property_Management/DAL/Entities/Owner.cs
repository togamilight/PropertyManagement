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

        [Display(Name="密码"), Required, StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        public bool Sex { get; set; }

        [Display(Name="联系电话"), Required, Phone, StringLength(20), RegularExpression(@"^((1[3|4|5|8]\d{9})|([2-9]\d{6,7}))$", ErrorMessage = "手机或固话号码的格式不正确")]
        public string PhoneNum { get; set; }

        [Display(Name="身份证号"), Required, StringLength(20), RegularExpression(@"^(([1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx])|([1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}))$", ErrorMessage = "身份证号格式不正确")]
        public string IDCard { get; set; }

        public int RoomId { get; set; }

        public int? ParkingId { get; set; }

        public DateTime Date { get; set; }

        public bool Disuse { get; set; }

        public DateTime? DisuseDate { get; set; }
    }
}