namespace govnocode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RatePublications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdPublication = c.Int(nullable: false),
                        IdUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RatePublications");
        }
    }
}
