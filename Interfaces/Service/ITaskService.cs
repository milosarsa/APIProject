using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Service
{
    public interface ITaskService
    {
        public Task CreateTask(int projectId, TaskBaseModel Task);
        public Task<List<TaskModel>> GetAllTasks();
        public Task<List<TaskModel>> GetAllTasksByProject(int projectId);
        public Task<TaskModel> GetTask(int id);
        public Task UpdateTask(int projectId, int id, TaskBaseModel Task);
        public Task DeleteTask(int id);
        public Task UpdateTaskState(int taskId, TaskState projectState);
    }
}
