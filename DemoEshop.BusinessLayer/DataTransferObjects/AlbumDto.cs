using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class AlbumDto : DtoBase
    {
        public Guid Id { get; set; }

        public string TableName { get; } = nameof(DemoEshopDbContext.Albums);

        public string Name { get; set; }

        public Genre Genre { get; set; }

        public Guid ArtistId { get; set; }

        public virtual Artist Artist { get; set; }
    }
}
