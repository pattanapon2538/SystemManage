using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemManage.Models
{
    public class TypeOfCotractModel
    {
        public int Contrat_ID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลครบถ้วน")]
        public string Contrat_Name { get; set; }
        public string Contrat_Detail { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public int Alert { get; set; }
    }
}