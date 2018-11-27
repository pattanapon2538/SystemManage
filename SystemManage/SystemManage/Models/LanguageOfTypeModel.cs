using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemManage.Models
{
    public class LanguageOfTypeModel
    {
        public int languageID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลให้ครบถ้วน")]
        public string languagesName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int Updateby { get; set; }
    }
}