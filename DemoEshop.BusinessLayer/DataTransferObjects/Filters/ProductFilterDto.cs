using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class ProductFilterDto : FilterDtoBase
    {
        public Guid[] CategoryIds { get; set; }

        public string[] CategoryNames { get; set; }

        public string SearchedName { get; set; }

        public decimal MinimalPrice { get; set; } = 0;

        public decimal MaximalPrice { get; set; } = decimal.MaxValue;
    }
}
