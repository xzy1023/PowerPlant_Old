using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Dtos
{
    public class WebMaterialLookupDto
    {
        [Display(Name = "Percentage %")]
        public int Percentage { get; set; }

        [Display(Name = "Roll Diameter (mm)")]
        public double RollDiameter { get; set; }

        [Display(Name = "Length (feet)")]
        public double Length { get; set; }

        [Display(Name = "IMPs")]
        public int? Imps { get; set; }
    }
}
