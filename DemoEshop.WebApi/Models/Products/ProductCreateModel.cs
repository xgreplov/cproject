using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects;

namespace DemoEshop.WebApi.Models.Products
{
    public class ProductCreateModel
    {
        /// <summary>
        /// Product to create.
        /// </summary>
        public ProductDto Product { get; set; }

        /// <summary>
        /// Name of the category to assign product to.
        /// </summary>
        [Required, MinLength(2), MaxLength(256)]
        public string CategoryName { get; set; }
    }
}