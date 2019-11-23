using System;
using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class SongDto : DtoBase
    {
        public Guid Id { get; set; }

        public string TableName { get; } = nameof(DemoEshopDbContext.Songs);

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; } = new DateTime(1950, 1, 1);

        public string Lyrics { get; set; }

        public string SongInfo { get; set; }

        public Guid AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}
