using System;
using System.Data.Entity.Migrations;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.DataAccessLayer.EntityFramework.Enums;

namespace DemoEshop.DataAccessLayer.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DemoEshopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DemoEshopDbContext context)
        {
            // Configure case invariant comparison for song and album names
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE Songs ALTER COLUMN Name VARCHAR(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL");
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE Albums ALTER COLUMN Name VARCHAR(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL");

            // Password: PV226jesuper
            var admin = new User { Id = Guid.Parse("ab00dc64-5c07-40fe-a916-175165b9b90f"), Username = "xgreplov", PasswordHash = "ZXnjeNKhDTSH6Rc6q4++tVoQVHo=", PasswordSalt = "hFrDVp5UB9eMycpU+4wSEA==", Role = Role.Admin, Email = "469400@mail.muni.cz"};
            context.Users.AddOrUpdate(admin);

            // Password: qwerty123
            var artist = new Artist {Id = Guid.Parse("aa00dc64-5c07-40fe-a916-175165b9b90f"), Name = "5SOS", CountryOfOrigin = "Australia", BirthDate = DateTime.Now.AddYears(-24)};
            context.Artists.AddOrUpdate(artist);
            
            var youngblood = new Album { Id = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b90f"), Name = "YoungBlood", Genre = Genre.PopRock, ArtistId = artist.Id };

            var soundsGoodFeelsGood = new Album { Id = Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f"), Name = "Sounds Good Feels Good", Genre = Genre.PopRock, ArtistId = artist.Id };

            var fiveSOS = new Album { Id = Guid.Parse("aa03dc64-5c07-40fe-a916-175165b9b90f"), Name = "5 Seconds of Summer", Genre = Genre.PopRock, ArtistId = artist.Id };

            context.Albums.AddOrUpdate(album => album.Id, youngblood, soundsGoodFeelsGood, fiveSOS );

            var teeth = new Song
            {
                Id = Guid.Parse("aa05dc64-5c07-40fe-a916-175165b9b90f"),
                Album = youngblood,
                AlbumId = youngblood.Id,
                Name = "Teeth",
                Lyrics = "Fight so dirty, but your love so sweet, Talk so pretty, but your heart got teeth, Late night devil, put your hands on me, And never, never, never ever let go, Fight so dirty, but your love so sweet, Talk so pretty, but your heart got teeth, Late night devil, put your hands on me, And never, never, never ever let go",
                SongInfo = "a bass-heavy track about the dark highs and lows of a relationship",
            };
            var valentine = new Song
            {
                Id = Guid.Parse("aa06dc64-5c07-40fe-a916-175165b9b90f"),
                Album = youngblood,
                AlbumId = youngblood.Id,
                Name = "Valentine",
                Lyrics = "I can take you out, oh, oh, We can kill some time, stay home, Throw balloons, teddy bears and the chocolate eclairs away, Got nothing but love for you, fall more in love every day, Valentine, valentine",
                SongInfo = "first performed live at the iHeartRadio Music Awards Fan Army Celebration on March 10th 2018",
            };

            var amnesia = new Song
            {
                Id = Guid.Parse("aa07dc64-5c07-40fe-a916-175165b9b90f"),
                Album = fiveSOS,
                AlbumId = fiveSOS.Id,
                Name = "Amnesia",
                Lyrics = "I remember the day you told me you were leaving, I remember the make-up running down your face, And the dreams you left behind you didn't need them, Like every single wish we ever made, I wish that I could wake up with amnesia, And forget about the stupid little things, Like the way it felt to fall asleep next to you, And the memories I never can escape",
                SongInfo = "the narrative voice reminisces about his past relationship and the pain of hearing the fact that his ex has ‘moved on’",
            };

            context.Songs.AddOrUpdate(song => song.Id, teeth, valentine, amnesia);

            context.SaveChanges();
        }
    }
}
