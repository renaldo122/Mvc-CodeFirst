namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Marka : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Compares", "Marka", c => c.String());
            AddColumn("dbo.Products", "Marka", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Marka");
            DropColumn("dbo.Compares", "Marka");
        }
    }
}
