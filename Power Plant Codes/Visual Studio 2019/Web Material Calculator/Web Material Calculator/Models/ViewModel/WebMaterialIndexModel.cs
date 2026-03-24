using PowerPlant.Dtos;
using System.Collections.Generic;

namespace Web_Material_Calculator.Models.ViewModel
{
    public class WebMaterialIndexModel
    {
        public IEnumerable<WebMaterialDto> WebMaterialDtos { get; set; } = new List<WebMaterialDto>();

        public WebMaterialDto? EditedWebMaterialDto { get; set; } 

        public string? AlertMessage { get; set; }
    }
}