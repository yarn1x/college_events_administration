using college_events_admin_API.Models;
using college_events_admin_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace college_events_admin_API.Controllers
{
    [ApiController]
    [Route("/college/admin/auth")]
    public class ControllerAuthorization(SutrEventsDbContext db, AuthorizationService service) : Controller
    {
        SutrEventsDbContext _db = db;
        AuthorizationService _service = service;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest user)
        {
            try
            {
                var db_response = _db.UserUsertypes
                    .FirstOrDefault(u => u.TypeId == 1 && u.Login.Login == user.login && u.Login.PasswordHash == user.passwordHash);

                if (db_response == null) return Unauthorized("Invalid credentials");
                
                var jwt = _service.GenerateJwtToken(user);

                return Ok(new {Token = jwt, ExpiresIn = 120});
            }
            catch
            {
                return BadRequest();
            }
        }
    }

    public class LoginRequest
    {
        public required string login { get; set; }
        public required string passwordHash { get; set; }
    }
}
