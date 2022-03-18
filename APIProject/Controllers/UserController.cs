using Entities.Auth;
using Interfaces.Service;
using Entities.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private ILogger<UserController> logger;
        private IUserService userService;

        //User controller is not yet provide a way to handle user data
        //And is only checking if user data is valid(please check Repo/UserRepo.cs for user data)

        public UserController(IUserService _userService, ILogger<UserController> _logger)
        {
            logger = _logger ?? throw new ArgumentNullException(nameof(logger));
            userService = _userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromForm] UserCreds userCreds)
        {
            if (userCreds == null)
                return BadRequest("Credentials can not be empty");
            if (!ModelState.IsValid)
                return BadRequest("Invalid model state");
            UserAuth userAuth;
            if (!userService.Authenticate(userCreds, out userAuth))
                return Unauthorized("Provided login info does not match any user.");

            return Ok(userAuth);
        }

        [HttpGet("Verify")]
        [Authorize]
        public IActionResult VerifyToken()
        {
            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value ?? "Unauthorized";
            return Ok($"Successfully authorized!\nUser access level {userRole}");
        }
    }
}
