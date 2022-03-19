
using System.Text;
using Entities.Enums;

namespace Service
{
    static class Mapper
    {
        public static ProjectModel ProjectEntityToObject(ProjectEntity projectEntity)
        {
            ProjectModel project = new ProjectModel();
            project.Id = int.Parse(projectEntity.PartitionKey);
            project.Name = projectEntity.RowKey;
            project.Description = projectEntity.Description;
            project.Tasks = MySerializer.StringToList(projectEntity.Tasks);
            project.Code = projectEntity.Code;
            project.State = Enum.GetName<ProjectState>((ProjectState)projectEntity.State);
            return project;
        }

        public static ProjectEntity ProjectBaseObjectToEntity(ProjectBaseModel project, bool newProject = false)
        {
            ProjectEntity projectEntity = new ProjectEntity();
            projectEntity.RowKey = project.Name;
            projectEntity.Description = project.Description;
            projectEntity.Tasks = MySerializer.ListToString(new List<int>());
            
            if (newProject)
            {
                projectEntity.Code = String.Empty;
                projectEntity.State = 0;
            }

            return projectEntity;
        }
        public static TaskModel TaskEntityToObject(TaskEntity taskEntity)
        {
            TaskModel task = new TaskModel();
            task.Id = int.Parse(taskEntity.PartitionKey);
            task.ProjectId = int.Parse(taskEntity.RowKey);
            task.Name = taskEntity.Name;
            task.Description = taskEntity.Description;
            task.State = Enum.GetName<TaskState>((TaskState)taskEntity.State);
            return task;
        }

        public static TaskEntity TaskBaseObjectToEntity(TaskBaseModel taskBase, bool newTask = false)
        {
            TaskEntity taskEntity = new TaskEntity();
            taskEntity.Name = taskBase.Name;
            taskEntity.Description = taskBase.Description;
            if(newTask)
                taskEntity.State = 0;
            return taskEntity;
        }
    }

    public static class MySerializer
    {
        public static List<int> StringToList(string inputString)
        {
            if (inputString.Length > 0)
            {
                string[] stringArray = inputString.Split(",");
                List<int> list = Array.ConvertAll(stringArray, int.Parse).ToList();
                return list;
            }
            else
                return new List<int>();
        }

        public static string ListToString(List<int> inputList)
        {
            if (inputList.Any())
            {
                StringBuilder stringBuilder = new StringBuilder();
                int[] intArray = inputList.ToArray();
                string intList = String.Join(",", intArray);
                return intList;
            }
            else
                return String.Empty;
        }
    }
}
