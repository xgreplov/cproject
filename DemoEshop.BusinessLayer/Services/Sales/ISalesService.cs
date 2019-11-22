using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;

namespace DemoEshop.BusinessLayer.Services.Sales
{
    public interface ISalesService
    {
        Task<IEnumerable<RateSongDto>> ListRateSongsAsync(RateSongFilterDto filter);
        Task<QueryResultDto<RatingDto, RatingFilterDto>> ListOrdersAsync(RatingFilterDto filter);
    }
}