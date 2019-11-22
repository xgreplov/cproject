using System.Collections.Generic;
using System.Web.Mvc;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using X.PagedList;

namespace DemoEshop.PresentationLayer.Models.Products
{
    public class ProductListViewModel
    {
        public string[] ProductSortCriteria => new[]{nameof(ProductDto.Name), nameof(ProductDto.Price), nameof(ProductDto.DiscountPercentage) };

        public IList<CategoryDto> Categories { get; set; }

        public IPagedList<ProductDto> Products { get; set; }

        public ProductFilterDto Filter { get; set; }

        public SelectList AllSortCriteria => new SelectList(ProductSortCriteria);
    }
}