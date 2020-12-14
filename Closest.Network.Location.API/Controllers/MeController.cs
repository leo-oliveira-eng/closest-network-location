using Microsoft.AspNetCore.Mvc;

namespace Closest.Network.Location.API.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class MeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { name = "Closest Network Location", version = "0.1" });
    }
}
