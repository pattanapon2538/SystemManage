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
    
    public partial class Defect
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Defect()
        {
            this.SubDefects = new HashSet<SubDefect>();
        }
    
        public int Defect_ID { get; set; }
        public int Sub_ID { get; set; }
        public string Detail { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public Nullable<int> Status { get; set; }
        public string AttachShow { get; set; }
        public string AttachFile { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual SubTask SubTask { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubDefect> SubDefects { get; set; }
    }
}