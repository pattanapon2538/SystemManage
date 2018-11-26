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
        }
    
        public int User_ID { get; set; }
        public string User_Email { get; set; }
        public string User_Password { get; set; }
        public string User_Name { get; set; }
        public string User_LastName { get; set; }
        public string Gender { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string ContractFrom { get; set; }
        public System.DateTime Date_of_Started { get; set; }
        public Nullable<System.DateTime> Date_of_Ended { get; set; }
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
        public Nullable<int> AVG { get; set; }
        public Nullable<int> Amount_Succ { get; set; }
        public Nullable<int> Amount_Non { get; set; }
        public Nullable<int> TotalCoding { get; set; }
        public Nullable<int> Speaking { get; set; }
        public Nullable<int> Reading { get; set; }
        public Nullable<int> Writng { get; set; }
        public Nullable<int> Listening { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public int Contract_ID { get; set; }
        public int Position_ID { get; set; }
    
        public virtual Language_of_Type Language_of_Type { get; set; }
        public virtual Position Position { get; set; }
        public virtual Type_of_Contract Type_of_Contract { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
    }
}
