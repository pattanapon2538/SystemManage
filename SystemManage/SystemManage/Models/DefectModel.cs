using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class DefectModel
    {
        public int Defect_ID { get; set; }
        public int Sub_ID { get; set; }
        public string Detail { get; set; }
        public DateTime SendDate { get; set; }
        public int Status { get; set; }
        public string AttachShow { get; set; }
        public string AttachFile { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }


        public string TaskName { get; set; }
        public string SubTaskName { get; set; }
        public string DevName { get; set; }
        public string TestName { get; set; }
        public string QAName { get; set; }

        public List<string> DetailList { get; set; }
        public List<int> StausList { get; set; }
        public List<string> AttachShowList { get; set; }
        public List<string> AttachFileList { get; set; }
        public List<DateTime> SendDateList { get; set; }

    }
}