using AutoMapper;
using DietAssistant.DAL.Models;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Enums;
using DietAssistant.Services.Helpers;

namespace DietAssistant.Services.MappingConfigurations
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, Customer>()
                .ForMember(m => m.Age, opt => opt.MapFrom(s => Utils.CalculateAge(s.BirthDate.Value)))
                .ForMember(m => m.BodyType, opt => opt.MapFrom(s => (TypeOfBody)s.BodyTypeId.Value));

            CreateMap<SystemUser, User>()
                .Include<Customer, User>()
                .ForMember(m => m.BodyType, opt => opt.Ignore());

            CreateMap<Customer, User>()
                .ForMember(m => m.BodyTypeId, opt => opt.MapFrom(s => (int)s.BodyType))
                .ForMember(m => m.BodyType, opt => opt.Ignore());

            
        }
    }
}
