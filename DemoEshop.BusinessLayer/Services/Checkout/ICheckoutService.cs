using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;

namespace DemoEshop.BusinessLayer.Services.Checkout
{
    public interface ICheckoutService
    {
        /// <summary>
        /// Persists Order together with all related data
        /// </summary>
        /// <param name="createRatingDto">wrapper for Order, RateSongs, customer and coupon</param>
        Task<Guid> ConfirmOrderAsync(OrderCreateDto createRatingDto);

        /// <summary>
        /// Calculates total price for all Order items (overall coupon discount is included)
        /// </summary>
        /// <param name="RateSongs">all Order items</param>
        /// <returns>Total price for given items</returns>
        decimal CalculateTotalPrice(ICollection<RateSongDto> RateSongs);
    }
}