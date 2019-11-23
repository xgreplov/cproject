using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class ArtistFilterDto : FilterDtoBase
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }
}
