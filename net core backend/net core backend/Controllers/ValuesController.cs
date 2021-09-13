using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace net_core_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        [Authorize]
        [Route("getlogin")]
        public IActionResult GetLogin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var personId = identity.FindFirst("PersonId").Value;
                return Ok($"Your login: {User.Identity.Name}, {personId}");
            }
            else
            {
                return Ok("Person not found");
            }

        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("getrole")]
        public IActionResult GetRole()
        {
            return Ok("Your role is admin");
        }
    }
}
