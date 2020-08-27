using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Interfaces;
using System.Text;
using DietAssistant.DAL.Models;
using System.Linq;

namespace DietAssistant.Services
{
    public class DietParametersService : IDietService
    {
        protected readonly DietLimits _dietLimits;

        //Constructor for test purposes
        public DietParametersService(DietLimits dietLimits)
        {
            _dietLimits = dietLimits;
            ValidateLimits();
        }

        protected virtual void ValidateLimits()
        {
            var dietLimitsProperties = typeof(DietLimits).GetProperties();

            bool isValid = true;

            foreach(var property in dietLimitsProperties)
            {
                var value = (Limits)property.GetValue(_dietLimits);

                if (value.Min <= 0 || value.Max <= 0)
                {
                    isValid = false;
                    break;
                }
            }

            if (!isValid)
            {
                throw new ArgumentException("All Food limits should be defined in appsettings.json file");
            }
        }

        public virtual void ValidateDailyReport(DailyReport report)
        {
            var warningsBuilder = new StringBuilder();
            var parameters = new[] { "Proteins", "Carbohydrates", "Fats"};
            var dietLimitsProperties = typeof(DietLimits).GetProperties();
            var reportProperties = typeof(DailyReport).GetProperties();

            foreach(var parameter in parameters)
            {
                var reportProperty = (decimal)reportProperties.First(p => p.Name.Contains(parameter)).GetValue(report);
                var limitProperty = (Limits)dietLimitsProperties.First(p => p.Name.Contains(parameter)).GetValue(_dietLimits);

                if (reportProperty < limitProperty.Min)
                {
                    warningsBuilder.AppendLine($"Daily {parameter} amount is lower than defined minimum value {limitProperty.Min}");
                }

                if (reportProperty > limitProperty.Max)
                {
                    warningsBuilder.AppendLine($"Daily {parameter} amount is greather than defined maximum value {limitProperty.Max}");
                }
            }

            report.Warnings = warningsBuilder.ToString();
            report.HasWarnings = report.Warnings.Length > 0;
        }
    }
}
