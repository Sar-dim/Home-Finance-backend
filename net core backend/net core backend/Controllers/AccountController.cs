using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using net_core_backend.Models;
using net_core_backend.Repositories.Interfaces;
using net_core_backend.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace net_core_backend.Controllers
{
    public class AccountController : Controller
    {
        private readonly IBaseRepository<Person> _person;
        private readonly IPersonService _personService;

        public AccountController(IPersonService personService, IBaseRepository<Person> person)
        {
            _person = person;
            _personService = personService;
        }

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            var identity = _personService.GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }


    }
}
