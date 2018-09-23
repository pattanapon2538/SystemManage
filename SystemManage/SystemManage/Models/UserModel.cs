using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemManage.Models
{
    public class UserModel
    {
        public string Users_ID { get; set; }
        [Required(ErrorMessage = "กรุณาใส่ Email ให้ถูกต้อง")]
        public string User_Email { get; set; }
        [Required(ErrorMessage = "กรุณาใส่ Password ให้ถูกต้อง")]
        public string User_Password { get; set; }
        public string User_Name { get; set; }
        public string User_LastName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string ContractFrom { get; set; }
        public DateTime Date_of_Started { get; set; }
        public Nullable<DateTime> Date_of_Ended { get; set; }
        public string AttachShow1 { get; set; }
        public string AttachShow2 { get; set; }
        public string AttachShow3 { get; set; }
        public string AttachShow4 { get; set; }
        public string AttachFile1 { get; set; }
        public string AttachFile2 { get; set; }
        public string AttachFile3 { get; set; }
        public string AttachFile4 { get; set; }
        public string Permission { get; set; }
        public Nullable<double> SuccPass { get; set; }
        public Nullable<double> Non_Success { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string RoleID { get; set; }
        public string ContractID { get; set; }
        public string PositionID { get; set; }
        public bool Favorite { get; set; }
    }
}