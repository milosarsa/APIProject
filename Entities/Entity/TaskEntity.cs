using Entities.Enums;

namespace Entities.Entity
{
    public class TaskEntity : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int State { get; set; }
    }
    
}
