using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class LanguageOfTypeModel
    {
        public string languageID { get; set; }
        public string languagesName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string Updateby { get; set; }
    }
}