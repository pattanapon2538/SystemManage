using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class InboxModel
    {
        [Required, Display(Name = "Your name")]
        public string FromName { get; set; }
        [Required, Display(Name = "Your email"), EmailAddress]
        public string FromEmail { get; set; }
        [Required]
        public string Message { get; set; }




        public string MailID { get; set; }
        public string MailName { get; set; }
        [Required]
        public string MailDetail { get; set; }
        [Required, Display(Name = "Email to Send"), EmailAddress]
        public string SendTo { get; set; }
        public string AttachShow1 { get; set; }
        public string AttachShow2 { get; set; }
        public string AttachShow3 { get; set; }
        public string AttachShow4 { get; set; }
        public string AttachShow5 { get; set; }
        public string AttachFile1 { get; set; }
        public string AttachFile2 { get; set; }
        public string AttachFile3 { get; set; }
        public string AttachFile4 { get; set; }
        public string AttachFile5 { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
    }
}