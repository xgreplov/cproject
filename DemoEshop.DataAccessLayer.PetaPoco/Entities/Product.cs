using System;
using AsyncPoco;
using DemoEshop.DataAccessLayer.PetaPoco.Enums;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.PetaPoco.Entities
{
    /// <summary>
    /// Eshop featured product
    /// </summary>
    [TableName(TableNames.ProductTable)]
    [PrimaryKey("Id", autoIncrement = false)]
    public class Product : IEntity
    {
        public Guid Id { get; set; }

        [Ignore]
        public string TableName { get; } = TableNames.ProductTable;

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Number of units avaiable at the storage
        /// </summary>
        public int StoredUnits { get; set; }

        public decimal Price { get; set; }

        public DiscountType DiscountType { get; set; }

        public int DiscountPercentage { get; set; }

        public string ProductImgUri { get; set; }

        public Guid CategoryId { get; set; }

        [Ignore]
        public Category Category { get; set; }
    }
}
