using Microsoft.AspNetCore.Mvc;

namespace limitR.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
