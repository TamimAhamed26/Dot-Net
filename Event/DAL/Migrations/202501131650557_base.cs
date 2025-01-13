namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _base : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Speakers", "EventId", "dbo.Events");
            DropIndex("dbo.Speakers", new[] { "EventId" });
            AlterColumn("dbo.Speakers", "EventId", c => c.Int());
            CreateIndex("dbo.Speakers", "EventId");
            AddForeignKey("dbo.Speakers", "EventId", "dbo.Events", "EventId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Speakers", "EventId", "dbo.Events");
            DropIndex("dbo.Speakers", new[] { "EventId" });
            AlterColumn("dbo.Speakers", "EventId", c => c.Int(nullable: false));
            CreateIndex("dbo.Speakers", "EventId");
            AddForeignKey("dbo.Speakers", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
        }
    }
}
