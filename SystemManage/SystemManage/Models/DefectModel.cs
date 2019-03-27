using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class DefectModel
    {
        public int Defect_ID { get; set; }
        public int Sub_ID { get; set; }
        public string Detail { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public DateTime SendDate { get; set; }
        public int Status { get; set; }
        public string AttachShow { get; set; }
        public HttpPostedFileBase AttachFile { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public string Comment_Test { get; set; }
        public string Comment_Dev { get; set; }
        public StatusDefectDev StatusDev { get; set; }
        public StatusDefectTest StatusTest { get; set; }


        public string TaskName { get; set; }
        public string SubTaskName { get; set; }
        public int DevID { get; set; }
        public string DevName { get; set; }
        public string TestName { get; set; }
        public string QAName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public List<string> DetailList { get; set; }
        public List<int> StausList { get; set; }
        public List<string> AttachShowList { get; set; }
        public List<HttpPostedFileBase> AttachFileList { get; set; }
        public List<DateTime> SendDateList { get; set; }

        public enum StatusDefectDev
        {
            แก้ไขแล้ว,
            กำลังแก้ไข
        }
        public enum StatusDefectTest
        {
            ปิด,
            แก้ไขใหม่
        }
        public string Error_Detail { get; set; }
        public string Path_Defect { get; set; }
        public int Project_Status { get; set; }
    }
}