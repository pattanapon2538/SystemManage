using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class RoleModel
    {
        public string Role_ID { get; set; }
        public string Role_Name { get; set; }
        public string Role_Initial { get; set; }
        public string Role_Detail { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }
}