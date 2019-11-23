using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using System;

namespace DemoEshop.BusinessLayer.DataTransferObjects.Filters
{
    public class ArtistFilterDto : FilterDtoBase
    {
        public string Name { get; set; }
        public string CountryOfOrigin { get; set; }
        
        public DateTime BirthDate { get; set; } 
    }
}
