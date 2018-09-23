using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class DocumentModel
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDetail { get; set; }
        public string RoleID { get; set; }
        public string AttachShow { get; set; }
        public string AttachFile { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }
}