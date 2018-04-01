namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Marka1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Barcode", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Barcode", c => c.String(nullable: false));
        }
    }
}
