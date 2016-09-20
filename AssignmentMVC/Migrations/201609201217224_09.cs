namespace Project01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _09 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Carts", "ItemName");
            DropColumn("dbo.Carts", "ItemDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Carts", "ItemDescription", c => c.String());
            AddColumn("dbo.Carts", "ItemName", c => c.String());
        }
    }
}
