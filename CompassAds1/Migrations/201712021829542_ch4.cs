namespace CompassAds1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ch4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "DateOfBirthday", c => c.String());

        }
        
        public override void Down()
        {
        }
    }
}
