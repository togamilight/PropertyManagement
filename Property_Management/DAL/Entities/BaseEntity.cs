using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Property_Management.DAL.Entities {
    public class BaseEntity {
        [Key]
        public int Id;
    }
}