using System;
using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class SongDto : DtoBase
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; } = new DateTime(1950, 1, 1);
        
        [MaxLength(65536)]
        public string Lyrics { get; set; }
        
        [MaxLength(65536)]
        public string SongInfo { get; set; }

        public Guid AlbumId { get; set; }
        public AlbumDto Album { get; set; }
    }
}
