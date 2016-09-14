namespace Project01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderDetails", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderDetails", "Quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
