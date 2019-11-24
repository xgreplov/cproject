using System;
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
        
        Task<ArtistDto> GetAsync(Guid entityId, bool withIncludes = true);

        Guid Create(ArtistDto entityDto);

        Task Update(ArtistDto entityDto);

        void Delete(Guid entityId);

        Task<QueryResultDto<ArtistDto, ArtistFilterDto>> ListAllAsync();
    }
}