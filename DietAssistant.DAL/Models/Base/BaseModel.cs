using DietAssistant.DAL.Models.Interfaces;

namespace DietAssistant.DAL.Models.Base
{
    public abstract class BaseModel : IModel
    {
        public int Id { get; set; }
    }
}
