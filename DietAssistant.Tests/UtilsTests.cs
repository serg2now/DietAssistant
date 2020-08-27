using System;
using Xunit;
using DietAssistant.Services.Helpers;

namespace DietAssistant.Tests
{
    public class UtilsTests
    {
        [Fact]
        public void CalculateAge_WhenBirthDayGreatherThenCurrentDay()
        {
            //Prepare test
            var birthDate = DateTime.Today.AddYears(-20).AddDays(-5);

            //Do test
            var age = Utils.CalculateAge(birthDate);

            //Assert
            Assert.Equal(19, age);
        }

        [Fact]
        public void CalculateAge_WhenBirthDayEqualToCurrentDay()
        {
            //Prepare test
            var birthDate = DateTime.Today.AddYears(-20);

            //Do test
            var age = Utils.CalculateAge(birthDate);

            //Assert
            Assert.Equal(20, age);
        }

        [Fact]
        public void CalculateAge_WhenBirthDayLowerThenCurrentDay()
        {
            //Prepare test
            var birthDate = DateTime.Today.AddYears(-20).AddDays(5);

            //Do test
            var age = Utils.CalculateAge(birthDate);

            //Assert
            Assert.Equal(20, age);
        }
    }
}
