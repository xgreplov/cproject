using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.Facades.Common;
using DemoEshop.BusinessLayer.Services.Sales;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.BusinessLayer.Facades
{
    public class ArtistFacade : FacadeBase
    {
        private readonly IArtistService artistService;
        
        public ArtistFacade(IUnitOfWorkProvider unitOfWorkProvider, IArtistService artistService) : base(unitOfWorkProvider)
        {
            this.artistService = artistService;
        }
        
        public async Task<ArtistDto> GetArtistAsync(Guid id)
        {
            using (UnitOfWorkProvider.Create())
            {
                var artistDto = await artistService.GetAsync(id);
                return artistDto;
            }
        }
        /// <summary>
        /// Updates song
        /// </summary>
        /// <param name="artistDto">Product details</param>
        public async Task<bool> EditArtistAsync(ArtistDto artistDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if ((await artistService.GetAsync(artistDto.Id, false)) == null)
                {
                    return false;
                }
                await artistService.Update(artistDto);
                await uow.Commit();
                return true;
            }
        }

        /// <summary>
        /// Deletes song with given Id
        /// </summary>
        /// <param name="id">Id of the song to delete</param>
        public async Task<bool> DeleteArtistAsync(Guid id)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if ((await artistService.GetAsync(id, false)) == null)
                {
                    return false;
                }
                artistService.Delete(id);
                await uow.Commit();
                return true;
            }
        }
        
        public async Task<IEnumerable<ArtistDto>> GetAllArtists()
        {
            using (UnitOfWorkProvider.Create())
            {
                return (await artistService.ListAllAsync()).Items;
            }
        }
        
    }
}