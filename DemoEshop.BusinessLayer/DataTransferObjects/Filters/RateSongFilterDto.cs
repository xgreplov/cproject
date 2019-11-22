using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class RateSongFilterDto : FilterDtoBase
    {
        public Guid RateSongId { get; set; }
    }
}
