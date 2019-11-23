using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class AlbumFilterDto : FilterDtoBase
    {
        public string[] Names { get; set; }
    }
}
