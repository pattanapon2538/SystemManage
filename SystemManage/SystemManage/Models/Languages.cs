using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class Languages
    {
        [Required(ErrorMessage = "กรุณใส่ภาษา")]
        public string languages { get; set; }      
        public DateTime? UpdateDate { get; set; }      
        public string CreateBy { get; set; }      
        public string UpdateBy { get; set; }      
    }
}