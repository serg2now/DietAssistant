using DietAssistant.Services.Enums;
using System.Collections.Generic;

namespace DietAssistant.Services.DTOs
{
    public class GroupStatistic
    {
        public TypeOfBody Key { get; set; }

        public string KeyDisplayValue { get; set; }

        public decimal AverageCarbohydratesAmount { get; set; }

        public decimal AverageProteinsAmount { get; set; }

        public decimal AverageFatsAmount { get; set; }

        public IEnumerable<AdminReportDTO> Values { get; set; }
    }
}
