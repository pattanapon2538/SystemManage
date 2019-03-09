using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SystemManage.Database;

namespace SystemManage.Models
{
    public class InboxModel
    {
        public string MailID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public string MailName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public string MailDetail { get; set; }
        [Required, Display(Name = "Email to Send"), EmailAddress]
        public int Status { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public string SendTo { get; set; }
        public string AttachShow1 { get; set; }
        public IEnumerable<HttpPostedFileBase> AttachFile1 { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UserList { get; set; }

        public List<User> List { get; set; }
        public string Image { get; set; }
    }
}