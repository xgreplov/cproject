using System;
using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class AlbumDto : DtoBase
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public Genre Genre { get; set; }

        public Guid ArtistId { get; set; }
    }
}
