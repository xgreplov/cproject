using DemoEshop.BusinessLayer.DataTransferObjects;

namespace DemoEshop.PresentationLayer.Models.AdminProducts
{
    public class AdminProductEditModel
    {
        public ProductDto Product { get; set; }
        public CategoryDto Category { get; set; }

    }
}