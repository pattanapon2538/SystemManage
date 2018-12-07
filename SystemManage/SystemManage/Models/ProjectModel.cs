using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemManage.Models
{
    public class ProjectModel
    {
        public int ProjectID { get; set; }
        [Required(ErrorMessage = "กรุณาใส่ข้อมูลให้ถูกต้อง")]
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public byte ProjectStatus { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProjectSendDate { get; set; }
        public int TotalPercent { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }
}