using college_events_admin_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace college_events_admin_API.Controllers
{
    [ApiController]
    [Route("/college/admin/organizers")]
    public class ControllerOrganizers(SutrEventsDbContext db) : Controller
    {
        SutrEventsDbContext _db = db;

        [HttpGet]
        public ActionResult GETOrganizersList()
        {
            var arr = _db.UserUsertypes.Select(u => new
            {
                u.Login.Username.FirstName,
                u.Login.Username.LastName,
                u.Login.Username.MiddleName,
                u.TypeId,
                u.Type.TypeName,
                u.Login.Email,
                u.Login.MobilePhone,
            })
                .Where(t => t.TypeId == 3)
            .ToList();

            return Ok(arr);
        }
    }
}
