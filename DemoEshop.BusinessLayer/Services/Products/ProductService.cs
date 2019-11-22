using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.BusinessLayer.Services.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.Query;

namespace DemoEshop.BusinessLayer.Services.Products
{
    public class ProductService : CrudQueryServiceBase<Product, ProductDto, ProductFilterDto>, IProductService
    {
        public ProductService(IMapper mapper, QueryObjectBase<ProductDto, Product, ProductFilterDto, IQuery<Product>> productQuery, IRepository<Product> productRepository)
            : base(mapper, productRepository, productQuery) { }
        
        protected override Task<Product> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, nameof(Product.Category));
        }

        /// <summary>
        /// Gets product with given name
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product with given name</returns>
        public async Task<ProductDto> GetProductByNameAsync(string name)
        {
            var queryResult = await Query.ExecuteQuery(new ProductFilterDto { SearchedName = name });
            return queryResult.Items.SingleOrDefault();
        }

        /// <summary>
        /// Gets products according to given filter
        /// </summary>
        /// <param name="filter">The products filter</param>
        /// <returns>Filtered results</returns>
        public async Task<QueryResultDto<ProductDto, ProductFilterDto>> ListProductsAsync(ProductFilterDto filter)
        {
            return await Query.ExecuteQuery(filter);
        }
    }
}
