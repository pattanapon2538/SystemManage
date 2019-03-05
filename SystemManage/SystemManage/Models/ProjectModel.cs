using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace SystemManage.Models
{

    public class ProjectModel
    {

        public int ProjectID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public string ProjectDescription { get; set; }
        public int ProjectStatus { get; set; }
        public Status status { get; set; }
        public double TotalPercent { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        [DataType(DataType.Date)]
        public DateTime ProjectSendDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public int ProjectRole { get; set; }
        public int UserRole { get; set; }
        //////////////////////////////////////
        public string TaskName { get; set; }
        public double TaskPercent { get; set; }
        public int SIT_Menu { get; set; }

        public enum Status
        {
            ดำเนินการ,
            พัก,
            หยุดดำเนินการ,
        }



    }
}