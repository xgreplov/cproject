using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades.Common;
using DemoEshop.BusinessLayer.Services.Checkout;
using DemoEshop.BusinessLayer.Services.Reservation;
using DemoEshop.BusinessLayer.Services.Sales;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.BusinessLayer.Facades
{
    public class OrderFacade : FacadeBase
    {
        private readonly ISalesService salesService;
        private readonly ICheckoutService checkoutService;
        private readonly IReservationService productReservationService;

        public OrderFacade(IUnitOfWorkProvider unitOfWorkProvider, ICheckoutService checkoutService, ISalesService salesService, 
            IReservationService productReservationService) : base(unitOfWorkProvider)
        {
            this.checkoutService = checkoutService;
            this.salesService = salesService;
            this.productReservationService = productReservationService;
        }

        #region OrderManagement

        /// <summary>
        /// Persists Order together with all related data
        /// </summary>
        /// <param name="createRatingDto">wrapper for Order, RateSongs, customer and coupon</param>
        /// <param name="confirmationAction">Action to perform when uow successfully performs after commit action</param>
        public async Task<Guid> ConfirmOrderAsync(OrderCreateDto createRatingDto, Action confirmationAction)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                uow.RegisterAction(confirmationAction);
                await productReservationService.DispatchProduct(createRatingDto.RatingDto.CustomerId);
                var createdRateSongId = await checkoutService.ConfirmOrderAsync(createRatingDto);
                await uow.Commit();
                return createdRateSongId;
            }
        } 
        
        /// <summary>
        /// Calculates total price for all Order items (overall coupon discount is included)
        /// </summary>
        /// <param name="RateSongs">all Order items</param>
        /// <returns>Total price for given items</returns>
        public decimal CalculateTotalPrice(ICollection<RateSongDto> RateSongs)
        {
            using (UnitOfWorkProvider.Create())
            {
                return checkoutService.CalculateTotalPrice(RateSongs);
            }
        } 
        
        /// <summary>
        /// Gets all Orders according to filter and required page
        /// </summary>
        /// <param name="filter">Order filter</param>
        /// <returns>All Orders</returns>
        public async Task<IEnumerable<RatingDto>> GetOrdersAsync(RatingFilterDto filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                return (await salesService.ListOrdersAsync(filter)).Items;
            }
        }

        /// <summary>
        /// Gets all RateSongs from specified Order
        /// </summary>
        /// <param name="filter">Order item filter</param>
        /// <returns>all Order items from specified Order</returns>
        public async Task<IEnumerable<RateSongDto>> ListItemsFromOrderAsync(RateSongFilterDto filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await salesService.ListRateSongsAsync(filter);
            }
        }
        
        #endregion

        #region ProductsReservationManagement

        /// <summary>
        /// Performs product reservation in Order to ensure
        /// simultaneous Orders can be satisfied
        /// </summary>
        /// <param name="productReservation">Product reservation details containing 
        /// customer reserving the product, amount, productID and expiration</param>
        /// <returns>true if reservation is successfull, otherwise false</returns>
        public async Task<bool> ReserveProductAsync(ProductReservationDto productReservation)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await productReservationService.ReserveProduct(productReservation);
            }
        }

        /// <summary>
        /// Performs product release 
        /// (typically when user confirms or cancels the Order)
        /// </summary>
        /// <param name="customerId">ID of customer who reserved the product</param>
        /// <param name="productIds">IDs of the product to release</param>
        public void ReleaseReservations(Guid customerId, params Guid[] productIds)
        {
            using (UnitOfWorkProvider.Create())
            {
                productReservationService.ReleaseProductReservations(customerId, productIds);
            }
        }
        
        /// <summary>
        /// Gets number of currently available (free)
        /// units for product belonging to product with specified ID
        /// </summary>
        /// <param name="productId">ID of the product for which product availability should be checked</param>
        /// <returns>Number of free units for this product</returns>
        public async Task<int> GetCurrentlyAvailableUnitsAsync(Guid productId)
        {
            return await productReservationService.GetCurrentlyAvailableUnits(productId);
        }

        /// <summary>
        /// Performs Order of required product from distributor
        /// </summary>
        /// <param name="productId">id of product to Order</param>
        /// <param name="value">Value of Order</param>
        public async Task OrderProductFromDistributorAsync(Guid productId, int value)
        {
            await productReservationService.OrderProductFromDistributor(productId, value);
        }

        #endregion
    }
}
