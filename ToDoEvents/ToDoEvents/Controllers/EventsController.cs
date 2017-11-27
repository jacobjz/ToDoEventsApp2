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
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading.Tasks;
using Calendar.ASP.NET.MVC5;

namespace ToDoEvents.Controllers
{
   // [Authorize]
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
        public async Task<ActionResult> Create([Bind(Include = "EventId,Description,DateTime,EndTime")] Event @event)
        {
            var service = await GetService();   

            if (ModelState.IsValid)
            {
                var googleEvent = new Google.Apis.Calendar.v3.Data.Event
                {
                    Description = @event.Description,
                    Summary = @event.Description,
                    Start = new Google.Apis.Calendar.v3.Data.EventDateTime
                    {
                        DateTime = @event.DateTime

                    },
                    End = new Google.Apis.Calendar.v3.Data.EventDateTime
                    {
                        DateTime = @event.EndTime
                    }
                    
                };
                var result = service.Events.Insert(googleEvent,"primary").Execute();
                @event.GoogleId = result.Id;
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
        public ActionResult Edit([Bind(Include = "EventId,Description,DateTime,EndTime,EventStatusId")] Event @event)
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
        private readonly IDataStore dataStore = new FileDataStore(GoogleWebAuthorizationBroker.Folder);

        private async Task<UserCredential> GetCredentialForApiAsync()
        {
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = MyClientSecrets.ClientId,
                    ClientSecret = MyClientSecrets.ClientSecret,
                },
                Scopes = MyRequestedScopes.Scopes,
            };
            var flow = new GoogleAuthorizationCodeFlow(initializer);

            var identity = await HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(
                DefaultAuthenticationTypes.ApplicationCookie);
            var userId = identity.FindFirstValue(MyClaimTypes.GoogleUserId);

            var token = await dataStore.GetAsync<TokenResponse>(userId);
            return new UserCredential(flow, userId, token);
        }
        public async Task<CalendarService> GetService()
        {
            var credential = await GetCredentialForApiAsync();

            var initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ASP.NET MVC5 Calendar Sample",
            };
            var service = new CalendarService(initializer);

            return service;
        }
        // GET: /Calendar/UpcomingEvents
        public async Task<ActionResult> UpcomingEvents()
        {
       
            return View();
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
