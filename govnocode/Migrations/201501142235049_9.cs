namespace govnocode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Tags", name: "Publication_Id", newName: "PublicationId");
            RenameIndex(table: "dbo.Tags", name: "IX_Publication_Id", newName: "IX_PublicationId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Tags", name: "IX_PublicationId", newName: "IX_Publication_Id");
            RenameColumn(table: "dbo.Tags", name: "PublicationId", newName: "Publication_Id");
        }
    }
}
