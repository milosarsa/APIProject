namespace Interfaces.Service
{
    public interface ITaskService
    {
        public Task CreateTask(TaskBaseModel Task);
        public Task<List<TaskModel>> GetAllTasks();
        public Task<List<TaskModel>> GetAllTasksByProject(int projectId);
        public Task<TaskModel> GetTask(int id);
        public Task UpdateTask(int id, TaskBaseModel Task);
        public Task DeleteTask(int id);
        public Task UpdateTaskState(int taskId, TaskState projectState);
    }
}
