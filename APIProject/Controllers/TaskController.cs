using Entities.Model;
using Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
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
        [HttpPost("Create")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> CreateTask([FromBody] TaskBaseModel task)
        {
            try
            {
                if (task == null)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await taskService.CreateTask(task);

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

        [HttpGet("AllTasks")]
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

        [HttpGet("Project/{projectId}")]
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

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTask(int taskId)
        {
            try
            {
                TaskModel task = await taskService.GetTask(taskId);
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

        [HttpPut("{taskId}/Update")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskUpdateModel task)
        {
            try
            {
                if (task == null)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await taskService.UpdateTask(taskId, task);
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

        [HttpPut("{taskId}/UpdateState")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> UpdateTaskState(int taskId, [FromForm] TaskState taskState)
        {
            try
            {
                if ((int)taskState < 0 || (int)taskState > 5)
                    return BadRequest("Object sent is null.");

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await taskService.UpdateTaskState(taskId, taskState);
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


        [HttpDelete("{taskId}/Delete")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                await taskService.DeleteTask(taskId);
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
