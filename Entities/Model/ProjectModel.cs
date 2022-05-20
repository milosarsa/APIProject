namespace Entities.Model
{
    public class ProjectModel : ProjectBaseModel
    {
        public int Id { get; set; }

        public List<int> Tasks { get { return _tasks ?? new List<int>(); } set { _tasks = value; } }

        public string State { get; set; }


        private List<int> _tasks;
    }
}
