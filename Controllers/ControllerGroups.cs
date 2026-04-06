using college_events_admin_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace college_events_admin_API.Controllers
{
    [ApiController]
    [Route("/college/admin/groups")]
    public class ControllerGroups(SutrEventsDbContext db) : Controller
    {
        SutrEventsDbContext _db = db;

        [HttpGet]
        public ActionResult GETGroupList()
        {
            var arr = _db.Groups.Select(g => new
            {
                GroupName = g.Name,
                SupervisorName = g.Login.Username.FirstName,
                SupervisorSurname = g.Login.Username.LastName,
                SupervisorMiddlename = g.Login.Username.MiddleName,
                SupervisorEmail = g.Login.Email,
                SupervisorPhone = g.Login.MobilePhone,
            })
            .ToList();

            return Ok(arr);
        }

        [HttpGet("{GroupID}/statistic")]
        public ActionResult GETGroupEventsOnlyStatistic()
        {
            var arr = _db.ActualAttendances.Select(eg => new
            {
                eg.EventGroup.EventId,
                eg.EventGroup.Event.Title,
                eg.EventGroup.Group.Name,
                SupervisorName = eg.EventGroup.Group.Login.Username.FirstName,
                SupervisorSurname = eg.EventGroup.Group.Login.Username.LastName,
                SupervisorMiddlename = eg.EventGroup.Group.Login.Username.MiddleName,
            });
            return Ok(arr);
        }
    }
}
