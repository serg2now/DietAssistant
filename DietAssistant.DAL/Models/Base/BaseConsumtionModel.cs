namespace DietAssistant.DAL.Models.Base
{
    public abstract class BaseConsumtionModel : BaseModel
    { 
        public decimal CarbohydratesAmount { get; set; }

        public decimal ProteinsAmount { get; set; }

        public decimal FatsAmount { get; set; }
    }
}
