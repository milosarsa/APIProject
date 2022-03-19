using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData;
using MyLog;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        IMyLogger myLogger;

        public TestController(IMyLogger _myLogger)
        {
            myLogger = _myLogger ?? throw new ArgumentNullException(nameof(myLogger));
        }
        //Using this to test new implementations
        [HttpPost("Log")]
        public IActionResult TestLog([FromForm] string? message)
        {
            myLogger.Info(message);
            return Ok();
        }
    }
}
