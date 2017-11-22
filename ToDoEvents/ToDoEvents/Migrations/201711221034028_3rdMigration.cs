namespace ToDoEvents.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3rdMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "EventStatus_EventStatusId", "dbo.EventStatus");
            DropIndex("dbo.Events", new[] { "EventStatus_EventStatusId" });
            RenameColumn(table: "dbo.Events", name: "EventStatus_EventStatusId", newName: "EventStatusId");
            AlterColumn("dbo.Events", "EventStatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.Events", "EventStatusId");
            AddForeignKey("dbo.Events", "EventStatusId", "dbo.EventStatus", "EventStatusId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "EventStatusId", "dbo.EventStatus");
            DropIndex("dbo.Events", new[] { "EventStatusId" });
            AlterColumn("dbo.Events", "EventStatusId", c => c.Int());
            RenameColumn(table: "dbo.Events", name: "EventStatusId", newName: "EventStatus_EventStatusId");
            CreateIndex("dbo.Events", "EventStatus_EventStatusId");
            AddForeignKey("dbo.Events", "EventStatus_EventStatusId", "dbo.EventStatus", "EventStatusId");
        }
    }
}
