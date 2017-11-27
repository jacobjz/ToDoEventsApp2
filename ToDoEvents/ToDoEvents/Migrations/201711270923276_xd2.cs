namespace ToDoEvents.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xd2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "EndTime");
        }
    }
}
