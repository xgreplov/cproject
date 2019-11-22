using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.BusinessLayer.Services.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure.Query;

namespace DemoEshop.BusinessLayer.Services.Sales
{
    public class SalesService : ServiceBase, ISalesService
    {
        private readonly QueryObjectBase<RateSongDto, RateSong, RateSongFilterDto, IQuery<RateSong>> RateSongQueryObject;
        private readonly QueryObjectBase<RatingDto, Rating, RatingFilterDto, IQuery<Rating>> OrderQueryObject;


        public SalesService(IMapper mapper, QueryObjectBase<RateSongDto, RateSong, RateSongFilterDto, IQuery<RateSong>> RateSongQueryObject, QueryObjectBase<RatingDto, Rating, RatingFilterDto, IQuery<Rating>> OrderQueryObject) : base(mapper)
        {
            this.RateSongQueryObject = RateSongQueryObject;
            this.OrderQueryObject = OrderQueryObject;
        }
        
        /// <summary>
        /// Gets all Orders according to filter
        /// </summary>
        /// <param name="filter">Order filter</param>
        /// <returns>All Orders</returns>
        public async Task<QueryResultDto<RatingDto, RatingFilterDto>> ListOrdersAsync(RatingFilterDto filter)
        {
            return await OrderQueryObject.ExecuteQuery(filter);
        }

        /// <summary>
        /// Gets all Order items according to filter
        /// </summary>
        /// <param name="filter">Order filter</param>
        /// <returns>All Orders</returns>
        public async Task<IEnumerable<RateSongDto>> ListRateSongsAsync(RateSongFilterDto filter)
        {
            return (await RateSongQueryObject.ExecuteQuery(filter)).Items;
        }
    }
}
