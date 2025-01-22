namespace APICRUD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Author_AuthorId", "dbo.Authors");
            DropIndex("dbo.Books", new[] { "Author_AuthorId" });
            DropColumn("dbo.Books", "AuthorId");
            RenameColumn(table: "dbo.Books", name: "Author_AuthorId", newName: "AuthorId");
            AlterColumn("dbo.Books", "AuthorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Books", "AuthorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "AuthorId");
            AddForeignKey("dbo.Books", "AuthorId", "dbo.Authors", "AuthorId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Books", new[] { "AuthorId" });
            AlterColumn("dbo.Books", "AuthorId", c => c.Int());
            AlterColumn("dbo.Books", "AuthorId", c => c.String());
            RenameColumn(table: "dbo.Books", name: "AuthorId", newName: "Author_AuthorId");
            AddColumn("dbo.Books", "AuthorId", c => c.String());
            CreateIndex("dbo.Books", "Author_AuthorId");
            AddForeignKey("dbo.Books", "Author_AuthorId", "dbo.Authors", "AuthorId");
        }
    }
}
