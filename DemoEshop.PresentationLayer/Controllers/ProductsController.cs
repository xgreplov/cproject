using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades;
using DemoEshop.PresentationLayer.Models.Products;
using X.PagedList;

namespace DemoEshop.PresentationLayer.Controllers
{
    public class ProductsController : Controller
    {
        #region SessionKey constants

        public const int PageSize = 9;

        private const string FilterSessionKey = "filter";
        private const string CategoryTreesSessionKey = "categoryTrees";

        #endregion

        #region Facades

        public ProductFacade ProductFacade { get; set; }

        #endregion

        #region ProductsActionMethods

        public async Task<ActionResult> Index(int page = 1)
        {
            var filter = Session[FilterSessionKey] as ProductFilterDto ?? new ProductFilterDto{ PageSize = PageSize};
            filter.RequestedPageNumber = page;

            //TODO: This is soo much inefficient, why and how could we solve this?
            var allProducts = await ProductFacade.GetProductsAsync(new ProductFilterDto());
            var result = await ProductFacade.GetProductsAsync(filter);

            var categoryTrees = Session[CategoryTreesSessionKey] as IList<CategoryDto>;
            var model = await InitializeProductListViewModel(result, (int)allProducts.TotalItemsCount, categoryTrees);
            return View("ProductListView", model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(ProductListViewModel model)
        {
            model.Filter.PageSize = PageSize;
            model.Filter.CategoryIds = ProcessCategoryIds(model);
            Session[FilterSessionKey] = model.Filter;
            Session[CategoryTreesSessionKey] = model.Categories;

            //TODO: This is soo much inefficient, why and how could we solve this?
            var allProducts = await ProductFacade.GetProductsAsync(new ProductFilterDto());
            var result = await ProductFacade.GetProductsAsync(model.Filter);
            var newModel = await InitializeProductListViewModel(result, (int)allProducts.TotalItemsCount, model.Categories);
            return View("ProductListView", newModel);
        }

        public ActionResult ClearFilter()
        {
            Session[FilterSessionKey] = null;
            Session[CategoryTreesSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var model = await ProductFacade.GetProductAsync(id);
            return View("ProductDetailView", model);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Initializes new ProductListViewModel according to its parameters
        /// </summary>
        /// <param name="result">Product list query result containing products page and related data</param>
        /// <param name="categories">List of category trees</param>
        /// <returns>Initialized instance of ProductListViewModel</returns>
        private async Task<ProductListViewModel> InitializeProductListViewModel(QueryResultDto<ProductDto, ProductFilterDto> result, int totalItemsCount, IList<CategoryDto> categories = null)
        {
            return new ProductListViewModel
            {
                Products = new StaticPagedList<ProductDto>(result.Items, result.RequestedPageNumber ?? 1, PageSize, totalItemsCount),
                Categories = categories ?? await ProductFacade.GetAllCategories() as IList<CategoryDto>,
                Filter = result.Filter
            };
        }

        /// <summary>
        /// Processes category IDs by filtering out unchecked categories
        /// </summary>
        /// <param name="model">model containing category trees</param>
        /// <returns>selected categories</returns>
        private static Guid[] ProcessCategoryIds(ProductListViewModel model)
        {
            var selectedCategoryIds = new List<Guid>();
            foreach (var categoryTreeRoot in model.Categories)
            {
                if (categoryTreeRoot.IsActive)
                {
                    selectedCategoryIds.Add(categoryTreeRoot.Id);
                    selectedCategoryIds.AddRange(model.Categories
                        .Where(node => node.ParentId == categoryTreeRoot.Id && node.IsActive)
                        .Select(node => node.Id));
                }                
            }
            return selectedCategoryIds.ToArray();
        } 
        
        #endregion
    }
}
