using System;
using AsyncPoco;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.PetaPoco.Entities
{
    /// <summary>
    /// Product Category (hierarchical structure)
    /// </summary>
    [TableName(TableNames.CategoryTable)]
    [PrimaryKey("Id", autoIncrement = false)]
    public class Category : IEntity
    {
        public Guid Id { get; set; }

        [Ignore]
        public string TableName { get; } = TableNames.CategoryTable;

        public string Name { get; set; }

        [Ignore]
        public bool HasParent => this.Parent != null;

        public Guid? ParentId { get; set; }

        /// <summary>
        /// Parent category
        /// </summary>
        [Ignore]
        public Category Parent { get; set; }
    }
}
