namespace CompassAds1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HH : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Flag1", c => c.String());
            AddColumn("dbo.AspNetUsers", "Type1", c => c.String());
            AddColumn("dbo.AspNetUsers", "country1", c => c.String());
            AddColumn("dbo.AspNetUsers", "Fname1", c => c.String());
            AddColumn("dbo.AspNetUsers", "DateBirthday1", c => c.String());
           
        }
        
        public override void Down()
        {
            
            DropColumn("dbo.AspNetUsers", "DateBirthday1");
            DropColumn("dbo.AspNetUsers", "Fname1");
            DropColumn("dbo.AspNetUsers", "country1");
            DropColumn("dbo.AspNetUsers", "Type1");
            DropColumn("dbo.AspNetUsers", "Flag1");
        }
    }
}
