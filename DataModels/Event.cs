using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace college_events_desktop.DataModels
{
    public class Event
    {
        public int eventId { get; set; }
        public string title { get; set; }
        public string startDate { get; set; }
        public string startTime { get; set; }
        public int duration { get; set; }
        public string endDate { get; set; }
        public string endTime { get; set; }
        public string description { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public string place { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }
        public string organizerName { get; set; }
        public string organizerSurname { get; set; }
        public string organizerLastname { get; set; }
        public string additionalInfo { get; set; }
        public int maxListenersCount { get; set; }
        public int maxParticipantsCount { get; set; }
    }
}
