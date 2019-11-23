using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class SongFilterDto : FilterDtoBase
    {
        public Guid[] AlbumIds { get; set; }

        public string[] AlbumNames { get; set; }

        public string SearchedName { get; set; }
    }
}
