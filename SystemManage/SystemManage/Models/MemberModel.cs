using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemManage.Models
{
    public class MemberModel
    {
        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public int Role { get; set; }
        public int MemberID { get; set; }
    }
}