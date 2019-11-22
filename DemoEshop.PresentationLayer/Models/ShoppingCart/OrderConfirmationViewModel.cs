using DemoEshop.BusinessLayer.DataTransferObjects;

namespace DemoEshop.PresentationLayer.Models.ShoppingCart
{
    /// <summary>
    /// Wrapper for Order confirmation
    /// </summary>
    public class OrderConfirmationViewModel
    {
        public CustomerDto Customer { get; set; }

        public RatingDto Order { get; set; }
    }
}
