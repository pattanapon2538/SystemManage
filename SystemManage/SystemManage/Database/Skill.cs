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
    
    public partial class Skill
    {
        public int SkillsID { get; set; }
        public int User_ID { get; set; }
        public int languageID { get; set; }
        public int level_of_lg { get; set; }
    
        public virtual Language_of_Type Language_of_Type { get; set; }
    }
}
