using System.Net;

namespace Service
{
    //Responsible for data manipulation
    public class TaskService : ITaskService
    {
        private ILogger<TaskService> logger;
        private ITaskRepo taskRepo;
        private IProjectRepo projectRepo;
        public TaskService(ITaskRepo _taskRepo, ILogger<TaskService> _logger, IProjectRepo _projectRepo)
        {
            taskRepo = _taskRepo ?? throw new ArgumentNullException(nameof(taskRepo));
            logger = _logger ?? throw new ArgumentNullException(nameof(logger));
            projectRepo = _projectRepo ?? throw new ArgumentNullException(nameof(projectRepo));
        }
        public async Task CreateTask(TaskBaseModel task)
        {
            int projectId = task.ProjectId;
            if (!await projectRepo.Exists(projectId.ToString()))
                throw new HttpRequestException("Project doesn't exist!", null, HttpStatusCode.NotFound);

            TaskEntity taskEntity = Mapper.TaskBaseObjectToEntity(task, true);
            int taskId = await taskRepo.CreateAsync(taskEntity);
            List<int> projectTasks = await GetProjectTasks(projectId);
            if (projectTasks.Contains(taskId))
            {
                throw new HttpRequestException("Task already exists for a given project!", null, HttpStatusCode.Conflict);
            }
            projectTasks.Add(taskId);
            await UpdateProjectTasks(projectId, projectTasks);
        }

        public async Task<List<TaskModel>> GetAllTasks()
        {
            List<TaskEntity> taskEntities = await taskRepo.ReadAllAsync();
            List<TaskModel> tasks = new List<TaskModel>();

            foreach (var entity in taskEntities)
            {
                TaskModel Task = Mapper.TaskEntityToObject(entity);
                tasks.Add(Task);
            }

            return tasks;
        }

        public async Task<List<TaskModel>> GetAllTasksByProject(int projectId)
        {
            List<TaskEntity> taskEntities = await taskRepo.ReadByKeyAsync(projectId);
            List<TaskModel> tasks = new List<TaskModel>();

            foreach (var entity in taskEntities)
            {
                TaskModel task = Mapper.TaskEntityToObject(entity);
                tasks.Add(task);
            }

            return tasks;
        }

        public async Task<TaskModel> GetTask(int id)
        {
            TaskEntity taskEntity = await taskRepo.ReadAsync(id);
            TaskModel task = Mapper.TaskEntityToObject(taskEntity);

            return task;
        }

        public async Task UpdateTask(int id, TaskBaseModel task)
        {
            TaskEntity taskEntity = Mapper.TaskBaseObjectToEntity(task);
            taskEntity.PartitionKey = id.ToString();
            await taskRepo.UpdateAsync(taskEntity);
        }
        public async Task DeleteTask(int id)
        {
            TaskEntity taskEntity = await taskRepo.ReadAsync(id);
            await taskRepo.DeleteAsync(taskEntity);
        }

        private async Task<List<int>> GetProjectTasks(int projectId)
        {
            ProjectEntity projectEntity = await projectRepo.ReadAsync(projectId);
            List<int> projectTasks = MySerializer.StringToList(projectEntity.Tasks);
            return projectTasks;
        }

        private async Task UpdateProjectTasks(int projectId, List<int> projectTasks)
        {
            ProjectEntity projectEntity = await projectRepo.ReadAsync(projectId);
            projectEntity.Tasks = MySerializer.ListToString(projectTasks);
            await projectRepo.UpdateAsync(projectEntity);
        }

        public async Task UpdateTaskState(int taskId, TaskState taskState)
        {
            TaskEntity taskEntity = await taskRepo.ReadAsync(taskId);
            if (taskEntity.State == (int)taskState)
                return;

            taskEntity.State = (int)taskState;
            await taskRepo.UpdateAsync(taskEntity);
        }
    }
}
