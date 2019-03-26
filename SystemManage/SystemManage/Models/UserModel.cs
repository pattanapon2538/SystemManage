using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public string User_EmailError { get; set; }
        [Required(ErrorMessage = "กรุณาใส่ Password ให้ถูกต้อง")]
        public string User_Password { get; set; }
        public string User_PasswordError { get; set; }
        [Required(ErrorMessage = "กรุณาใส่ Password ให้ถูกต้อง")]
        public string User_RePassword { get; set; }
        public string User_Name { get; set; }
        public string User_LastName { get; set; }
        public string NikcName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string ContractFrom { get; set; }
        public DateTime Date_of_Started { get; set; }
        public DateTime Date_of_Ended { get; set; }
        public string AttachShow1 { get; set; }
        public string AttachShow2 { get; set; }
        public string AttachShow3 { get; set; }
        public string AttachShow4 { get; set; }
        [DisplayName("Upload File")]
        public HttpPostedFileBase AttachFile1 { get; set; }
        public HttpPostedFileBase AttachFile2 { get; set; }
        public HttpPostedFileBase AttachFile3 { get; set; }
        public HttpPostedFileBase AttachFile4 { get; set; }
        public string PathShow1 { get; set; }
        public string PathShow2 { get; set; }
        public string PathShow3 { get; set; }
        public string PathShow4 { get; set; }
        public string Permission { get; set; }
        public string Comment { get; set; }
        public int LanguageID { get; set; }
        public double AVG { get; set; }
        public double Amount_Succ { get; set; }
        public double Percent_Succ { get; set; }
        public double Amount_Non { get; set; }
        public double Percent_Non { get; set; }
        public int TotalCoding { get; set; }
        public string Speaking { get; set; }
        public string Reading { get; set; }
        public string Writng { get; set; }
        public string Listening { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public int Contract_ID { get; set; }
        public int Position_ID { get; set; }
        public string Phone { get; set; }
        public string PositionName { get; set; }
        public string ContactName { get; set;  }
        public int Role { get; set; }
        public int Check_Role { get; set; }

        public int ProjectCreateBy { get; set; }
        public HttpPostedFileBase File1 { get; set; }
        [NotMapped]
        public List<Position> PositionList { get; set; }
        public List<Type_of_Contract> ContractsList { get; set; }
        public List<Language_of_Type> LanguageList { get; set; }
        //////////////////////////////////////
        public string TaskName { get; set; }
        public string Level { get; set; }
        public int RoundCoding { get; set; }
        public Levels _Speaking { get; set; }
        public Levels _Reading { get; set; }
        public Levels _Writng { get; set; }
        public Levels _Listening { get; set; }
        public Sex Genders { get; set; }
        public _Role Roles { get; set; }
        public string Select_Laguages { get; set; }
        public enum Levels
        {
            อ่อน,
            ปานกลาง,
            เก่ง,
        }
        public enum Sex
        {
            ชาย,
            หญิง,
        }
        public enum _Role
        {
            ผู้จัดการโครงการ,
            ผู้พัฒนาซอฟแวร์,
            ผู้ทดสอบ,
            ผู้ตรวจคุณภาพ,
            ลูกค้า
        }
        public int Follow_C { get; set; }
        public string language_string { get; set; }
        public int open_Role { get; set; }
        public int Alert { get; set; }
        public int Project_Status { get; set; }
    }
    
   
}