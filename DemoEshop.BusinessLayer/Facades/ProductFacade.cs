using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades.Common;
using DemoEshop.BusinessLayer.Services.Albums;
using DemoEshop.BusinessLayer.Services.Songs;
using DemoEshop.BusinessLayer.Services.Reservation;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.BusinessLayer.Facades
{
    public class ProductFacade : FacadeBase
    {
        #region Dependencies

        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        private readonly IReservationService stockReservationService;

        public ProductFacade(IUnitOfWorkProvider unitOfWorkProvider, ICategoryService categoryService, 
            IProductService productService, IReservationService stockReservationService)
            : base(unitOfWorkProvider)
        {
            this.categoryService = categoryService;
            this.productService = productService;
            this.stockReservationService = stockReservationService;
        }

        #endregion

        #region ProductManagement

        /// <summary>
        /// GetAsync product according to ID
        /// </summary>
        /// <param name="id">ID of the product</param>
        /// <returns>The product with given ID, null otherwise</returns>
        public async Task<ProductDto> GetProductAsync(Guid id)
        {
            using (UnitOfWorkProvider.Create())
            {
                var product = await productService.GetAsync(id);
                product.CurrentlyAvailableUnits = await stockReservationService.GetCurrentlyAvailableUnits(id);
                return product;
            }
        }

        /// <summary>
        /// GetAsync product according to ID
        /// </summary>
        /// <param name="name">name of the product</param>
        /// <returns>The product with given name, null otherwise</returns>
        public async Task<ProductDto> GetProductAsync(string name)
        {
            using (UnitOfWorkProvider.Create())
            {
                var product = await productService.GetProductByNameAsync(name);
                if (product != null)
                {
                    product.CurrentlyAvailableUnits = await stockReservationService.GetCurrentlyAvailableUnits(product.Id);
                }
                return product;
            }
        }

        /// <summary>
        /// Gets products according to filter and required page
        /// </summary>
        /// <param name="filter">products filter</param>
        /// <param name="includeCurrentlyAvailableUnits">Include number of currently available units for each product</param>
        /// <returns></returns>
        public async Task<QueryResultDto<ProductDto, ProductFilterDto>> GetProductsAsync(ProductFilterDto filter, bool includeCurrentlyAvailableUnits = true)
        {
            using (UnitOfWorkProvider.Create())
            {
                if (filter.CategoryIds == null && filter.CategoryNames != null)
                {
                    filter.CategoryIds = await categoryService.GetCategoryIdsByNamesAsync(filter.CategoryNames);
                }
                var productListQueryResult = await productService.ListProductsAsync(filter);
                if (!includeCurrentlyAvailableUnits)
                {
                    return productListQueryResult;
                }
                foreach (var product in productListQueryResult.Items)
                {
                    product.CurrentlyAvailableUnits = await stockReservationService.GetCurrentlyAvailableUnits(product.Id);
                }
                return productListQueryResult;
            }
        }

        /// <summary>
        /// Creates product with category that corresponds with given name
        /// </summary>
        /// <param name="product">product</param>
        /// <param name="categoryName">category name</param>
        public async Task<Guid> CreateProductWithCategoryNameAsync(ProductDto product, string categoryName)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                product.CategoryId = (await categoryService.GetCategoryIdsByNamesAsync(categoryName)).FirstOrDefault();
                var productId = productService.Create(product);
                await uow.Commit();
                return productId;
            }
        }

        /// <summary>
        /// Updates product
        /// </summary>
        /// <param name="productDto">Product details</param>
        public async Task<bool> EditProductAsync(ProductDto productDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if ((await productService.GetAsync(productDto.Id, false)) == null)
                {
                    return false;
                }
                await productService.Update(productDto);
                await uow.Commit();
                return true;
            }
        }

        /// <summary>
        /// Deletes product with given Id
        /// </summary>
        /// <param name="id">Id of the product to delete</param>
        public async Task<bool> DeleteProductAsync(Guid id)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if ((await productService.GetAsync(id, false)) == null)
                {
                    return false;
                }
                productService.DeleteProduct(id);
                await uow.Commit();
                return true;
            }
        }

        #endregion

        #region CategoriesManagement

        /// <summary>
        /// Gets category according to ID
        /// </summary>
        /// <param name="categoryId">category ID</param>
        /// <returns>The category</returns>
        public async Task<CategoryDto> GetCategoryAsync(Guid categoryId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await categoryService.GetAsync(categoryId);
            }
        }

        /// <summary>
        /// Gets ids of the categories with the corresponding names
        /// </summary>
        /// <param name="names">names of the categories</param>
        /// <returns>ids of categories with specified name</returns>
        public async Task<Guid[]> GetProductCategoryIdsByNamesAsync(params string[] names)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await categoryService.GetCategoryIdsByNamesAsync(names);
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>All available categories</returns>
        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            using (UnitOfWorkProvider.Create())
            {
                return (await categoryService.ListAllAsync()).Items;
            }
        }

        #endregion
    }
}
