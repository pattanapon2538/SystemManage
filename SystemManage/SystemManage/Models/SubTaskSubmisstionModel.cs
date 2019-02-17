using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class SubTaskSubmisstionModel
    {
        public int Submit_ID { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime SubmitDate { get; set; }
        public int HandleBy { get; set; }
        public int SubID { get; set; }
    }
}