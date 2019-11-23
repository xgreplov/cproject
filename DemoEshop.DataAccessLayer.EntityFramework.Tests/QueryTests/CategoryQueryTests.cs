using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure.Query;
using DemoEshop.Infrastructure.Query.Predicates;
using DemoEshop.Infrastructure.Query.Predicates.Operators;
using DemoEshop.Infrastructure.UnitOfWork;
using NUnit.Framework;

namespace DemoEshop.DataAccessLayer.EntityFramework.Tests.QueryTests
{
    [TestFixture]
    public class CategoryQueryTests
    {
        private readonly IUnitOfWorkProvider unitOfWorkProvider = Initializer.Container.Resolve<IUnitOfWorkProvider>();

        private readonly Guid? smartphonesCategoryId = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b90f");
        private readonly Guid androidCategoryId = Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f");

        private readonly Guid iOSCategoryId = Guid.Parse("aa04dc64-5c07-40fe-a916-175165b9b90f");
        
        [Test]
        public async Task ExecuteAsync_SimpleWherePredicate_ReturnsCorrectQueryResult()
        {
            QueryResult<Album> actualQueryResult;
            var categoryQuery = Initializer.Container.Resolve<IQuery<Album>>();
            var expectedQueryResult = new QueryResult<Album>(new List<Album>{new Album
            {
                Id = androidCategoryId, Name = "Android", ParentId = smartphonesCategoryId
            }, new Album
            {
                Id = iOSCategoryId, Name = "iOS", ParentId = smartphonesCategoryId
            }}, 2);
            
            var predicate = new SimplePredicate(nameof(Album.ParentId), ValueComparingOperator.Equal, smartphonesCategoryId);
            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await categoryQuery.Where(predicate).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        [Test]
        public async Task ExecuteAsync_ComplexWherePredicate_ReturnsCorrectQueryResult()
        {
            QueryResult<Album> actualQueryResult;
            var categoryQuery = Initializer.Container.Resolve<IQuery<Album>>();
            var expectedQueryResult = new QueryResult<Album>(new List<Album>{new Album
            {
                Id = androidCategoryId, Name = "Android", ParentId = smartphonesCategoryId
            }}, 1);
            
            var predicate = new CompositePredicate(new List<IPredicate>
            {
                new SimplePredicate(nameof(Album.ParentId), ValueComparingOperator.Equal, smartphonesCategoryId),
                new CompositePredicate(new List<IPredicate>
                {
                    new SimplePredicate(nameof(Album.Name), ValueComparingOperator.Equal, "Android"),
                    new SimplePredicate(nameof(Album.Name), ValueComparingOperator.Equal, "Windows 10")
                }, LogicalOperator.OR)
            });
            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await categoryQuery.Where(predicate).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        [Test]
        public async Task ExecuteAsync_RatingAllCategoriesByName_ReturnsCorrectlyOrderedQueryResult()
        {
            QueryResult<Album> actualQueryResult;
            var categoryQuery = Initializer.Container.Resolve<IQuery<Album>>();
            var expectedQueryResult = new QueryResult<Album>(new List<Album>{new Album
            {
                Id = androidCategoryId, Name = "Android", ParentId = smartphonesCategoryId
            }, new Album
            {
                Id = iOSCategoryId, Name = "iOS", ParentId = smartphonesCategoryId
            }, new Album
            {
                Id = smartphonesCategoryId.Value, Name = "Smartphones", ParentId = null
            }}, 3);

            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await categoryQuery.SortBy(nameof(Album.Name), true).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        [Test]
        public async Task ExecuteAsync_RetrieveSecondCategoriesPage_ReturnsCorrectPage()
        {
            const int pageSize = 2;
            const int requestedPage = 2;
            QueryResult<Album> actualQueryResult;
            var categoryQuery = Initializer.Container.Resolve<IQuery<Album>>();
            var expectedQueryResult = new QueryResult<Album>(new List<Album>{new Album
            {
                Id = iOSCategoryId, Name = "iOS", ParentId = smartphonesCategoryId
            }}, 1, pageSize, requestedPage);

            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await categoryQuery.Page(requestedPage, pageSize).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }
    }
}
