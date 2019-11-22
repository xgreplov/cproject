using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.EntityFramework.Entities
{
    /// <summary>
    /// Single item within user Rating
    /// </summary>
    public class RateSong : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(DemoEshopDbContext.RateSongs);

        [Range(0, 5)]
        public int Value { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey(nameof(Rating))]
        public Guid RatingId { get; set; }

        public virtual Rating Rating { get; set; }
    }
}
