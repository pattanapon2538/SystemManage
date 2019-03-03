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
    
    public partial class SubTask
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubTask()
        {
            this.SubTaskSubmisstions = new HashSet<SubTaskSubmisstion>();
            this.Defects = new HashSet<Defect>();
        }
    
        public int SubID { get; set; }
        public string SubName { get; set; }
        public int SubStatus { get; set; }
        public double SubPercent { get; set; }
        public string SubDescriptionDev { get; set; }
        public int SubDevID { get; set; }
        public DateTime SubDevSend { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int TaskID { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public int Handle { get; set; }
        public int HaveDefect { get; set; }
        public int RoundCoding { get; set; }
        public string Comment_Dev { get; set; }
        public string Comment_Tester { get; set; }
        public string Comment_QA { get; set; }
        public string Comment_CM { get; set; }
    
        public virtual Task Task { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTaskSubmisstion> SubTaskSubmisstions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Defect> Defects { get; set; }
    }
}
