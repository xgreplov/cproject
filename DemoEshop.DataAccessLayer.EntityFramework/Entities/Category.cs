using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.EntityFramework.Entities
{
    /// <summary>
    /// Product Category (hierarchical structure)
    /// </summary>
    public class Category : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(DemoEshopDbContext.Categories);

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [NotMapped]
        public bool HasParent => this.Parent != null;

        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Parent category
        /// </summary>
        public virtual Category Parent { get; set; }
    }
}
