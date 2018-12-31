namespace Organizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changefriendsentity : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Friend", name: "ProgramingLangId_Id", newName: "FavoriteLanguageId");
            RenameIndex(table: "dbo.Friend", name: "IX_ProgramingLangId_Id", newName: "IX_FavoriteLanguageId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Friend", name: "IX_FavoriteLanguageId", newName: "IX_ProgramingLangId_Id");
            RenameColumn(table: "dbo.Friend", name: "FavoriteLanguageId", newName: "ProgramingLangId_Id");
        }
    }
}
