using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;

namespace DemoEshop.BusinessLayer.Services.Artists
{
    public interface IArtistService
    {
        Task<ArtistDto> GetArtistAccordingToNameAsync(string name);

        Task<ArtistDto> GetAsync(Guid entityId, bool withIncludes = true);

        Guid Create(ArtistDto entityDto);

        Task Update(ArtistDto entityDto);

        void Delete(Guid entityId);

        Task<QueryResultDto<ArtistDto, ArtistFilterDto>> ListAllAsync();
    }
}
