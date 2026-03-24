using System;
using System.Collections.Generic;

#nullable disable

namespace PowerPlant.Models
{
    public partial class WebMaterial
    {
        public int Rrn { get; set; }
        public string ItemNumber { get; set; }
        public double CoreDiameter { get; set; }
        public double RollDiameter { get; set; }
        public double Length { get; set; }
        public double Thickness { get; set; }
        public int? Imps { get; set; }
        public double? Implength { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }


        public ItemMaster ItemMaster { get; set; }
    }
}
