using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SystemManage.Models
{
    [DataContract]
    public class Chart
    {
        public Chart(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }
        [DataMember(Name = "label")]
        public string Label = "";
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}