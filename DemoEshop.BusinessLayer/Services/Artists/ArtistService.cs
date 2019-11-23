using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.BusinessLayer.Services.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.Query;

namespace DemoEshop.BusinessLayer.Services.Artists
{
    public class ArtistService : CrudQueryServiceBase<Artist, ArtistDto, ArtistFilterDto>, IArtistService
    {
        public ArtistService(IMapper mapper, QueryObjectBase<ArtistDto, Artist, ArtistFilterDto, IQuery<Artist>> artistQuery, IRepository<Artist> artistRepository)
           : base(mapper, artistRepository, artistQuery) { }

        protected override async Task<Artist> GetWithIncludesAsync(Guid entityId)
        {
            return await Repository.GetAsync(entityId);
        }

        public async Task<ArtistDto> GetArtistAccordingToNameAsync(string name)
        {
            var queryResult = await Query.ExecuteQuery(new ArtistFilterDto { Name = name });
            return queryResult.Items.SingleOrDefault();
        }


        public async Task<List<ArtistDto>> GetArtistsAccordingToCountryOfOriginAsync(string countryOfOrigin)
        {
            var queryResult = await Query.ExecuteQuery(new ArtistFilterDto { CountryOfOrigin = countryOfOrigin });
            return queryResult.Items.ToList();
        }
    }
}
