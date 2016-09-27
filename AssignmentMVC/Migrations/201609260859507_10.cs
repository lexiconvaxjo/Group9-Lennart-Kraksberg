namespace Project01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CartId", c => c.String());
            DropColumn("dbo.Orders", "UserName");
            DropColumn("dbo.Orders", "FirstName");
            DropColumn("dbo.Orders", "LastName");
            DropColumn("dbo.Orders", "Email");
            DropColumn("dbo.Orders", "PhoneNumber");
            DropColumn("dbo.Orders", "Country");
            DropColumn("dbo.Orders", "City");
            DropColumn("dbo.Orders", "Address");
            DropColumn("dbo.Orders", "PostalCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "PostalCode", c => c.String());
            AddColumn("dbo.Orders", "Address", c => c.String());
            AddColumn("dbo.Orders", "City", c => c.String());
            AddColumn("dbo.Orders", "Country", c => c.String());
            AddColumn("dbo.Orders", "PhoneNumber", c => c.String());
            AddColumn("dbo.Orders", "Email", c => c.String());
            AddColumn("dbo.Orders", "LastName", c => c.String());
            AddColumn("dbo.Orders", "FirstName", c => c.String());
            AddColumn("dbo.Orders", "UserName", c => c.String());
            DropColumn("dbo.Orders", "CartId");
        }
    }
}
