using DietAssistant.Services.Enums;

namespace DietAssistant.Services.DTOs
{
    public class AdminReportDTO : DailyReportDTO
    {
        public TypeOfBody BodyType { get; set; }

        public string BodyTypeDisplayValue { get; set; }
    }
}
