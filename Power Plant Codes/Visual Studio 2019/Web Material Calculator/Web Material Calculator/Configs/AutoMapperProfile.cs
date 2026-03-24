    using AutoMapper;
using PowerPlant.Dtos;
using PowerPlant.Models;
using PowerPlant.Service;
using PowerPlant.Models.Context;
using System;

namespace Web_Material_Calculator.Configs
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<WebMaterial, WebMaterialDto>()
                .ForMember(
                    dest => dest.ItemDescription,
                    opt => opt.MapFrom(src => $"{src.ItemMaster.ItemDesc1.Trim()} {src.ItemMaster.ItemDesc2.Trim()}"));

            CreateMap<WebMaterialDto, WebMaterial>()
                .ForMember(
                    // Thickness = Pi * (RollDiameter^2 - CoreDiameter^2) / Length
                    dest => dest.Thickness,
                    opt => opt.MapFrom(src => Math.Round(Math.PI * (Math.Pow(src.RollDiameter / 25.4, 2) - Math.Pow(src.CoreDiameter, 2)) / src.Length / 12, 5)))
                .ForMember(
                    // Implength = Length / Imps
                    dest => dest.Implength,
                    opt => opt.MapFrom(src => src.Imps.HasValue ? Math.Round(src.Length * 12 / (double)src.Imps, 2) : src.Implength))
                .ForMember(
                    dest => dest.CreatedOn,
                    opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}