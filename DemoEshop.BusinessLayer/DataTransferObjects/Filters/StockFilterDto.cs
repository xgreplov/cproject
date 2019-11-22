using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class StockFilterDto : FilterDtoBase
    {
        public Guid ProductId { get; set; }
    }
}
