using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.Services.Checkout.PriceCalculators
{
    public interface IPriceCalculator
    {
        DiscountType DiscountType { get; }

        decimal CalculatePrice(RateSongDto RateSong);
    }
}
