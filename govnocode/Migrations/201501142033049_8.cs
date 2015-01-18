namespace govnocode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Publications", "TagString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publications", "TagString");
        }
    }
}
