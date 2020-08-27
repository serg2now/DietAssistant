using System;

namespace DietAssistant.Services.DTOs
{
    public class DailyReportDTO
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerSurname { get; set; }

        public DateTime ReportDate { get; set; }

        public decimal CarbohydratesAmount { get; set; }

        public decimal ProteinsAmount { get; set; }

        public decimal FatsAmount { get; set; }

        public bool HasWarnings { get; set; }
        
        public string Warnings { get; set; }
    }
}
