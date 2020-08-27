using DietAssistant.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DietAssistant.DAL.Models
{
    public class DailyReport : BaseConsumtionModel
    {
        public DateTime ReportDate { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public bool HasWarnings { get; set; }

        public string Warnings { get; set; }
    }
}
