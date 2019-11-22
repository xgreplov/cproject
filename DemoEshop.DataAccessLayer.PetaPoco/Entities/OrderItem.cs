using System;
using AsyncPoco;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.PetaPoco.Entities
{
    /// <summary>
    /// Single item within customer order
    /// </summary>
    [TableName(TableNames.OrderItemTable)]
    [PrimaryKey("Id", autoIncrement = false)]
    public class OrderItem : IEntity
    {
        public Guid Id { get; set; }

        [Ignore]
        public string TableName { get; } = TableNames.OrderItemTable;

        public int Quantity { get; set; }

        public Guid ProductId { get; set; }

        [Ignore]
        public Product Product { get; set; }

        public Guid OrderId { get; set; }

        [Ignore]
        public Order Order { get; set; }
    }
}
