using college_events_admin_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace college_events_admin_API.Services
{
    public class EventsService(SutrEventsDbContext db, ILogger<EventsService> logger)
    {
        private readonly SutrEventsDbContext _db = db;
        private readonly ILogger<EventsService> _logger = logger;


        public List<Event> GetFullList() => [.. _db.Events];

        public List<Event> GetListByStatus(int Id) => [.. _db.Events.Where(s => s.StatusId == Id)];


        public int UpdateExpiredEvents()
        {
            DateTime realTime = DateTime.Now;

            var expired_events = _db.Events
                .Where(e => e.StatusId == 2 && e.Datetime.AddMinutes(e.Duration) <= realTime)
                .ToList();

            if (expired_events.Any())
            {
                foreach (Event expired in expired_events)
                {
                    _logger?.LogInformation($"Обновление: {expired.Title} (ID: {expired.EventId})");
                    expired.StatusId = 3;
                }

                int savedCount = _db.SaveChanges();
                return savedCount;
            }

            return 0;
        }
    }
}
