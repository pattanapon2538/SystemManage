using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemManage.Database;

namespace SystemManage.Models
{
    public class SITModel
    {
        public int SIT_ID { get; set; }
        public int Project_ID { get; set; }
        public int Tester_ID { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string AttachShow { get; set; }
        public string AttachFile { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public int Task_ID { get; set; }

        public List<Task> Task { get; set; }
        public List<ProjectMember> Tester { get; set; }
    }
}