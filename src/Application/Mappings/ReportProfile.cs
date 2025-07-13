using AutoMapper;
using AutoService.Core.DTOs;
using AutoService.Core.Entities;

namespace AutoService.Application.Mappings
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Repair, RepairItemDto>()
                .ForMember(dest => dest.CarMake, opt => opt.MapFrom(src => src.Car.Make))
                .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.Car.Model))
                .ForMember(dest => dest.MasterFullName, opt => opt.MapFrom(src => src.Master.FullName));
        }
    }
}