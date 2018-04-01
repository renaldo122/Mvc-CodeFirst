namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Typesss : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Compares", "Quantity", c => c.String());
            AlterColumn("dbo.Products", "Quantity", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Quantity", c => c.Int());
            AlterColumn("dbo.Compares", "Quantity", c => c.Int());
        }
    }
}
