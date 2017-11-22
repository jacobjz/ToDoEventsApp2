using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToDoEvents.DbModels;
using ToDoEvents.Models;
using ToDoEvents.Repo;

namespace ToDoEvents.Controllers
{
    public class EventsController : Controller
    {
        private EFContext db = new EFContext();
        private EFService ef = new EFService();
        private static log4net.ILog Log { get; set; }

       ILog log = log4net.LogManager.GetLogger(typeof(EventsController));
        // GET: Events
        public ActionResult Index()
        {
          
            return View(ef.GetListOfEvents());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            return View(ef.GetEventById(id));
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventId,Description,DateTime")] Event @event)
        {
            
            if (ModelState.IsValid)
            {
                ef.CreateEvent(@event);                
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewData["EventStatusDropDown"] = new SelectList(db.EventStatuses, "EventStatusId", "Status");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = ef.GetEventById(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,Description,DateTime,EventStatusId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                ef.EditEvent(@event);
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = ef.GetEventById(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = ef.GetEventById(id);
            ef.DeleteEvent(@event);
            return RedirectToAction("Index");
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
