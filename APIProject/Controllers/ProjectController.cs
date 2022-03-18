using Microsoft.AspNetCore.Mvc;
using System.Net;
using Interfaces.Service;
using Entities.Model;
using Microsoft.AspNetCore.Authorization;

namespace APIProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "User")]
    public class ProjectController : Controller
    {
        ILogger<ProjectController> logger;
        IProjectService projectService;
        public ProjectController(IProjectService _projectService, ILogger<ProjectController> _logger)
        {
            projectService = _projectService ?? throw new ArgumentNullException(nameof(projectService));
            logger = _logger ?? throw new ArgumentNullException(nameof(logger));
        }
        //Responsible for data presentation and input
        [HttpPost("Project")]
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

                return Created("",project);
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

        [HttpGet("Projects")]
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

        [HttpGet("Project/{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            try
            {
                ProjectModel project = await projectService.GetProject(id);
                return Ok(project);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Task failed during data access\n" +
                    ex.Message);
                HttpStatusCode statusCode;
                if(ex.StatusCode.HasValue)
                {
                    statusCode = ex.StatusCode.Value;
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                return StatusCode((int)statusCode, ex.Message) ;
            }
            catch(Exception ex)
            {
                logger.LogError("Task execution failed\n" +
                    ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("Project/{id}")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectBaseModel project)
        {
            try
            {
                if (project == null)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await projectService.UpdateProject(id, project);
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
        [HttpPut("Project/{id}/UpdateState")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> UpdateProjectState(int id, [FromForm] ProjectState projectState)
        {
            try
            {
                if ((int)projectState < 0 || (int)projectState > 3)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await projectService.UpdateProjectState(id, projectState);
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

        [HttpDelete("Project/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await projectService.DeleteProject(id);
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
