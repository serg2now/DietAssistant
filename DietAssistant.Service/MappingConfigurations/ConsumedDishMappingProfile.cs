using AutoMapper;
using DietAssistant.DAL.Models;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Enums;

namespace DietAssistant.Services.MappingConfigurations
{
    public class ConsumedDishMappingProfile : Profile
    {
        public ConsumedDishMappingProfile()
        {
            CreateMap<ConsumedDish, ConsumeLogItem>()
                .ForMember(m => m.ConsumeTimeType, m => m.MapFrom(s => (Enums.ConsumeTimeType)s.ConsumeTimeTypeId))
                .ForMember(m => m.CustomerId, m => m.MapFrom(s => s.UserId))
                .ForMember(m => m.FoodType, m => m.MapFrom(s => (s.IsFoodStuff) ? FoodType.FoodStuff : FoodType.Dish))
                .ForMember(m => m.Food, m => m.MapFrom(s => MapConsumedDisOnFood(s)))
                .ForMember(m => m.FoodTypeDisplayValue, m => m.MapFrom(s => GetFoodTypeDisplayValue(s.IsFoodStuff)))
                .ForMember(m => m.ConsumeTimeTypeDisplayValue, m => m.MapFrom(s => ((Enums.ConsumeTimeType)s.ConsumeTimeTypeId).ToString()));

            CreateMap<ConsumeLogItem, ConsumedDish>()
                .ForMember(m => m.UserId, m => m.MapFrom(s => s.CustomerId))
                .ForMember(m => m.IsFoodStuff, m => m.MapFrom(s => s.FoodType == FoodType.FoodStuff))
                .ForMember(m => m.ConsumeTimeTypeId, m => m.MapFrom(s => (int)s.ConsumeTimeType))
                .ForMember(m => m.ConsumeTimeType, m => m.Ignore())
                .ForMember(m => m.CarbohydratesAmount, m => m.MapFrom(s => s.Food.CarbohydratesAmount))
                .ForMember(m => m.ProteinsAmount, m => m.MapFrom(s => s.Food.ProteinsAmount))
                .ForMember(m => m.FatsAmount, m => m.MapFrom(s => s.Food.FatsAmount))
                .ForMember(m => m.Name, m => m.MapFrom(s => s.Food.Name))
                .ForMember(m => m.Description, m => m.MapFrom(s => s.Food.Description));
        }

        private Food MapConsumedDisOnFood(ConsumedDish dish)
        {
            var dishItem = new Food()
            {
                CarbohydratesAmount = dish.CarbohydratesAmount,
                FatsAmount = dish.FatsAmount,
                ProteinsAmount = dish.ProteinsAmount,
                Name = dish.Name,
                Description = dish.Description
            };

            return dishItem;
        }

        private string GetFoodTypeDisplayValue(bool isFoodStuff)
        {
            return ((isFoodStuff) ? FoodType.FoodStuff : FoodType.Dish).ToString();
        }
    }
}
