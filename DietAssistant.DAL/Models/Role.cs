using DietAssistant.DAL.Models.Base;
using System.Collections.Generic;

namespace DietAssistant.DAL.Models
{
    public class Role : BaseModel
    {
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
