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
    
    public partial class Document
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDetail { get; set; }
        public string AttachShow { get; set; }
        public string AttachFile { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public int Project_ID { get; set; }
    
        public virtual Project Project { get; set; }
    }
}
