namespace CompassAds1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ch6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Type", c => c.String());
            AddColumn("dbo.AspNetUsers", "country", c => c.String());
            AddColumn("dbo.AspNetUsers", "Fname", c => c.String());
            AddColumn("dbo.AspNetUsers", "DateBirthday", c => c.String());
        }
        
        public override void Down()
        {

            DropColumn("dbo.AspNetUsers", "Type");
            DropColumn("dbo.AspNetUsers", "country");
            DropColumn("dbo.AspNetUsers", "Fname");
            DropColumn("dbo.AspNetUsers", "DateBirthday");
        }
    }
}
