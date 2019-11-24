using System;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.BusinessLayer.Services.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.Query;

namespace DemoEshop.BusinessLayer.Services.RateSongs
{
    public class RateSongService : CrudQueryServiceBase<RateSong, RateSongDto, RateSongFilterDto>, IRateSongService
    {
        public RateSongService(IMapper mapper, IRepository<RateSong> rateRepository, QueryObjectBase<RateSongDto, RateSong, RateSongFilterDto, IQuery<RateSong>> songRateQueryObject)
            : base(mapper, rateRepository, songRateQueryObject) { }

        public async void RateSong(RateSongDto songRateDTO)
        {
            var result = await Query.ExecuteQuery(new RateSongFilterDto { UserId = songRateDTO.UserId, SongId = songRateDTO.SongId });
            var songRate = Mapper.Map<RateSong>(songRateDTO);
            if (result.TotalItemsCount > 0)
            {
                await Task.Run(() => Repository.Update(songRate));
            }
            else
            {
                await Task.Run(() => Repository.Create(songRate));
            }
        }

        public async Task<RateSongDto> GetRatingAccordingToSongIdAsync(Guid songId)
        {
            var queryResult = await Query.ExecuteQuery(new RateSongFilterDto { SongId = songId });
            return queryResult.Items.GetEnumerator().Current;
        }

        protected override async Task<RateSong> GetWithIncludesAsync(Guid entityId)
        {
            return await Repository.GetAsync(entityId);
        }
    }
}