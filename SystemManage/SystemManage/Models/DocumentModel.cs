using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class DocumentModel
    {
        public int DocumentID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public string DocumentName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public string DocumentDetail { get; set; }
        public string RoleID { get; set; }
        public string Type { get; set; }
        public string AttachShow { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public HttpPostedFileBase AttachFile { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public string pathFormView { get; set; }
        public string ShowPath { get; set; }
        public int Project_Status { get; set; }
    }
}