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
    public class Product : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(DemoEshopDbContext.Products);

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(65536)]
        public string Description { get; set; }

        /// <summary>
        /// Number of units avaiable at the storage
        /// </summary>
        [Range(0, int.MaxValue)]
        public int StoredUnits { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        public DiscountType DiscountType { get; set; }

        [Range(0, 99)]
        public int DiscountPercentage { get; set; }

        [MaxLength(1024)]
        public string ProductImgUri { get; set; }

        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
