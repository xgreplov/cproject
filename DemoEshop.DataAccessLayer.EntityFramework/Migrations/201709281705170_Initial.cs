namespace DemoEshop.DataAccessLayer.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                a => new
                    {
                        Id = a.Guid(nullable: false),
                        Name = a.String(nullable: false, maxLength: 256),
                        Genre = a.Int(nullable: false),
                        ArtistId = a.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artists", t => t.ArtistId, cascadeDelete: true)
                .Index(t => t.ArtistId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(nullable: false),
                        PasswordSalt = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.String(nullable: false, maxLength: 100),
                        Role = c.Int(nullable: false),
                        Email = c.String(),
                  //      Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Artists",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(nullable: false),
                    CountryOfOrigin = c.String(),
                    BirthDate = c.DateTime(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.RateSongs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Value = c.Int(nullable: false),
                        SongId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        ReleaseDate = c.DateTime(),
                        Lyrics = c.String(),
                        SongInfo = c.String(),                        
                        AlbumId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.AlbumId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RateSongs", "SongId", "dbo.Songs");
            DropForeignKey("dbo.Songs", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.RateSongs", "UserId", "dbo.Users");
            DropForeignKey("dbo.Albums", "ArtistId", "dbo.Artists");
            DropIndex("dbo.Songs", new[] { "AlbumId" });
            DropIndex("dbo.RateSongs", new[] { "SongId" });
            DropIndex("dbo.RateSongs", new[] { "UserId" });
            DropIndex("dbo.Albums", new[] { "ArtistId" });
            DropTable("dbo.Songs");
            DropTable("dbo.RateSongs");
            DropTable("dbo.Users");
            DropTable("dbo.Albums");
            DropTable("dbo.Artists");
        }
    }
}
