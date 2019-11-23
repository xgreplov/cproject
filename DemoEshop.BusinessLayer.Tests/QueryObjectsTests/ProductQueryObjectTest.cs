using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects;
using DemoEshop.BusinessLayer.Tests.QueryObjectsTests.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure.Query.Predicates;
using DemoEshop.Infrastructure.Query.Predicates.Operators;
using NUnit.Framework;

namespace DemoEshop.BusinessLayer.Tests.QueryObjectsTests
{
    [TestFixture]
    public class ProductQueryObjectTest
    {
        [Test]
        public async Task ApplyWhereClause_SimpleFilterWithMinimalPrice_ReturnsCorrectSimplePredicate()
        {
            var mockManager = new QueryMockManager();
            var expectedPredicate = new SimplePredicate(nameof(Song.Price), ValueComparingOperator.GreaterThanOrEqual, 1000m);
            var mapperMock = mockManager.ConfigureMapperMock<Song, ProductDto, ProductFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Song>();
            var productQueryObject = new ProductQueryObject(mapperMock.Object, queryMock.Object);

            var unused = await productQueryObject.ExecuteQuery(new ProductFilterDto{MinimalPrice = 1000});

            Assert.AreEqual(mockManager.CapturedPredicate, expectedPredicate);
        }

        [Test]
        public async Task ApplyWhereClause_SimpleFilterWithMinimalAndMaximalPrice_ReturnsCorrectCompositePredicate()
        {
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(new List<IPredicate>{
                    new SimplePredicate(nameof(Song.Price), ValueComparingOperator.GreaterThanOrEqual, 1000m),
                    new SimplePredicate(nameof(Song.Price), ValueComparingOperator.LessThanOrEqual, 3000m)});
            var mapperMock = mockManager.ConfigureMapperMock<Song, ProductDto, ProductFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Song>();
            var productQueryObject = new ProductQueryObject(mapperMock.Object, queryMock.Object);

            var unused = await productQueryObject.ExecuteQuery(new ProductFilterDto { MinimalPrice = 1000, MaximalPrice = 3000});

            Assert.AreEqual(mockManager.CapturedPredicate, expectedPredicate);
        }

        [Test]
        public async Task ApplyWhereClause_ComplexFilterWithMinimalPriceAndCategoryId_ReturnsCorrectCompositePredicate()
        {
            var categoryIds = new[] { Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f")};
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(new List<IPredicate>{
                new CompositePredicate(new List<IPredicate>{
                    new SimplePredicate(nameof(Song.CategoryId), ValueComparingOperator.Equal, categoryIds.First())}, LogicalOperator.OR),
                new SimplePredicate(nameof(Song.Price), ValueComparingOperator.GreaterThanOrEqual, 7000m)});
            var mapperMock = mockManager.ConfigureMapperMock<Song, ProductDto, ProductFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Song>();
            var productQueryObject = new ProductQueryObject(mapperMock.Object, queryMock.Object);

            var unused = await productQueryObject.ExecuteQuery(new ProductFilterDto { MinimalPrice = 7000, CategoryIds = categoryIds });

            Assert.AreEqual(mockManager.CapturedPredicate, expectedPredicate);
        }

        [Test]
        public async Task ApplyWhereClause_ComplexFilterWithSeveralConditions_ReturnsCorrectCompositePredicate()
        {
            const string searchedName = "Samsung";
            var categoryIds = new[] {Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f"), Guid.Parse("aa04dc64-5c07-40fe-a916-175165b9b90f")};
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(new List<IPredicate>{
                new CompositePredicate(new List<IPredicate>{
                    new SimplePredicate(nameof(Song.CategoryId), ValueComparingOperator.Equal, categoryIds.First()),
                    new SimplePredicate(nameof(Song.CategoryId), ValueComparingOperator.Equal, categoryIds.Last())}, LogicalOperator.OR),
                new SimplePredicate(nameof(Song.Name), ValueComparingOperator.StringContains, searchedName),
                new CompositePredicate(new List<IPredicate>{
                    new SimplePredicate(nameof(Song.Price), ValueComparingOperator.GreaterThanOrEqual, 1000m),
                    new SimplePredicate(nameof(Song.Price), ValueComparingOperator.LessThanOrEqual, 3000m)})
            });
            var mapperMock = mockManager.ConfigureMapperMock<Song, ProductDto, ProductFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Song>();
            var productQueryObject = new ProductQueryObject(mapperMock.Object, queryMock.Object);

            var unused = await productQueryObject.ExecuteQuery(new ProductFilterDto { MinimalPrice = 1000, MaximalPrice = 3000, SearchedName = searchedName, CategoryIds = categoryIds});

            Assert.AreEqual(mockManager.CapturedPredicate, expectedPredicate);
        }
    }
}
