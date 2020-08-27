using AutoMapper;
using DietAssistant.DAL.Models;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Enums;

namespace DietAssistant.Services.MappingConfigurations
{
    public class DailyReportMappingProfile : Profile
    {
        public DailyReportMappingProfile()
        {
            CreateMap<DailyReport, DailyReportDTO>()
                .ForMember(m => m.CustomerName, opt => opt.MapFrom(s => s.User.Name))
                .ForMember(m => m.CustomerSurname, opt => opt.MapFrom(s => s.User.Surname));

            CreateMap<DailyReport, AdminReportDTO>()
                .ForMember(m => m.CustomerName, opt => opt.MapFrom(s => s.User.Name))
                .ForMember(m => m.CustomerSurname, opt => opt.MapFrom(s => s.User.Surname))
                .ForMember(m => m.BodyType, opt => opt.MapFrom(s => (TypeOfBody)s.User.BodyTypeId))
                .ForMember(m => m.BodyTypeDisplayValue, opt => opt.MapFrom(s => ((TypeOfBody)s.User.BodyTypeId).ToString()));
        }
    }
}
