namespace govnocode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "Publication_Id", c => c.Int());
            CreateIndex("dbo.Tags", "Publication_Id");
            AddForeignKey("dbo.Tags", "Publication_Id", "dbo.Publications", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "Publication_Id", "dbo.Publications");
            DropIndex("dbo.Tags", new[] { "Publication_Id" });
            DropColumn("dbo.Tags", "Publication_Id");
        }
    }
}
