using Entities.Model;
using Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using MyLog;

namespace APIProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "User")]
    public class ProjectController : Controller
    {
        ILogger<ProjectController> logger;
        IProjectService projectService;
        IMyLogger myLogger;
        public ProjectController(IProjectService _projectService, ILogger<ProjectController> _logger, IMyLogger _myLogger)
        {
            projectService = _projectService ?? throw new ArgumentNullException(nameof(projectService));
            logger = _logger ?? throw new ArgumentNullException(nameof(logger));
            myLogger = _myLogger ?? throw new ArgumentNullException(nameof(myLogger));
        }
        //Responsible for data presentation and input
        [HttpPost("Create")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectBaseModel project)
        {
            try
            {
                if (project == null)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await projectService.CreateProject(project);

                return Created("", project);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Task failed during data access\n" +
                    ex.Message);
                HttpStatusCode statusCode;
                if (ex.StatusCode.HasValue)
                {
                    statusCode = ex.StatusCode.Value;
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                return StatusCode((int)statusCode, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("Task execution failed\n" +
                    ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet("AllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                List<ProjectModel> projects = await projectService.GetAllProjects();
                return Ok(projects);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Task failed during data access\n" +
                    ex.Message);
                HttpStatusCode statusCode;
                if (ex.StatusCode.HasValue)
                {
                    statusCode = ex.StatusCode.Value;
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                return StatusCode((int)statusCode, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("Task execution failed\n" +
                    ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProject(int projectId)
        {
            try
            {
                ProjectModel project = await projectService.GetProject(projectId);
                return Ok(project);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Task failed during data access\n" +
                    ex.Message);
                HttpStatusCode statusCode;
                if (ex.StatusCode.HasValue)
                {
                    statusCode = ex.StatusCode.Value;
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                return StatusCode((int)statusCode, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("Task execution failed\n" +
                    ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{projectId}/Update")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> UpdateProject(int projectId, [FromBody] ProjectBaseModel project)
        {
            try
            {
                if (project == null)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await projectService.UpdateProject(projectId, project);
                return Ok("Project updated!");
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Task failed during data access\n" +
                    ex.Message);
                HttpStatusCode statusCode;
                if (ex.StatusCode.HasValue)
                {
                    statusCode = ex.StatusCode.Value;
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                return StatusCode((int)statusCode, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("Task execution failed\n" +
                    ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPut("{projectId}/UpdateState")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> UpdateProjectState(int projectId, [FromForm] ProjectState projectState)
        {
            try
            {
                if ((int)projectState < 0 || (int)projectState > 3)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await projectService.UpdateProjectState(projectId, projectState);
                return Ok("Project updated!");
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Task failed during data access\n" +
                    ex.Message);
                HttpStatusCode statusCode;
                if (ex.StatusCode.HasValue)
                {
                    statusCode = ex.StatusCode.Value;
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                return StatusCode((int)statusCode, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("Task execution failed\n" +
                    ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{projectId}/Delete")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await projectService.DeleteProject(projectId);
                return Ok("Project deleted");
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Task failed during data access\n" +
                    ex.Message);
                HttpStatusCode statusCode;
                if (ex.StatusCode.HasValue)
                {
                    statusCode = ex.StatusCode.Value;
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                return StatusCode((int)statusCode, ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("Task execution failed\n" +
                    ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
