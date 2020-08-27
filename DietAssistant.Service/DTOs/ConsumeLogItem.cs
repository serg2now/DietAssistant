using DietAssistant.Services.Enums;
using System;

namespace DietAssistant.Services.DTOs
{
    public class ConsumeLogItem
    {
        public int Id { get; set; }

        public Food Food { get; set; }

        public FoodType FoodType { get; set; }

        public string FoodTypeDisplayValue { get; set; }

        public ConsumeTimeType ConsumeTimeType { get; set; }

        public string ConsumeTimeTypeDisplayValue { get; set; }

        public int CustomerId { get; set; }

        public DateTime DateOfConsume { get; set; }
    }
}
