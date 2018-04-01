namespace Mvc.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompareTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Compares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Barcode = c.String(nullable: false),
                        Kod = c.String(),
                        Name = c.String(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ProductPrice = c.Decimal(precision: 18, scale: 2),
                        FurnitorPrice = c.Decimal(precision: 18, scale: 2),
                        SmallPrice = c.Decimal(precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Compares");
        }
    }
}
