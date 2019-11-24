using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades.Common;
using DemoEshop.BusinessLayer.Services.Albums;
using DemoEshop.BusinessLayer.Services.Songs;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.BusinessLayer.Facades
{
    public class SongFacade : FacadeBase
    {
        #region Dependencies

        private readonly IAlbumService albumService;
        private readonly ISongService songService;

        public SongFacade(IUnitOfWorkProvider unitOfWorkProvider, IAlbumService albumService,
            ISongService songService)
            : base(unitOfWorkProvider)
        {
            this.albumService = albumService;
            this.songService = songService;
        }

        #endregion

        #region ProductManagement

        /// <summary>
        /// GetAsync song according to ID
        /// </summary>
        /// <param name="id">ID of the song</param>
        /// <returns>The song with given ID, null otherwise</returns>
        public async Task<SongDto> GetSongAsync(Guid id)
        {
            using (UnitOfWorkProvider.Create())
            {
                var songDto = await songService.GetAsync(id);
                return songDto;
            }
        }

        /// <summary>
        /// GetAsync song according to ID
        /// </summary>
        /// <param name="name">name of the song</param>
        /// <returns>The song with given name, null otherwise</returns>
        public async Task<SongDto> GetSongAsync(string name)
        {
            using (UnitOfWorkProvider.Create())
            {
                var songDto = await songService.GetSongByNameAsync(name);
                return songDto;
            }
        }

        /// <summary>
        /// Gets products according to filter and required page
        /// </summary>
        /// <param name="filter">products filter</param>
        /// <param name="includeCurrentlyAvailableUnits">Include number of currently available units for each song</param>
        /// <returns></returns>
        public async Task<QueryResultDto<SongDto, SongFilterDto>> GetSongsAsync(SongFilterDto filter, bool includeCurrentlyAvailableUnits = true)
        {
            using (UnitOfWorkProvider.Create())
            {
                var songListQueryResult = await songService.ListSongsAsync(filter);
                if (!includeCurrentlyAvailableUnits)
                {
                    return songListQueryResult;
                }
                return songListQueryResult;
            }
        }

        /// <summary>
        /// Creates song with category that corresponds with given name
        /// </summary>
        /// <param name="song">song</param>
        /// <param name="albumName">category name</param>
        public async Task<Guid> CreateSongWithAlbumNameAsync(SongDto song, string albumName)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                song.AlbumId = (await albumService.GetAlbumIdsByNamesAsync(albumName)).FirstOrDefault();
                var productId = songService.Create(song);
                await uow.Commit();
                return productId;
            }
        }

        /// <summary>
        /// Updates song
        /// </summary>
        /// <param name="songDto">Product details</param>
        public async Task<bool> EditSongAsync(SongDto songDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if ((await songService.GetAsync(songDto.Id, false)) == null)
                {
                    return false;
                }
                await songService.Update(songDto);
                await uow.Commit();
                return true;
            }
        }

        /// <summary>
        /// Deletes song with given Id
        /// </summary>
        /// <param name="id">Id of the song to delete</param>
        public async Task<bool> DeleteSongAsync(Guid id)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if ((await songService.GetAsync(id, false)) == null)
                {
                    return false;
                }
                songService.Delete(id);
                await uow.Commit();
                return true;
            }
        }

        #endregion

        #region CategoriesManagement

        /// <summary>
        /// Gets category according to ID
        /// </summary>
        /// <param name="albumId">category ID</param>
        /// <returns>The category</returns>
        public async Task<AlbumDto> GetAlbumAsync(Guid albumId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await albumService.GetAsync(albumId);
            }
        }

        /// <summary>
        /// Gets ids of the categories with the corresponding names
        /// </summary>
        /// <param name="names">names of the categories</param>
        /// <returns>ids of categories with specified name</returns>
        public async Task<Guid[]> GetProductCategoryIdsByNamesAsync(params string[] names)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await albumService.GetAlbumIdsByNamesAsync(names);
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>All available categories</returns>
        public async Task<IEnumerable<AlbumDto>> GetAllAlbums()
        {
            using (UnitOfWorkProvider.Create())
            {
                return (await albumService.ListAllAsync()).Items;
            }
        }

        #endregion
    }
}