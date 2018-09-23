using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class InboxModel
    {
        public string MailID { get; set; }
        public string MailName { get; set; }
        public string MailDetail { get; set; }
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