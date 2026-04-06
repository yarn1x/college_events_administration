using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace college_events_desktop.DataModels
{
    public class EventGroups : IDataErrorInfo
    {
        public int eventId { get; set; }
        public string name { get; set; }
        public string supervisorName { get; set; }
        public string supervisorSurname { get; set; }
        public string supervisorLastname { get; set; }
        public int expectedListenersCount { get; set; }
        public int expectedParticipantsCount { get; set; }
        public int expectedSuperParticipantsCount { get; set; }
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "expectedListenersCount":
                        if (expectedListenersCount < 0)
                        {
                            error += "Число слушателей не может быть отрицательным! ";
                        }
                        break;
                    case "expectedParticipantsCount":
                        if (expectedParticipantsCount < 0)
                        {
                            error += "Число участников не может быть отрицательным! ";
                        }
                        break;
                    case "expectedSuperParticipantsCount":
                        if (expectedParticipantsCount < 0)
                        {
                            error += "Число супер-участников не может быть отрицательным! ";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
}
