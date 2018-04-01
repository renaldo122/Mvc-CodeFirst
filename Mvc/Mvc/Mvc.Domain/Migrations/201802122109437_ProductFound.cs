namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductFound : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Compares", "ProductFound", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Compares", "ProductFound", c => c.Boolean(nullable: false));
        }
    }
}
