  //------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SystemManage.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.ProjectMembers = new HashSet<ProjectMember>();
            this.Skills = new HashSet<Skill>();
        }
    
        public int User_ID { get; set; }
        public string User_Email { get; set; }
        public string User_Password { get; set; }
        public string User_Name { get; set; }
        public string User_LastName { get; set; }
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
        public string AttachFile1 { get; set; }
        public string AttachFile2 { get; set; }
        public string AttachFile3 { get; set; }
        public string AttachFile4 { get; set; }
        public string Permisstion { get; set; }
        public string comment { get; set; }
        public int LanguageID { get; set; }
        public double AVG { get; set; }
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
        public string Phone { get; set; }
    
        public virtual Language_of_Type Language_of_Type { get; set; }
        public virtual Position Position { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
        public virtual Type_of_Contract Type_of_Contract { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
