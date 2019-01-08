namespace Organizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changesinphonenumberentity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PhoneNumber", "Number", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PhoneNumber", "Number", c => c.Int(nullable: false));
        }
    }
}
