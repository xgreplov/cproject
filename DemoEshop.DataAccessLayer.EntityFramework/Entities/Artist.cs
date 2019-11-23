using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.EntityFramework.Entities
{
    /// <summary>
    /// Represents eshop customer
    /// </summary>
    public class Artist : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(DemoEshopDbContext.Artists);

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string CountryOfOrigin { get; set; }
        public DateTime BirthDate { get; set; } = new DateTime(1950, 1, 1);
    }
}