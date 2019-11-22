using System;
using AsyncPoco;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.PetaPoco.Entities
{
    /// <summary>
    /// Customer order
    /// </summary>
    [TableName(TableNames.OrderTable)]
    [PrimaryKey("Id", autoIncrement = false)]
    public class Order : IEntity
    {        
        public Guid Id { get; set; }

        [Ignore]
        public string TableName { get; } = TableNames.OrderTable;

        public DateTime Issued { get; set; }

        public decimal TotalPrice { get; set; }

        public Guid CustomerId { get; set; }

        [Ignore]
        public Customer Customer { get; set; }
    }
}
