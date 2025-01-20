namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 10, unicode: false),
                        DeviceName = c.String(),
                        MetricType = c.String(),
                        Value = c.Double(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Username)
                .Index(t => t.Username);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 10, unicode: false),
                        Password = c.String(nullable: false, maxLength: 10, unicode: false),
                        Email = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.HealthGoals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 10, unicode: false),
                        GoalType = c.String(),
                        TargetValue = c.Double(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Username)
                .Index(t => t.Username);
            
            CreateTable(
                "dbo.HealthMetrics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 10, unicode: false),
                        MetricType = c.String(),
                        Value = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Username)
                .Index(t => t.Username);
            
            CreateTable(
                "dbo.SharedDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 10, unicode: false),
                        SharedWith = c.String(),
                        Token = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        IsRevoked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Username)
                .Index(t => t.Username);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        ExpiredAt = c.DateTime(),
                        Uname = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Uname)
                .Index(t => t.Uname);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceDatas", "Username", "dbo.Users");
            DropForeignKey("dbo.Tokens", "Uname", "dbo.Users");
            DropForeignKey("dbo.SharedDatas", "Username", "dbo.Users");
            DropForeignKey("dbo.HealthMetrics", "Username", "dbo.Users");
            DropForeignKey("dbo.HealthGoals", "Username", "dbo.Users");
            DropIndex("dbo.Tokens", new[] { "Uname" });
            DropIndex("dbo.SharedDatas", new[] { "Username" });
            DropIndex("dbo.HealthMetrics", new[] { "Username" });
            DropIndex("dbo.HealthGoals", new[] { "Username" });
            DropIndex("dbo.DeviceDatas", new[] { "Username" });
            DropTable("dbo.Tokens");
            DropTable("dbo.SharedDatas");
            DropTable("dbo.HealthMetrics");
            DropTable("dbo.HealthGoals");
            DropTable("dbo.Users");
            DropTable("dbo.DeviceDatas");
        }
    }
}
