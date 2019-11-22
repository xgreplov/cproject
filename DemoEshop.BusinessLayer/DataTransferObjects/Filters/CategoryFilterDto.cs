using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class CategoryFilterDto : FilterDtoBase
    {
        public string[] Names { get; set; }
    }
}
