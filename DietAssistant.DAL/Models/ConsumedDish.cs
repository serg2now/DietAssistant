using System;
using DietAssistant.DAL.Models.Base;

namespace DietAssistant.DAL.Models
{
    public class ConsumedDish : BaseConsumtionModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateOfConsume { get; set; }

        public int ConsumeTimeTypeId { get; set; }

        public ConsumeTimeType ConsumeTimeType { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public bool IsFoodStuff { get; set; }
    }
}
