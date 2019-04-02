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
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public int ProjectRole { get; set; }
        public int UserRole { get; set; }
        public string User_Permission { get; set; }
        //////////////////////////////////////
        public string TaskName { get; set; }
        public double TaskPercent { get; set; }
        public int SIT_Menu { get; set; }
        public SIT_State SIT { get; set; }
        public string CreateName { get; set; }

        public enum Status
        {
            ดำเนินการ,
            พัก,
            หยุดดำเนินการ,
        }
        public enum SIT_State
        {
           ใช่,
           ไม่ใช่
        }



    }
}