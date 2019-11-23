using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoEshop.DataAccessLayer.EntityFramework.Enums;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.EntityFramework.Entities
{
    public class Album : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(DemoEshopDbContext.Albums);

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public Genre Genre { get; set; }

        [ForeignKey(nameof(Artist))]
        public Guid ArtistId { get; set; }

        public virtual Artist Artist { get; set; }
    }
}
