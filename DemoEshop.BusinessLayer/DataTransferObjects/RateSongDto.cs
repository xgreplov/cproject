using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class RateSongDto : DtoBase
    {
        public int Value { get; set; }
        public Guid SongId { get; set; }
        public Guid UserId { get; set; }

    }
}
