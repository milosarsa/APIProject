namespace Interfaces.Repo
{
    public interface ITaskRepo
    {
        public Task<int> CreateAsync(TaskEntity entity);
        public Task<List<TaskEntity>> ReadAllAsync();
        public Task<List<TaskEntity>> ReadByKeyAsync(int key);
        public Task<TaskEntity> ReadAsync(int id);
        public Task UpdateAsync(TaskEntity entity);
        public Task DeleteAsync(Entity entity);
    }
}
