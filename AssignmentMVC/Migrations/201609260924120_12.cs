namespace Project01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InvoiceAddresses", "OrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InvoiceAddresses", "OrderId", c => c.Int(nullable: false));
        }
    }
}
