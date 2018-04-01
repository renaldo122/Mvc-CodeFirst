namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompareProductFound1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Compares", "ProductFound", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Compares", "ProductFound");
        }
    }
}
