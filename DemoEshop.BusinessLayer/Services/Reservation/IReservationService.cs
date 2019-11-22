using System;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;

namespace DemoEshop.BusinessLayer.Services.Reservation
{
    public interface IReservationService
    {
        /// <summary>
        /// Performs product reservation in Order to ensure
        /// simultaneous Orders can be satisfied
        /// </summary>
        /// <param name="productReservation">Product reservation details containing 
        ///   customer reserving the product, amount, and expiration</param>
        /// <returns>true if reservation is successfull, otherwise false</returns>
        Task<bool> ReserveProduct(ProductReservationDto productReservation);

        /// <summary>
        /// Performs product release 
        /// (typically when user confirms or cancels the Order)
        /// </summary>
        /// <param name="customerId">ID of customer who reserved the product</param>
        /// <param name="productIds">IDs of the product to release</param>
        void ReleaseProductReservations(Guid customerId, params Guid[] productIds);

        /// <summary>
        /// Performs product expedition by decreasing product 
        /// stored units and releasing corresponding reservations
        /// </summary>
        /// <param name="customerId">Id of customer whose reserved product should be dispatched</param>
        Task DispatchProduct(Guid customerId);

        /// <summary>
        /// Performs Order of required product from distributor
        /// </summary>
        /// <param name="productId">id of product to Order</param>
        /// <param name="value">Number of units to Order</param>
        Task OrderProductFromDistributor(Guid productId, int value);

        /// <summary>
        /// Gets number of currently available (free)
        /// units for product
        /// </summary>
        /// <param name="productId">ID of the product for which product availability should be checked</param>
        /// <returns>Number of free units for this product</returns>
        Task<int> GetCurrentlyAvailableUnits(Guid productId);
    }
}