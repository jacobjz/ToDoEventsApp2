namespace ToDoEvents.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "GoogleId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "GoogleId");
        }
    }
}
