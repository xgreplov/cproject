using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.BusinessLayer.Services.Categories;
using DemoEshop.BusinessLayer.Services.Products;
using DemoEshop.BusinessLayer.Services.Reservation;
using DemoEshop.BusinessLayer.Tests.FacadesTests.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.Query;
using Moq;
using NUnit.Framework;

namespace DemoEshop.BusinessLayer.Tests.FacadesTests
{
    [TestFixture]
    public class ProductFacadeTests
    {
        [Test]
        public async Task GetProductAsync_ExistingProductWithoutReservations_ReturnsCorrectCustomer()
        {
            var productId = Guid.NewGuid();
            const string productName = "Samsung Galaxy S8";
            const int storedAmount = 3;
            var expectedProductDto = new ProductDto
            {
                Id = productId,
                StoredUnits = storedAmount,
                CurrentlyAvailableUnits = storedAmount,
                Name = productName
            };
            var expectedProduct = new Product
            {
                Id = productId,
                StoredUnits = storedAmount,
                Name = productName
            };
            var mockManager = new FacadeMockManager();
            var productRepositoryMock = mockManager.ConfigureGetRepositoryMock(expectedProduct);
            var categoryRepositoryMock = mockManager.ConfigureRepositoryMock<Category>();
            var productQueryMock = mockManager.ConfigureQueryObjectMock<ProductDto, Product, ProductFilterDto>(null);
            var categoryQueryMock = mockManager.ConfigureQueryObjectMock<CategoryDto, Category, CategoryFilterDto>(null);
            var productFacade = await CreateProductFacade(productQueryMock, productRepositoryMock, categoryRepositoryMock, categoryQueryMock);

            var actualProductDto = await productFacade.GetProductAsync(productId);

            Assert.AreEqual(actualProductDto, expectedProductDto);
        }

        [Test]
        public async Task GetProductAsync_ExistingProductWithReservations_ReturnsCorrectCustomerAsync()
        {
            var productId = Guid.NewGuid();
            const string productName = "Samsung Galaxy S8";
            const int storedAmount = 3;
            const int reservedAmount = 2;
            var expectedProductDto = new ProductDto
            {
                Id = productId,
                StoredUnits = storedAmount,
                CurrentlyAvailableUnits = storedAmount - reservedAmount,
                Name = productName
            };
            var expectedProduct = new Product
            {
                Id = productId,
                StoredUnits = storedAmount,
                Name = productName
            };
            var productReservation = new ProductReservationDto
            {
                CustomerId = Guid.NewGuid(),
                Expiration = DateTime.Now.Add(new TimeSpan(1, 0, 0)),
                ProductId = productId,
                ReservedAmount = reservedAmount
            };
            var mockManager = new FacadeMockManager();
            var productRepositoryMock = mockManager.ConfigureGetRepositoryMock(expectedProduct);
            var categoryRepositoryMock = mockManager.ConfigureRepositoryMock<Category>();
            var productQueryMock = mockManager.ConfigureQueryObjectMock<ProductDto, Product, ProductFilterDto>(null);
            var categoryQueryMock = mockManager.ConfigureQueryObjectMock<CategoryDto, Category, CategoryFilterDto>(null);
            var productFacade = await CreateProductFacade(productQueryMock, productRepositoryMock, categoryRepositoryMock, categoryQueryMock, productReservation);

            var actualProductDto = await productFacade.GetProductAsync(productId);

            Assert.AreEqual(actualProductDto, expectedProductDto);
        }

        [Test]
        public async Task GetProductsAsync_ExistingProduct_ReturnsCorrectCustomer()
        {
            const int storedAmount = 10;
            const int reservedAmount = 3;
            var firstProductId = Guid.NewGuid();
            var secondProductId = Guid.NewGuid();
            var returnedResult = new QueryResultDto<ProductDto, ProductFilterDto>
            {
                Filter = new ProductFilterDto(),
                Items = new List<ProductDto> {
                    new ProductDto{Id = firstProductId, StoredUnits = storedAmount},
                    new ProductDto { Id = secondProductId, StoredUnits = storedAmount} },
                TotalItemsCount = 2
            };
            var productReservation = new ProductReservationDto
            {
                CustomerId = Guid.NewGuid(),
                Expiration = DateTime.Now.Add(new TimeSpan(1, 0, 0)),
                ProductId = firstProductId,
                ReservedAmount = reservedAmount
            };

            var expectedResult = new List<ProductDto> {
                new ProductDto{Id = firstProductId, StoredUnits = storedAmount, CurrentlyAvailableUnits = storedAmount - reservedAmount},
                new ProductDto { Id = secondProductId, StoredUnits = storedAmount, CurrentlyAvailableUnits = storedAmount} };


            var mockManager = new FacadeMockManager();
            var productRepositoryMock = mockManager.ConfigureRepositoryMock<Product>();
            var categoryRepositoryMock = mockManager.ConfigureRepositoryMock<Category>();
            var productQueryMock = mockManager.ConfigureQueryObjectMock<ProductDto, Product, ProductFilterDto>(returnedResult);
            var categoryQueryMock = mockManager.ConfigureQueryObjectMock<CategoryDto, Category, CategoryFilterDto>(null);
            var productFacade = await CreateProductFacade(productQueryMock, productRepositoryMock, categoryRepositoryMock, categoryQueryMock, productReservation);

            productQueryMock.Setup(query => query.ExecuteQuery(It.IsAny<ProductFilterDto>())).ReturnsAsync(returnedResult);
            productRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Product {StoredUnits = storedAmount});
            
            var actualResult = (await productFacade.GetProductsAsync(new ProductFilterDto())).Items;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task CreateProductWithCategoryNameAsync_ExistingProduct_ReturnsCorrectCustomer()
        {
            const string categoryName = "Android";
            var categoryId = Guid.NewGuid();
            var returnedResult = new QueryResultDto<CategoryDto, CategoryFilterDto>
                {
                    Items = new List<CategoryDto> {new CategoryDto {Id = categoryId}}
                };

            var mockManager = new FacadeMockManager();
            var productRepositoryMock = mockManager.ConfigureCreateRepositoryMock<Product>(nameof(Product.CategoryId));
            var categoryRepositoryMock = mockManager.ConfigureRepositoryMock<Category>();
            var productQueryMock = mockManager.ConfigureQueryObjectMock<ProductDto, Product, ProductFilterDto>(null);
            var categoryQueryMock = mockManager.ConfigureQueryObjectMock<CategoryDto, Category, CategoryFilterDto>(returnedResult);
            var productFacade = await CreateProductFacade(productQueryMock, productRepositoryMock, categoryRepositoryMock, categoryQueryMock);

            var unused = await productFacade.CreateProductWithCategoryNameAsync(new ProductDto {Id = Guid.NewGuid()}, categoryName);

            Assert.AreEqual(categoryId, mockManager.CapturedCreatedId);
        }

        private static async Task<ProductFacade> CreateProductFacade(Mock<QueryObjectBase<ProductDto, Product, ProductFilterDto, IQuery<Product>>> productQueryMock, Mock<IRepository<Product>> productRepositoryMock, Mock<IRepository<Category>> categoryRepositoryMock, Mock<QueryObjectBase<CategoryDto, Category, CategoryFilterDto, IQuery<Category>>> categoryQueryMock, ProductReservationDto reservation = null)
        {
            var uowMock = FacadeMockManager.ConfigureUowMock();
            var mapperMock = FacadeMockManager.ConfigureRealMapper();
            var productService = new ProductService(mapperMock, productQueryMock.Object, productRepositoryMock.Object);
            var categoryService = new CategoryService(mapperMock, categoryRepositoryMock.Object, categoryQueryMock.Object);
            var reservationService = new ReservationService(mapperMock, productRepositoryMock.Object);
            if (reservation != null)
            {
                await reservationService.ReserveProduct(reservation);
            }
            var productFacade = new ProductFacade(uowMock.Object, categoryService, productService, reservationService);
            return productFacade;
        }
    }
}
