using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class RatingFilterDto : FilterDtoBase
    {
        public Guid CustomerId { get; set; }
    }
}
