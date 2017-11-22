using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoEvents.DbModels;

namespace ToDoEvents.Repo
{
    interface IEfInterface
    {
         EventStatus GetEventStatusByType(string type);
        List<Event> GetListOfEvents();
        Event GetEventById(int? id);
        void CreateEvent(Event @event);
        void EditEvent(Event @event);
        void DeleteEvent(Event @event);
    }
}
