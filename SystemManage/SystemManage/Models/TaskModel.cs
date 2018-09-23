using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class TaskModel
    {
        public string TaskID { get; set; }
        public string TaskName { get; set; }
        public double TotalPercent { get; set; }
        public string DescriptionTest { get; set; }
        public string TestID { get; set; }
        public DateTime TestSentDate { get; set; }
        public byte TestStatus { get; set; }
        public string DescriptionQA { get; set; }
        public string QAID { get; set; }
        public DateTime QASentDate { get; set; }
        public byte QAStatus { get; set; }
        public string AttachShow1 { get; set; }
        public string AttachFile1 { get; set; }
        public string AttachShow2 { get; set; }
        public string AttachFile2 { get; set; }
        public string AttachShow3 { get; set; }
        public string AttachFile3 { get; set; }
        public string AttachShow4 { get; set; }
        public string AttachFile4 { get; set; }
        public string AttachShow5 { get; set; }
        public string AttachFile5 { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string ProjectID { get; set; }
    }
}