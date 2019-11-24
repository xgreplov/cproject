using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.Facades.Common;
using DemoEshop.BusinessLayer.Services.RateSongs;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.BusinessLayer.Facades
{
    public class RateSongFacade : FacadeBase
    {
        private readonly IRateSongService rateSongFacade;

        public RateSongFacade(IUnitOfWorkProvider unitOfWorkProvider, IRateSongService rateSongFacade) : base(unitOfWorkProvider)
        {
            this.rateSongFacade = rateSongFacade;
        }

        public async Task<RateSongDto> GetRateSongAsync(Guid id)
        {
            using (UnitOfWorkProvider.Create())
            {
                var rateSongDto = await rateSongFacade.GetAsync(id);
                return rateSongDto;
            }
        }

        /// <summary>
        /// Deletes song with given Id
        /// </summary>
        /// <param name="id">Id of the song to delete</param>
        public async Task<bool> DeleteRateSongAsync(Guid id)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if ((await rateSongFacade.GetAsync(id, false)) == null)
                {
                    return false;
                }
                rateSongFacade.Delete(id);
                await uow.Commit();
                return true;
            }
        }

        public async Task<IEnumerable<RateSongDto>> GetAllRateSongs()
        {
            using (UnitOfWorkProvider.Create())
            {
                return (await rateSongFacade.ListAllAsync()).Items;
            }
        }
    }
}