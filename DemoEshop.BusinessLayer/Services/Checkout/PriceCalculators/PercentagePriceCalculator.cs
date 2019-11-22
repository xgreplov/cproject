using System;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.Services.Checkout.PriceCalculators
{
    public class PercentagePriceCalculator : IPriceCalculator
    {
        public DiscountType DiscountType { get; } = DiscountType.Percentage;

        public decimal CalculatePrice(RateSongDto RateSong)
        {
            var pricePerSingleProduct = Math.Round(RateSong.Product.Price * (decimal)(1 - RateSong.Product.DiscountPercentage / 100.0));
            return pricePerSingleProduct * RateSong.Value;
        }
    }
}
