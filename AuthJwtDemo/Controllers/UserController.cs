using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthJwtDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Authorize]
        [HttpGet("user")]
        public IActionResult GetUser()
        {
            return Ok($"Welcome User : {User.Identity?.Name}");
        }

        [Authorize (Roles ="Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdmin()
        {
            return Ok("This is for Admin only.");
        }
    }
}
