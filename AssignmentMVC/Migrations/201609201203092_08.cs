namespace Project01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _08 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "ItemName", c => c.String());
            AddColumn("dbo.Carts", "ItemDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "ItemDescription");
            DropColumn("dbo.Carts", "ItemName");
        }
    }
}
