using System;

namespace DietAssistant.API.DTOs
{
    public class CreateReportModel
    {
        public int CustomerId { get; set; }

        public DateTime ReportDate { get; set; }
    }
}
