using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class ArtistDto : DtoBase
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string CountryOfOrigin { get; set; }

        public DateTime BirthDate { get; set; } = new DateTime(1950, 1, 1);
    }
}
