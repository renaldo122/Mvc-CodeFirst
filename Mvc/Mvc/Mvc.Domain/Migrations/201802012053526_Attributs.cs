namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attributs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Funitor_Id", "dbo.Funitors");
            DropIndex("dbo.Products", new[] { "Funitor_Id" });
            AlterColumn("dbo.Products", "Price", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Products", "Funitor_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Funitor_Id", c => c.Int());
            AlterColumn("dbo.Products", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Products", "Funitor_Id");
            AddForeignKey("dbo.Products", "Funitor_Id", "dbo.Funitors", "Id");
        }
    }
}
