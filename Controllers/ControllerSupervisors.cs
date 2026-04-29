using college_events_admin_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace college_events_admin_API.Controllers
{
    [ApiController]
    [Route("/college/admin/supervisors")]
    public class ControllerSupervisors(SutrEventsDbContext db) : Controller
    {
        SutrEventsDbContext _db = db;

        [HttpGet]
        public ActionResult GETSupervisorList()
        {
            var arr = _db.UserUsertypes.Select(u => new
            {
                u.TypeId,
                u.Login.LoginId,
                u.Login.Username.FirstName,
                u.Login.Username.LastName,
                u.Login.Username.MiddleName,
                u.Login.Email,
                u.Login.MobilePhone,
                groups = _db.Groups.Select(g => new 
                { 
                    g.LoginId,
                    g.Name,
                }).Where(g => g.LoginId == u.Login.LoginId).ToList()
            })
            .Where(uid => uid.TypeId == 2)
            .ToList();

            return Ok(arr);
        }
    }
}
