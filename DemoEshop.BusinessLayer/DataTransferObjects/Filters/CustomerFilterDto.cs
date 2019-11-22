using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class CustomerFilterDto : FilterDtoBase
    {
        public string Email { get; set; }
        public string Roles { get; set; }
    }
}
