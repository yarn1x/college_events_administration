using college_events_admin_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace college_events_admin_API.Controllers
{
    [Authorize]
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
                g.GroupId,
                GroupName = g.Name,
                SupervisorName = g.Login.Username.FirstName,
                SupervisorSurname = g.Login.Username.LastName,
                SupervisorMiddlename = g.Login.Username.MiddleName,
                SupervisorEmail = g.Login.Email,
                SupervisorPhone = g.Login.MobilePhone,
                eventsCount = _db.ActualAttendances.Select(a => new
                {
                    a.EventGroup.GroupId,
                }).Count(a => a.GroupId == g.GroupId),
                categoriesCount = _db.ActualAttendances.Select(c => new
                {
                    c.EventGroup.GroupId,
                    c.EventGroup.Event.CategoryId,
                }).Where(c => c.GroupId == g.GroupId).GroupBy(co => co.CategoryId).Count()
            })
            .OrderBy(g => g.GroupName);

            return Ok(arr);
        }

        [HttpGet("{GroupID}/statistic")]
        public ActionResult GETGroupEventsOnlyStatistic(int GroupID)
        {
            var arr = _db.ActualAttendances.Select(eg => new
            {
                eg.EventGroup.GroupId,
                eg.EventGroup.EventId,
                eg.EventGroup.Event.Title,
                eg.EventGroup.Group.Name,
                SupervisorName = eg.EventGroup.Group.Login.Username.FirstName,
                SupervisorSurname = eg.EventGroup.Group.Login.Username.LastName,
                SupervisorMiddlename = eg.EventGroup.Group.Login.Username.MiddleName,
            })
            .Where(a => a.GroupId == GroupID);


            return Ok(arr);
        }
    }
}
