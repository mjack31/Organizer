namespace Organizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addprograminglanguagesentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramingLang",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Friend", "ProgramingLangId_Id", c => c.Int());
            CreateIndex("dbo.Friend", "ProgramingLangId_Id");
            AddForeignKey("dbo.Friend", "ProgramingLangId_Id", "dbo.ProgramingLang", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friend", "ProgramingLangId_Id", "dbo.ProgramingLang");
            DropIndex("dbo.Friend", new[] { "ProgramingLangId_Id" });
            DropColumn("dbo.Friend", "ProgramingLangId_Id");
            DropTable("dbo.ProgramingLang");
        }
    }
}
