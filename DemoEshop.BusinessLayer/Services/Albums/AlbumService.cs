using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.BusinessLayer.Services.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.Query;

namespace DemoEshop.BusinessLayer.Services.Albums
{
    public class AlbumService : CrudQueryServiceBase<Album, AlbumDto, AlbumFilterDto>, IAlbumService
    {
        public AlbumService(IMapper mapper, IRepository<Album> categoryRepository, QueryObjectBase<AlbumDto, Album, AlbumFilterDto, IQuery<Album>> categoryListQuery)
            : base(mapper, categoryRepository, categoryListQuery) { }

        protected override async Task<Album> GetWithIncludesAsync(Guid entityId)
        {
            return await Repository.GetAsync(entityId);
        }

        /// <summary>
        /// Gets ids of the albums with the corresponding names
        /// </summary>
        /// <param name="names">names of the albums</param>
        /// <returns>ids of albums with specified name</returns>
        public async Task<Guid[]> GetAlbumIdsByNamesAsync(params string[] names)
        {
            var queryResult = await Query.ExecuteQuery(new AlbumFilterDto { Names = names });
            return queryResult.Items.Select(albumDto => albumDto.Id).ToArray();
        }
    }
}
