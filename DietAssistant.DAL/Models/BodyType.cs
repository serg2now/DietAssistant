using DietAssistant.DAL.Models.Base;
using DietAssistant.DAL.Models.Interfaces;
using System.Collections.Generic;

namespace DietAssistant.DAL.Models
{
    public class BodyType : BaseModel, IReferenceTypeModel
    {
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
