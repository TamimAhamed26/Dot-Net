namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.Speakers",
                c => new
                    {
                        SpeakerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Topic = c.String(),
                        EventId = c.Int(nullable: false),
                        Speakers_SpeakerId = c.Int(),
                    })
                .PrimaryKey(t => t.SpeakerId)
                .ForeignKey("dbo.Speakers", t => t.Speakers_SpeakerId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.Speakers_SpeakerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Speakers", "EventId", "dbo.Events");
            DropForeignKey("dbo.Speakers", "Speakers_SpeakerId", "dbo.Speakers");
            DropIndex("dbo.Speakers", new[] { "Speakers_SpeakerId" });
            DropIndex("dbo.Speakers", new[] { "EventId" });
            DropTable("dbo.Speakers");
            DropTable("dbo.Events");
        }
    }
}
