using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;

namespace DemoEshop.BusinessLayer.Services.Sales
{
    public interface IArtistService
    {
        Task<ArtistDto> GetArtistAccordingToNamelAsync(string name);
    }
}