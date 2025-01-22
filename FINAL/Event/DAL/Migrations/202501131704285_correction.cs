namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Speakers", "Speakers_SpeakerId", "dbo.Speakers");
            DropForeignKey("dbo.Speakers", "EventId", "dbo.Events");
            DropIndex("dbo.Speakers", new[] { "EventId" });
            DropIndex("dbo.Speakers", new[] { "Speakers_SpeakerId" });
            AlterColumn("dbo.Speakers", "EventId", c => c.Int(nullable: false));
            CreateIndex("dbo.Speakers", "EventId");
            AddForeignKey("dbo.Speakers", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
            DropColumn("dbo.Speakers", "Speakers_SpeakerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Speakers", "Speakers_SpeakerId", c => c.Int());
            DropForeignKey("dbo.Speakers", "EventId", "dbo.Events");
            DropIndex("dbo.Speakers", new[] { "EventId" });
            AlterColumn("dbo.Speakers", "EventId", c => c.Int());
            CreateIndex("dbo.Speakers", "Speakers_SpeakerId");
            CreateIndex("dbo.Speakers", "EventId");
            AddForeignKey("dbo.Speakers", "EventId", "dbo.Events", "EventId");
            AddForeignKey("dbo.Speakers", "Speakers_SpeakerId", "dbo.Speakers", "SpeakerId");
        }
    }
}
