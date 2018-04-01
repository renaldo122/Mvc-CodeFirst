namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompareProductNewProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Compares", "Type", c => c.String());
            AddColumn("dbo.Products", "Klasifikim", c => c.String());
            AlterColumn("dbo.Compares", "Quantity", c => c.Int());
            AlterColumn("dbo.Products", "Quantity", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.Compares", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "Klasifikim");
            DropColumn("dbo.Compares", "Type");
        }
    }
}
