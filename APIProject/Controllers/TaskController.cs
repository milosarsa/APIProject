using Microsoft.AspNetCore.Mvc;
using System.Net;
using Interfaces.Service;
using Entities.Model;
using Microsoft.AspNetCore.Authorization;

namespace APIProject.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(Policy = "User")]
    public class TaskController : Controller
    {
        ILogger<TaskController> logger;
        ITaskService taskService;
        public TaskController(ITaskService _taskService, ILogger<TaskController> _logger)
        {
            taskService = _taskService ?? throw new ArgumentNullException(nameof(taskService));
            logger = _logger ?? throw new ArgumentNullException(nameof(logger));
        }
        //Responsible for data presentation and input
        [HttpPost("Project/{projectId}/Task")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> CreateTask(int projectId, [FromBody] TaskBaseModel task)
        {
            try
            {
                if (task == null)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await taskService.CreateTask(projectId, task);

                return Created("", task);
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

        [HttpGet("Tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                List<TaskModel> tasks = await taskService.GetAllTasks();
                return Ok(tasks);
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

        [HttpGet("Project/{projectId}/Tasks")]
        public async Task<IActionResult> GetAllTasksByProject(int projectId)
        {
            try
            {
                List<TaskModel> tasks = await taskService.GetAllTasksByProject(projectId);
                return Ok(tasks);
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

        [HttpGet("Task/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                TaskModel task = await taskService.GetTask(id);
                return Ok(task);
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

        [HttpPut("Project/{projectId}/Task/{id}")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> UpdateTask(int projectId,int id, [FromBody] TaskBaseModel task)
        {
            try
            {
                if (task == null)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await taskService.UpdateTask(projectId, id, task);
                return Ok("Task updated!");
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

        [HttpPut("TaskProject/{id}/UpdateState")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> UpdateTaskState(int id, [FromForm] TaskState taskState)
        {
            try
            {
                if ((int)taskState < 0 || (int)taskState > 3)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await taskService.UpdateTaskState(id, taskState);
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


        [HttpDelete("Task/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await taskService.DeleteTask(id);
                return Ok("Task deleted");
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
    }
}
