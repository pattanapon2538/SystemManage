using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class SubTaskModel
    {
        public string SubID { get; set; }
        public string SubName { get; set; }
        public byte SubStatus { get; set; }
        public double SubPercent { get; set; }
        public string SubDescriptionDev { get; set; }
        public string SubDevID { get; set; }
        public DateTime SubDevSend { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string TaskID { get; set; }
    }
}