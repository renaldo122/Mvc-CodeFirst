namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Marka11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Compares", "Barcode", c => c.String());
            AlterColumn("dbo.Compares", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Compares", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Compares", "Barcode", c => c.String(nullable: false));
        }
    }
}
