using System.ComponentModel;
namespace Entities.Model
{
    public class ProjectModel : ProjectBaseModel
    {
        public int Id { get; set; }

        public List<int> Tasks { get { return _tasks != null ? _tasks : new List<int>(); } set { _tasks = value; } }

        public string State { get; set; }

        public string Code { get; set; }

        private List<int> _tasks;
    }
}
