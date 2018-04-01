namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Funitor : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Categories", newName: "Funitors");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            AddColumn("dbo.Products", "Funitor_Id", c => c.Int());
            CreateIndex("dbo.Products", "Funitor_Id");
            AddForeignKey("dbo.Products", "Funitor_Id", "dbo.Funitors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Funitor_Id", "dbo.Funitors");
            DropIndex("dbo.Products", new[] { "Funitor_Id" });
            DropColumn("dbo.Products", "Funitor_Id");
            CreateIndex("dbo.Products", "CategoryId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Funitors", newName: "Categories");
        }
    }
}
