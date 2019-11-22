using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects;

namespace DemoEshop.PresentationLayer.Models.ShoppingCart
{
    public class ShoppingCartViewModel
    {
        public IList<RateSongDto> ShoppingCartItems { get; set; }

        public CustomerDto Customer { get; set; }

        [Range(0, int.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        public ShoppingCartViewModel()
        {
            ShoppingCartItems = new List<RateSongDto>();
        }
    }
}