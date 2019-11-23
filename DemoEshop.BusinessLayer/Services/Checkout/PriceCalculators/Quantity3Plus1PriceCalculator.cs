using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.Services.Checkout.PriceCalculators
{
    public class Value3Plus1PriceCalculator : IPriceCalculator
    {
        private const int RequiredForDiscount = 4;

        public Role DiscountType { get; } = Role.Value3Plus1;

        public decimal CalculatePrice(RateSongDto RateSong)
        {
            var numberOfFreeProducts = RateSong.Value / RequiredForDiscount;
            return (RateSong.Value - numberOfFreeProducts) * RateSong.Product.Price;
        }
    }
}
