using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class LogMessageModel
    {
        public int Log_ID { get; set; }
        public string Log_Name { get; set; }
        public string Log_Message { get; set; }
        public string AttachShow1 { get; set; }
        public string AttachFile { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public int Recive { get; set; }
        public int Message_ID { get; set; }
    }
}