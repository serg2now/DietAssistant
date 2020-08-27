using DietAssistant.Services.Enums;
using System;

namespace DietAssistant.Services.DTOs
{
    public class Customer : SystemUser
    {
        public DateTime BirthDate { get; set; }

        public int Age { get; set; }

        public int WeightInKilos { get; set; }

        public decimal HeightInMeters { get; set; }

        public TypeOfBody BodyType { get; set; }
    }
}
