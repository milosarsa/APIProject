namespace Service
{
    //Responsible for data manipulation
    public class ProjectService : IProjectService
    {
        private ILogger<ProjectService> logger;
        private IProjectRepo projectRepo;
        private IMemoryCache memoryCache;

        public ProjectService(IProjectRepo _projectRepo, ILogger<ProjectService> _logger, IMemoryCache _memoryCache)
        {
            projectRepo = _projectRepo ?? throw new ArgumentNullException(nameof(projectRepo));
            logger = _logger ?? throw new ArgumentNullException(nameof(logger));
            memoryCache = _memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        //Takes ProjectBase as input to omit using Id and Tasks
        public async Task CreateProject(ProjectBaseModel project)
        {
            //Add logic to create new project with new id (incrementing id)
            ProjectEntity projectEntity = Mapper.ProjectBaseObjectToEntity(project, true);
            await projectRepo.CreateAsync(projectEntity);
        }

        public async Task<List<ProjectModel>> GetAllProjects()
        {
            List<ProjectEntity> projectEntities = await projectRepo.ReadAllAsync();
            List<ProjectModel> projects = new List<ProjectModel>();

            foreach (var entity in projectEntities)
            {
                ProjectModel project = Mapper.ProjectEntityToObject(entity);
                projects.Add(project);
            }

            return projects;
        }

        public async Task<ProjectModel> GetProject(int id)
        {
            ProjectEntity projectEntity = await projectRepo.ReadAsync(id);
            ProjectModel project = Mapper.ProjectEntityToObject(projectEntity);

            return project;
        }

        //Takes ProjectBase as input to omit using Id and Tasks
        public async Task UpdateProject(int id, ProjectUpdateModel project)
        {
            ProjectEntity projectEntity = await projectRepo.ReadAsync(id);
            projectEntity.FromProjectUpdateModel(project);
            await projectRepo.UpdateAsync(projectEntity);
        }

        public async Task DeleteProject(int id)
        {
            ProjectEntity projectEntity = await projectRepo.ReadAsync(id);
            await projectRepo.DeleteAsync(projectEntity);
        }

        public async Task UpdateProjectState(int projectId, ProjectState projectState)
        {
            ProjectEntity projectEntity = await projectRepo.ReadAsync(projectId);
            if (projectEntity.State == (int)projectState)
                return;

            projectEntity.State = (int)projectState;
            await projectRepo.UpdateAsync(projectEntity);
        }
    }
}
