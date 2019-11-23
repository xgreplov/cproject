using System;
using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class RateSongDto : DtoBase
    {
        [Required]
        [Range(0, 5)]
        public int Value { get; set; }
        public Guid SongId { get; set; }
        public Guid UserId { get; set; }

    }
}
