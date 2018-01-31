namespace CompassAds1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ch3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Flag", c => c.String());
        }
        
        public override void Down()
        {
        }
    }
}
