using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class InboxModel
    {
        public string MailID { get; set; }
        public string MailName { get; set; }
        [Required]
        public string MailDetail { get; set; }
        [Required, Display(Name = "Email to Send"), EmailAddress]
        public int Status { get; set; }
        public string SendTo { get; set; }
        public string AttachShow1 { get; set; }
        public string AttachFile1 { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateBy { get; set; }
    }
}