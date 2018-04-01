namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class barcode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Barcode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Barcode", c => c.Long(nullable: false));
        }
    }
}
