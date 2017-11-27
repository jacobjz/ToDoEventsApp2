using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoEvents.DbModels
{
    public class Event
    {
        public int EventId { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }

        public DateTime EndTime { get; set; }
        public virtual EventStatus EventStatus { get; set; }
        public int EventStatusId { get; set; }
        public string GoogleId { get; set; }
    }
}