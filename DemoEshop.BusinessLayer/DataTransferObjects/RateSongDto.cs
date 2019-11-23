using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    /// <summary>
    /// Wrapper class for single item within user Rating
    /// </summary>
    public class RateSongDto : DtoBase
    {
        public Guid Id { get; set; }

        public string TableName { get; } = nameof(DemoEshopDbContext.RateSongs);

        public int Value { get; set; }

        public Guid SongId { get; set; }

        public virtual Song Song { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
