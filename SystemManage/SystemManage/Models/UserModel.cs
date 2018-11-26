using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SystemManage.Database;

namespace SystemManage.Models
{
    public class UserModel
    {
        public int Users_ID { get; set; }
        [Required(ErrorMessage = "กรุณาใส่ Email ให้ถูกต้อง")]
        public string User_Email { get; set; }
        [Required(ErrorMessage = "กรุณาใส่ Password ให้ถูกต้อง")]
        public string User_Password { get; set; }
        public string User_Name { get; set; }
        public string User_LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string ContractFrom { get; set; }
        public DateTime Date_of_Started { get; set; }
        public DateTime Date_of_Ended { get; set; }
        public string AttachShow1 { get; set; }
        public string AttachShow2 { get; set; }
        public string AttachShow3 { get; set; }
        public string AttachShow4 { get; set; }
        public string AttachFile1 { get; set; }
        public string AttachFile2 { get; set; }
        public string AttachFile3 { get; set; }
        public string AttachFile4 { get; set; }
        public string Permission { get; set; }
        public string comment { get; set; }
        public int LanguageID { get; set; }
        public int AVG { get; set; }
        public int Amount_Succ { get; set; }
        public int Amount_Non { get; set; }
        public int TotalCoding { get; set; }
        public int Speaking { get; set; }
        public int Reading { get; set; }
        public int Writng { get; set; }
        public int Listening { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public int Contract_ID { get; set; }
        public int Position_ID { get; set; }
        [NotMapped]
        public List<Position> PositionList { get; set; }
        public List<Type_of_Contract> ContractsList { get; set; }
        public List<Language_of_Type> LanguageList { get; set; }
    }
}