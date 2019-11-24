using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.Facades.Common;
using DemoEshop.BusinessLayer.Services.Categories;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.BusinessLayer.Facades
{
    public class AlbumFacade : FacadeBase
    {
        private readonly IAlbumService albumService;
        
        public AlbumFacade(IUnitOfWorkProvider unitOfWorkProvider, IAlbumService albumService) : base(unitOfWorkProvider)
        {
            this.albumService = albumService;
        }
        
        public async Task<AlbumDto> GetArtistAsync(Guid id)
        {
            using (UnitOfWorkProvider.Create())
            {
                var artistDto = await albumService.GetAsync(id);
                return artistDto;
            }
        }
        /// <summary>
        /// Updates song
        /// </summary>
        /// <param name="AlbumDto">Product details</param>
        public async Task<bool> EditAlbumAsync(AlbumDto albumDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if ((await albumService.GetAsync(albumDto.Id, false)) == null)
                {
                    return false;
                }
                await albumService.Update(albumDto);
                await uow.Commit();
                return true;
            }
        }

        /// <summary>
        /// Deletes song with given Id
        /// </summary>
        /// <param name="id">Id of the song to delete</param>
        public async Task<bool> DeleteAlbumAsync(Guid id)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if ((await albumService.GetAsync(id, false)) == null)
                {
                    return false;
                }
                albumService.Delete(id);
                await uow.Commit();
                return true;
            }
        }
        
        public async Task<IEnumerable<AlbumDto>> GetAllAlbums()
        {
            using (UnitOfWorkProvider.Create())
            {
                return (await albumService.ListAllAsync()).Items;
            }
        }
        
    }
}