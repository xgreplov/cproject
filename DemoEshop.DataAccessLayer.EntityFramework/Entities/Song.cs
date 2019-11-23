using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoEshop.DataAccessLayer.EntityFramework.Enums;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.EntityFramework.Entities
{
    /// <summary>
    /// Eshop featured product
    /// </summary>
    public class Song : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(DemoEshopDbContext.Songs);

        [Required, MaxLength(256)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; } = new DateTime(1950, 1, 1);

        [MaxLength(65536)]
        public string Lyrics { get; set; }

        [MaxLength(65536)]
        public string SongInfo { get; set; }

        [ForeignKey(nameof(Album))]
        public Guid AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}
