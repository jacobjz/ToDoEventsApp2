using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ToDoEvents.DbModels;

namespace ToDoEvents.Models
{
    public class EFContext : ApplicationDbContext
    {
        public DbSet<Event> Events { get; set;}
        public DbSet<EventStatus> EventStatuses { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}