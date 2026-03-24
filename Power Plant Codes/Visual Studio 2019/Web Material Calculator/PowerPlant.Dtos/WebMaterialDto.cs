using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable

namespace PowerPlant.Dtos
{
    public partial class WebMaterialDto
    {
        [Key]
        public int Rrn { get; set; }

        [Required, StringLength(maximumLength: 9, MinimumLength = 5)]
        [Display(Name = "Item Number")]
        public string ItemNumber { get; set; }

        [Display(Name = "Item Description")]
        public string ItemDescription { get; set; }

        [Required, Range(typeof(double), "1.00", "10.00")]
        [Display(Name = "Core Diameter (inch)")]
        public double CoreDiameter { get; set; }

        [Required, Range(typeof(double), "76.2", "1270.00")]
        [Display(Name = "Roll Diameter (mm)")]
        public double RollDiameter { get; set; }

        [Required, Range(typeof(double), "1.00", "10000.00")]
        [Display(Name = "Length (feet)")]
        public double Length { get; set; }

        [Required]
        [Display(Name = "Thickness (inch)")]
        public double Thickness { get; set; }

        [Range(typeof(int), "1", "10000")]
        [Display(Name = "IMPs")]
        public int? Imps { get; set; }

        [Display(Name = "IMP Length (inch)")]
        public double? Implength { get; set; }

        [Display(Name = "Created On")]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }


        public ItemMasterDto ItemMasterDto { get; set; }


        public IEnumerable<WebMaterialLookupDto> InitalwebMaterialLookupDtos(double? calculatedRollDiameter)
        {
            bool needExtraDataForCalculatedRollDiameter = true;
            List<WebMaterialLookupDto> webMaterialLookupDtos = new();

            for (int i = 10; i >= 0; i--)
            {
                WebMaterialLookupDto webMaterialLookupDto = new()
                {
                    Percentage = i * 10,
                    Length = Math.Round(this.Length * i / 10, 2),
                    RollDiameter = Math.Round(Math.Sqrt(Math.Round(this.Length * i / 10, 2) * 12 * this.Thickness / Math.PI + Math.Pow(this.CoreDiameter, 2)) * 25.4, 2),
                    Imps = this.Imps * i / 10
                };
                if (RollDiameter == calculatedRollDiameter) needExtraDataForCalculatedRollDiameter = false;
                webMaterialLookupDtos.Add(webMaterialLookupDto);
            }

            if (needExtraDataForCalculatedRollDiameter is true && calculatedRollDiameter > 0)
            {
                WebMaterialLookupDto calData = new();
                calData.RollDiameter = (double)calculatedRollDiameter;
                calData.Length = Math.Round(Math.PI * (Math.Pow(calData.RollDiameter / 25.4, 2) - Math.Pow(this.CoreDiameter, 2)) / this.Thickness / 12, 2);
                calData.Percentage = (int)(calData.Length / this.Length * 100);
                calData.Imps = (int?)(this.Imps * (calData.Length / this.Length));
                webMaterialLookupDtos.Add(calData);
                webMaterialLookupDtos = webMaterialLookupDtos.OrderByDescending(m => m.RollDiameter).ToList();
            }

            return webMaterialLookupDtos;
        }
    }
}
