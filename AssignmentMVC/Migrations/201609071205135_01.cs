namespace Project01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01 : DbMigration
    {
        public override void Up()
        {
          //  RenameColumn(table: "dbo.Cities", name: "country_Id", newName: "Countries_Id");
          //  RenameIndex(table: "dbo.Cities", name: "IX_country_Id", newName: "IX_Countries_Id");
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Picture = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ItemId);
            
          //  AddColumn("dbo.AspNetUsers", "City", c => c.String());
          //  AddColumn("dbo.AspNetUsers", "Country", c => c.String());
          //  AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
          //  AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
          //  AlterColumn("dbo.Countries", "CountryName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
         //   AlterColumn("dbo.Countries", "CountryName", c => c.String());
         //   DropColumn("dbo.AspNetUsers", "LastName");
         //   DropColumn("dbo.AspNetUsers", "FirstName");
         //   DropColumn("dbo.AspNetUsers", "Country");
         //   DropColumn("dbo.AspNetUsers", "City");
            DropTable("dbo.Items");
         //   RenameIndex(table: "dbo.Cities", name: "IX_Countries_Id", newName: "IX_country_Id");
         //   RenameColumn(table: "dbo.Cities", name: "Countries_Id", newName: "country_Id");
        }
    }
}
