namespace NyimboProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_UserID2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Songs", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Songs", "UserId", c => c.Int(nullable: false));
        }
    }
}
