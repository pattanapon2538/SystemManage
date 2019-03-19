using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemManage.Models
{
    public class PositionModel
    {
        public int Position_ID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public string Name { get; set; }
        public string Initial { get; set; }
        public string Detail { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public int Alert { get; set; }
    }
}