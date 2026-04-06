using college_events_admin_API.Models;
using college_events_admin_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace college_events_admin_API.Controllers
{
    [ApiController]
    [Route("/college/admin/events")]
    public class ControllerEvents(SutrEventsDbContext db) : Controller
    {
        private readonly SutrEventsDbContext _db = db;


        [HttpGet]
        public ActionResult GETEventList()
        {
            var arr = _db.Events
                .Select(e => new
                {
                    e.EventId,

                    e.Title,
                    StartDate = e.Datetime.ToString("dd.MM.yyyy"),
                    StartTime = e.Datetime.ToString("HH:mm"),
                    e.Duration,
                    EndDate = e.Datetime.AddMinutes(e.Duration).ToString("dd.MM.yyyy"),
                    EndTime = e.Datetime.AddMinutes(e.Duration).ToString("HH:mm"),
                    Desctiption = e.FullDescription ?? e.ShortDescription,

                    e.CategoryId,
                    e.Category.CategoryName,
                    e.Location.Place,
                    e.StatusId,
                    e.Status.StatusName,

                    OrganizerName = e.Organizer.Username.FirstName,
                    OrganizerSurname = e.Organizer.Username.LastName,
                    OrganizerLastname = e.Organizer.Username.MiddleName,

                    e.AdditionalInfo,
                    e.MaxListenersCount,
                    e.MaxParticipantsCount
                })
                .ToList();

            return Ok(arr);
        }

        [HttpGet("categories")]
        public ActionResult GETCategoryList()
        {
            var arr = _db.Categories
                .Select(c => new
                {
                    c.CategoryId,
                    c.CategoryName
                })
                .ToList();
            return Ok(arr);
        }

        [HttpGet("{EventId}/groups")]
        public ActionResult GETEventGroups(int EventId)
        {
            var arr = _db.EventGroups.Select(e => new
            {
                e.EventId,
                e.Group.Name,
                SupervisorName = e.Group.Login.Username.FirstName,
                SupervisorSurname = e.Group.Login.Username.LastName,
                SupervisorLastname = e.Group.Login.Username.MiddleName,
                e.ExpectedListenersCount,
                e.ExpectedParticipantsCount,
                e.ExpectedSuperParticipantsCount,
            })
            .Where(e => e.EventId == EventId)
            .ToList();

            return Ok(arr);
        }

        [HttpGet("places")]
        public ActionResult<List<Location>> GETPlaceList()
        {
            var arr = _db.Locations.ToList();
            return Ok(arr);
        }
    }
}
