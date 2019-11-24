using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class SongFilterDto : FilterDtoBase
    {
        public Guid AlbumId { get; set; }

        public string AlbumName { get; set; }

        public string SearchedName { get; set; }
    }
}
