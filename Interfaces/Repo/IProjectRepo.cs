namespace Interfaces.Repo
{
    public interface IProjectRepo
    {
        public Task CreateAsync(ProjectEntity entity);

        public Task<List<ProjectEntity>> ReadAllAsync();
        public Task<ProjectEntity> ReadAsync(int id);
        public Task UpdateAsync(ProjectEntity entity);
        public Task DeleteAsync(Entity entity);

        public Task<bool> Exists(string value, string key = "PartitionKey");
    }
}
