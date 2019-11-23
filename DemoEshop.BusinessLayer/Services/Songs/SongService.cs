using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.BusinessLayer.Services.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.Query;

namespace DemoEshop.BusinessLayer.Services.Songs
{
    public class SongService : CrudQueryServiceBase<Song, SongDto, SongFilterDto>, ISongService
    {
        public SongService(IMapper mapper, QueryObjectBase<SongDto, Song, SongFilterDto, IQuery<Song>> productQuery, IRepository<Song> songRepository)
            : base(mapper, songRepository, productQuery) { }

        protected override Task<Song> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, nameof(Song.Album));
        }
        public async Task<SongDto> GetSongByNameAsync(string name)
        {
            var queryResult = await Query.ExecuteQuery(new SongFilterDto { SearchedName = name });
            return queryResult.Items.SingleOrDefault();
        }
        public async Task<QueryResultDto<SongDto, SongFilterDto>> ListSongsAsync(SongFilterDto filter)
        {
            return await Query.ExecuteQuery(filter);
        }
    }
}
