using DietAssistant.DAL.Models.Base;
using System;
using System.Collections.Generic;

namespace DietAssistant.DAL.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string MiddleName { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? WeightInKilos { get; set; }

        public decimal? HeightInMeters { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public int? BodyTypeId { get; set; }

        public BodyType BodyType { get; set; }

        public List<ConsumedDish> ConsumedDishes { get; set; }

        public List<DailyReport> DailyReports { get; set; }
    }
}
