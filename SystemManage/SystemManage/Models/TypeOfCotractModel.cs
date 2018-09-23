using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class TypeOfCotractModel
    {
        public string Contrat_ID { get; set; }
        public string Contrat_Name { get; set; }
        public string Contrat_Detail { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateBy { get; set; }
    }
}