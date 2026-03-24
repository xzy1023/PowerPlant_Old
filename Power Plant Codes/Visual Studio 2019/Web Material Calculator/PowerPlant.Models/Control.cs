using System;
using System.Collections.Generic;

#nullable disable

namespace PowerPlant.Models
{
    public partial class Control
    {
        public string Facility { get; set; }
        public string Key { get; set; }
        public string SubKey { get; set; }
        public string Description { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }
}
