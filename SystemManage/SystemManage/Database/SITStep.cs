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
    
    public partial class SITStep
    {
        public int Step_ID { get; set; }
        public int SIT_ID { get; set; }
        public int Step { get; set; }
        public int Task_ID { get; set; }
    
        public virtual Task Task { get; set; }
        public virtual SIT SIT { get; set; }
    }
}
