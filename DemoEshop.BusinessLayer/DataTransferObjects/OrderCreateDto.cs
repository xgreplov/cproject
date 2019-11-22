using System.Collections.Generic;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class OrderCreateDto
    {
        public RatingDto RatingDto { get; set; }

        public IEnumerable<RateSongDto> RateSongs { get; set; } = new List<RateSongDto>();
    }
}
