using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades;
using DemoEshop.WebApi.Models.Products;

namespace DemoEshop.WebApi.Controllers
{
    public class ProductsController : ApiController
    {
        public ProductFacade ProductFacade { get; set; }

        /// <summary>
        /// Query products according to given query parameters, example URL call: http://localhost:56118/api/Products/Query?sort=name&asc=true&name=Samsung&minimalPrice=5000&maximalPrice=23000&category=android&category=ios
        /// </summary>
        /// <param name="sort">Name of the product attribute (e.g. "name", "price", ...) to sort according to</param>
        /// <param name="asc">Sort product collection in ascending manner</param>
        /// <param name="name">Product name (can be only partial: "Galaxy", "Lumia", ...)</param>
        /// <param name="minimalPrice">Minimal product price</param>
        /// <param name="maximalPrice">Maximal product price</param>
        /// <param name="category">Product category names, currently supported are: "android", "windows 10" and "ios"</param>
        /// <returns>Collection of products, satisfying given query parameters.</returns>       
        [HttpGet, Route("api/Products/Query")]
        public async Task<IEnumerable<ProductDto>> Query(string sort = null, bool asc = true, 
            string name = null, decimal minimalPrice = 0m, decimal maximalPrice = decimal.MaxValue, 
            [FromUri] string[] category = null)
        {
            var filter = new ProductFilterDto
            {
                SortCriteria = sort,
                SortAscending = asc,
                CategoryNames = category,
                SearchedName = name,
                MinimalPrice = minimalPrice,
                MaximalPrice = maximalPrice
            };
            var products = (await ProductFacade.GetProductsAsync(filter, false)).Items;
            foreach (var product in products)
            {
                product.Id = Guid.Empty;
            }
            return products;
        }

        /// <summary>
        /// Gets product info (including currently available units) for product with given name, 
        /// example URL call: http://localhost:56118/api/Products/Get?name=lg
        /// </summary>
        /// <param name="name">Product name ("microsoft lumia 650", ...)</param>
        /// <returns>Complete product info.</returns>
        public async Task<ProductDto> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var product = await ProductFacade.GetProductAsync(name);         
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            product.Id = Guid.Empty;
            return product;
        }

        /// <summary>
        /// Creates product, example URL call can be found in test folder.
        /// </summary>
        /// <param name="model">Created product details.</param>
        /// <returns>Message describing the action result.</returns>
        public async Task<string> Post([FromBody]ProductCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var productId = await ProductFacade.CreateProductWithCategoryNameAsync(model.Product, model.CategoryName);
            if (productId.Equals(Guid.Empty))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return $"Created product with id: {productId}, within {model.CategoryName} category.";
        }

        /// <summary>
        /// Updates product with given id, example URL call can be found in test folder.
        /// </summary>
        /// <param name="id">Id of the product to update.</param>
        /// <param name="product">Product to update</param>
        /// <returns>Message describing the action result.</returns>
        public async Task<string> Put(Guid id, [FromBody]ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var success = await ProductFacade.EditProductAsync(product);
            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return $"Updated product with id: {id}";
        }

        /// <summary>
        /// Deletes product with given id ("aa05dc64-5c07-40fe-a916-175165b9b90f", "aa06dc64-5c07-40fe-a916-175165b9b90f", ...),
        /// example URL call can be found in test folder.
        /// </summary>
        /// <param name="id">Id of the product to delete.</param>
        /// <returns>Message describing the action result.</returns>
        public async Task<string> Delete(Guid id)
        {
            var success = await ProductFacade.DeleteProductAsync(id);
            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return $"Deleted product with id: {id}";
        }
    }
}
