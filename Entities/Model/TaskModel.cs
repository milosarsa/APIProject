namespace Entities.Model
{
    public class TaskModel : TaskBaseModel
    {

        public int ProjectId { get; set; }
        public string State { get; set; }
    }
}
