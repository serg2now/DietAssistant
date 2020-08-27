using System;

namespace DietAssistant.Services.Helpers
{
    public static class Utils
    {
        public static int CalculateAge(DateTime birthDate)
        {
            var currentDate = DateTime.Today;
            
            var age = (birthDate.DayOfYear >= currentDate.DayOfYear) 
                ? currentDate.Year - birthDate.Year 
                : currentDate.Year - birthDate.Year - 1;

            return age;
        }
    }
}
