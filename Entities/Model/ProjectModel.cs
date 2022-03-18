using System.ComponentModel;
namespace Entities.Model
{
    public class ProjectModel : ProjectBaseModel
    {
        [ReadOnly(true)]
        public int Id { get; set; }

        [ReadOnly(true)]
        public List<int> Tasks { get { return _tasks != null ? _tasks : new List<int>(); } set { _tasks = value; } }

        public string State { get; set; }

        private List<int> _tasks;
    }
}
