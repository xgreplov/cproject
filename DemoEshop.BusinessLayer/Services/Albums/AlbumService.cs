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

namespace DemoEshop.BusinessLayer.Services.Categories
{
    public class AlbumService : CrudQueryServiceBase<Album, AlbumDto, AlbumFilterDto>, IAlbumService
    {
        public AlbumService(IMapper mapper, IRepository<Album> categoryRepository, QueryObjectBase<AlbumDto, Album, AlbumFilterDto, IQuery<Album>> categoryListQuery)
            : base(mapper, categoryRepository, categoryListQuery) { }
        
        protected override async Task<Album> GetWithIncludesAsync(Guid entityId)
        {
            //MY_POZN toto má být implementované ale nevím proč a jak
            return await Repository.GetAsync(entityId, nameof(Album.Artist));
        }
        
        /// <summary>
        /// Gets ids of the categories with the corresponding names
        /// </summary>
        /// <param name="names">names of the categories</param>
        /// <returns>ids of categories with specified name</returns>
        public async Task<Guid[]> GetAlbumIdsByNamesAsync(params string[] names)
        {
            var queryResult = await Query.ExecuteQuery(new AlbumFilterDto { Names = names });
            return queryResult.Items.Select(albumDto => albumDto.Id).ToArray();
        }
    }
}
