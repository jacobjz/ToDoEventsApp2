using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ToDoEvents.DbModels;
using ToDoEvents.Models;

namespace ToDoEvents.Repo
{
    public class EFService : IEfInterface
    {
        private EFContext db = new EFContext();
        private static log4net.ILog Log { get; set; }

        ILog log = log4net.LogManager.GetLogger(typeof(EFService));
        public Event GetEventById(int? id)
        {
            Event @event =null;
            try {
           @event = db.Events.Find(id);
                  }
            catch(Exception e){
                log.Error($"error:{e.Message}");
            }
            if(@event == null)
            {
                log.Debug($"no entry with id[{id}]");
            }
            return @event;
        }
        public void CreateEvent(Event @event)
        {
            try
            {
                @event.EventStatus = GetEventStatusByType("scheduled");
                db.Events.Add(@event);
                db.SaveChanges();
                log.Debug($"Created event[id: {@event.EventId} ] about: {@event.Description} at: {@event.DateTime}");
            }
            catch(Exception e)
            {
                log.Error($"error:{e.Message}");
            }
         
        }
        public void EditEvent(Event @event)
        {
            try
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                log.Debug($"Edited event[id: {@event.EventId} ] about: {@event.Description} at: {@event.DateTime}");
            }
            catch(Exception e)
            {
                log.Error($"error:{e.Message}");
            }

        }
        public void DeleteEvent(Event @event)
        {
            try
            {
                db.Events.Remove(@event);
                db.SaveChanges();
                log.Debug($"Deleted event[id: {@event.EventId} ] about: {@event.Description} at: {@event.DateTime}");
            }
            catch (Exception e)
            {
                log.Error($"error:{e.Message}");
            }

        }
        public EventStatus GetEventStatusByType(string type)
        {
            EventStatus es = null;
            try {
                es= db.EventStatuses.FirstOrDefault(x => x.Status == type);
            }
            catch(Exception e){
                log.Error($"error:{e.Message}");
            }
           return es;
        }

        public List<Event> GetListOfEvents()
        {
            List<Event> le = null;
            try {
                le= db.Events.ToList();
            } catch(Exception e)
            {
                log.Error($"error:{e.Message}");

            }
            return le;
        }
    }
}