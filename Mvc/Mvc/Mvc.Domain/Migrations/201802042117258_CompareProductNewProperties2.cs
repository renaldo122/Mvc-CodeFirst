namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompareProductNewProperties2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Compares", "Klasifikim", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Compares", "Klasifikim");
        }
    }
}
