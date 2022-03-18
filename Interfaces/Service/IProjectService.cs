using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Service
{
    public interface IProjectService
    {
        public Task CreateProject(ProjectBaseModel project);
        public Task<List<ProjectModel>> GetAllProjects();
        public Task<ProjectModel> GetProject(int id);
        public Task UpdateProject(int id, ProjectBaseModel project);
        public Task DeleteProject(int id);

        public Task UpdateProjectState(int projectId, ProjectState projectState);
    }
}
