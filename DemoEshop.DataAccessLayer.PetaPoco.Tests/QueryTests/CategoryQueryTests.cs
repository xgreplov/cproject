﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.DataAccessLayer.PetaPoco.Entities;
using DemoEshop.Infrastructure.Query;
using DemoEshop.Infrastructure.Query.Predicates;
using DemoEshop.Infrastructure.Query.Predicates.Operators;
using DemoEshop.Infrastructure.UnitOfWork;
using NUnit.Framework;

namespace DemoEshop.DataAccessLayer.PetaPoco.Tests.QueryTests
{
    [TestFixture]
    public class CategoryQueryTests
    {
        private readonly IUnitOfWorkProvider unitOfWorkProvider = Initializer.Container.Resolve<IUnitOfWorkProvider>();

        private readonly Guid smartphonesCategoryId = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b90f");

        private readonly Guid androidCategoryId = Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f");

        private readonly Guid iOSCategoryId = Guid.Parse("aa04dc64-5c07-40fe-a916-175165b9b90f");
        
        [Test]
        public async Task ExecuteAsync_SimpleWherePredicate_ReturnsCorrectQueryResult()
        {
            QueryResult<Category> actualQueryResult;
            var categoryQuery = Initializer.Container.Resolve<IQuery<Category>>();
            var expectedQueryResult = new QueryResult<Category>(new List<Category>{new Category
            {
                Id = androidCategoryId, Name = "Android", ParentId = smartphonesCategoryId
            }, new Category
            {
                Id = iOSCategoryId, Name = "iOS", ParentId = smartphonesCategoryId
            }}, 2);
            
            var predicate = new SimplePredicate(nameof(Category.ParentId), ValueComparingOperator.Equal, smartphonesCategoryId);
            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await categoryQuery.Where(predicate).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        [Test]
        public async Task ExecuteAsync_ComplexWherePredicate_ReturnsCorrectQueryResult()
        {
            QueryResult<Category> actualQueryResult;
            var categoryQuery = Initializer.Container.Resolve<IQuery<Category>>();
            var expectedQueryResult = new QueryResult<Category>(new List<Category>{new Category
            {
                Id = androidCategoryId, Name = "Android", ParentId = smartphonesCategoryId
            }}, 1);
            
            var predicate = new CompositePredicate(new List<IPredicate>
            {
                new SimplePredicate(nameof(Category.ParentId), ValueComparingOperator.Equal, smartphonesCategoryId),
                new CompositePredicate(new List<IPredicate>
                {
                    new SimplePredicate(nameof(Category.Name), ValueComparingOperator.Equal, "Android"),
                    new SimplePredicate(nameof(Category.Name), ValueComparingOperator.Equal, "Windows 10")
                }, LogicalOperator.OR)
            });
            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await categoryQuery.Where(predicate).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        [Test]
        public async Task ExecuteAsync_OrderAllCategoriesByName_ReturnsCorrectlyOrderedQueryResult()
        {
            QueryResult<Category> actualQueryResult;
            var categoryQuery = Initializer.Container.Resolve<IQuery<Category>>();
            var expectedQueryResult = new QueryResult<Category>(new List<Category>{new Category
            {
                Id = androidCategoryId, Name = "Android", ParentId = smartphonesCategoryId
            }, new Category
            {
                Id = iOSCategoryId, Name = "iOS", ParentId = smartphonesCategoryId
            }, new Category
            {
                Id = smartphonesCategoryId, Name = "Smartphones", ParentId = null
            }}, 3);

            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await categoryQuery.SortBy(nameof(Category.Name), true).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        [Test]
        public async Task ExecuteAsync_RetrieveSecondCategoriesPage_ReturnsCorrectPage()
        {
            const int pageSize = 2;
            const int requestedPage = 2;
            QueryResult<Category> actualQueryResult;
            var categoryQuery = Initializer.Container.Resolve<IQuery<Category>>();
            var expectedQueryResult = new QueryResult<Category>(new List<Category>{new Category
            {
                Id = iOSCategoryId, Name = "iOS", ParentId = smartphonesCategoryId
            }}, 3, pageSize, requestedPage);

            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await categoryQuery.Page(requestedPage, pageSize).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }
    }
}
