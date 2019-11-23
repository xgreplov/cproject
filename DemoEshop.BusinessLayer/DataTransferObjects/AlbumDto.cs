using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class AlbumDto : DtoBase
    {
        public string Name { get; set; }

        public Genre Genre { get; set; }

        public Guid ArtistId { get; set; }

        public virtual ArtistDto Artist { get; set; }
    }
}
