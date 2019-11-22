using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.EntityFramework.Entities
{
    /// <summary>
    /// Customer Rating
    /// </summary>
    public class Rating : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(DemoEshopDbContext.Ratings);

        public DateTime Issued { get; set; }

        public decimal TotalPrice { get; set; }

        [ForeignKey(nameof(Customer))]
        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
